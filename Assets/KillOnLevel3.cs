using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnLevel3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalManager.Instance.currentLevel == 4)
        {
            Destroy(gameObject);
        }
    }
}
