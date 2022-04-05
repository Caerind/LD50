using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainUpdaterEditor : MonoBehaviour
{
#if UNITY_EDITOR
    private TerrainManager terrainManager = null;

    private void Awake()
    {
        terrainManager = GetComponent<TerrainManager>();
    }

    private void Update()
    {
        if (Application.isEditor && terrainManager != null)
        {
            terrainManager.UpdateEditor();
        }
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        GameObject terrain = GameObject.Find("Terrain");
        if (terrain != null)
        {
            TerrainManager terrainManager = terrain.GetComponent<TerrainManager>(); 
            if (terrainManager != null)
            {
                terrainManager.ClearAllChunks();
                terrainManager.UpdateVisibleChunks();
            }
        }
    }
#endif // UNITY_EDITOR
}
