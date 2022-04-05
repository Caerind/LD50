using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstantsComponent : Singleton<GameConstantsComponent>
{
    [SerializeField] private float nodesDefaultZ = -1.0f;
    public static float GetNodesDefaultZ() { return Instance.nodesDefaultZ; }

    [SerializeField] private float linksDefaultZ = -0.95f;
    public static float GetLinksDefaultZ() { return Instance.linksDefaultZ; }

    protected GameConstantsComponent()
    {
    }
}
