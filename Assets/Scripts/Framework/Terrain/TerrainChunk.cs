using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class TerrainChunk 
{
	public readonly Vector2 coords;
	public event System.Action<TerrainChunk, bool> onVisibilityChanged;

	TerrainManager terrainManager;

	Bounds bounds;
	GameObject meshObject;
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	LODMesh[] lodMeshes;

	//HeightMap heightMap;
	bool heightMapReceived;
	int previousLODIndex = -1;
	bool hasSetCollider;

	public TerrainSettings terrainSettings
    {
		get
        {
			return terrainManager.terrainSettings;
        }
    }

	public TerrainChunk(Vector2 coords, TerrainManager terrainManager, Transform parent)
	{
		this.coords = coords;
		this.terrainManager = terrainManager;

		Vector2 position = coords * terrainSettings.meshWorldSize;
		bounds = new Bounds(position, Vector2.one * terrainSettings.meshWorldSize);

		meshObject = new GameObject("Terrain Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = terrainSettings.terrainMaterial;

#if UNITY_EDITOR
		SceneVisibilityManager.instance.DisablePicking(meshObject, true);
#endif // UNITY_EDITOR

		meshObject.transform.position = new Vector3(position.x, 0, position.y);
		meshObject.transform.parent = parent;
		SetVisible(false);

		lodMeshes = new LODMesh[terrainSettings.detailLevels.Length];
		for (int i = 0; i < terrainSettings.detailLevels.Length; i++)
		{
			lodMeshes[i] = new LODMesh(terrainSettings.detailLevels[i].lod);
			lodMeshes[i].updateCallback += UpdateTerrainChunk;
			if (i == terrainSettings.colliderLODIndex)
			{
				lodMeshes[i].updateCallback += UpdateCollisionMesh;
			}
		}
	}

	public void Load()
	{
		Vector2 sampleCenter = coords * terrainSettings.meshWorldSize / terrainSettings.meshScale;

#if UNITY_EDITOR
		if (Application.isEditor)
		{
			OnHeightMapReceived(null); // Temp : Remove when below line will be uncommented
			return;
		}
#endif // UNITY_EDITOR

		OnHeightMapReceived(null); // Temp : Remove when below line will be uncommented
		//ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(terrainSettings, sampleCenter), OnHeightMapReceived);
	}

	private void OnHeightMapReceived(object heightMapObject)
	{
		//heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;

		UpdateTerrainChunk();
	}

	public void UpdateTerrainChunk()
	{
		if (heightMapReceived)
		{
			float sqrViewerDstFromNearestEdge = bounds.SqrDistance(terrainManager.viewerPosition);

			bool visible = sqrViewerDstFromNearestEdge <= terrainSettings.sqrMaxViewDistance;

			if (visible)
			{
				int lodIndex = terrainSettings.GetLODIndexFromSqrDistance(sqrViewerDstFromNearestEdge);
				if (lodIndex != previousLODIndex)
				{
					LODMesh lodMesh = lodMeshes[lodIndex];
					if (lodMesh.hasMesh)
					{
						previousLODIndex = lodIndex;
						meshFilter.mesh = lodMesh.mesh;
					}
					else if (!lodMesh.hasRequestedMesh)
					{
						lodMesh.RequestMesh(/*heightMap,*/ terrainSettings);
					}
				}
			}

			if (IsVisible() != visible)
            {
				SetVisible(visible);
				if (onVisibilityChanged != null)
				{
					onVisibilityChanged(this, visible);
				}
			}
		}
	}

	public void UpdateCollisionMesh()
	{
		if (!hasSetCollider)
		{
			float sqrDstFromViewerToEdge = bounds.SqrDistance(terrainManager.viewerPosition);

			if (sqrDstFromViewerToEdge < terrainSettings.detailLevels[terrainSettings.colliderLODIndex].sqrVisibleDstThreshold)
			{
				if (!lodMeshes[terrainSettings.colliderLODIndex].hasRequestedMesh)
				{
					lodMeshes[terrainSettings.colliderLODIndex].RequestMesh(/*heightMap,*/ terrainSettings);
				}
			}

			if (sqrDstFromViewerToEdge < terrainSettings.sqrColliderGenerationDistanceThreshold)
			{
				if (lodMeshes[terrainSettings.colliderLODIndex].hasMesh)
				{
					meshCollider.sharedMesh = lodMeshes[terrainSettings.colliderLODIndex].mesh;
					hasSetCollider = true;
				}
			}
		}
	}

	public void SetVisible(bool visible)
	{
		meshObject.SetActive(visible);
	}

	public bool IsVisible()
	{
		return meshObject.activeSelf;
	}

	private class LODMesh
	{
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		public event System.Action updateCallback;

		public LODMesh(int lod)
		{
			this.lod = lod;
		}

		private void OnMeshDataReceived(object meshDataObject)
		{
			mesh = ((TerrainMeshData)meshDataObject).CreateMesh();
			hasMesh = true;

			updateCallback();
		}

		public void RequestMesh(/*HeightMap heightMap,*/ TerrainSettings terrainSettings)
		{
			hasRequestedMesh = true;

#if UNITY_EDITOR
			if (Application.isEditor)
			{
				OnMeshDataReceived(TerrainGenerator.GenerateTerrainMesh(/*heightMap.values,*/ terrainSettings, lod));
				return;
			}
#endif // UNITY_EDITOR
			
			ThreadedDataRequester.RequestData(() => TerrainGenerator.GenerateTerrainMesh(/*heightMap.values,*/ terrainSettings, lod), OnMeshDataReceived);
		}
	}
}
