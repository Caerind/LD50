using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspiration from : https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E21/Assets/Scripts/Data/MeshSettings.cs

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
	public const int numSupportedLODs = 5;
	public const int numSupportedChunkSizes = 9;
	public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

	public float colliderGenerationDistanceThreshold = 5f;
	public float sqrColliderGenerationDistanceThreshold
	{
		get
		{
			return colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold;
		}
	}

	public float viewerMoveThresholdForChunkUpdate = 25f;
	public float sqrViewerMoveThresholdForChunkUpdate
	{
		get
		{
			return viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
		}
	}

	[System.Serializable]
	public struct LODInfo
	{
		[Range(0, numSupportedLODs - 1)]
		public int lod;
		public float visibleDstThreshold;

		public float sqrVisibleDstThreshold
		{
			get
			{
				return visibleDstThreshold * visibleDstThreshold;
			}
		}
	}
	public LODInfo[] detailLevels;

	public int GetLODIndexFromSqrDistance(float sqrDistance)
	{
		int lodIndex = 0;
		for (int i = 0; i < detailLevels.Length - 1; ++i)
		{
			if (sqrDistance > detailLevels[i].sqrVisibleDstThreshold)
			{
				lodIndex = i + 1;
			}
			else
			{
				break;
			}
		}
		return lodIndex;
	}

	public float meshScale = 1.0f;

	[Range(0, numSupportedChunkSizes - 1)]
	public int chunkSizeIndex = 0;

	public int colliderLODIndex = 0;

	public Material terrainMaterial;

	// Num verts per line of mesh rendered at LOD = 0. Includes the 2 extra verts that are excluded from final mesh, but used for calculating normals
	public int numVertsPerLine
	{
		get
		{
			return supportedChunkSizes[chunkSizeIndex] + 5;
		}
	}

	public float meshWorldSize
	{
		get
		{
			return (numVertsPerLine - 3) * meshScale;
		}
	}

	public float maxViewDistance
	{
		get
        {
			return detailLevels[detailLevels.Length - 1].visibleDstThreshold;
		}
	}

	public float sqrMaxViewDistance
	{
		get
		{
			float m = maxViewDistance;
			return m * m;
		}
	}

	public int chunksVisibleInViewDst
    {
		get
        {
			return Mathf.RoundToInt(maxViewDistance / meshWorldSize);
		}
    }
}
