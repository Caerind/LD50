using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteSelector : MonoBehaviour
{
    [System.Serializable]
    public struct SpriteData
    {
        public Sprite sprite;
        public RuntimeAnimatorController controller;
    }

    public List<SpriteData> selector;


    private void Start()
    {
        int pc = PlayerManager.Instance.GetPlayerCount();
        if (pc == 1)
        {
            GetComponent<SpriteRenderer>().sprite = selector[0].sprite;
            GetComponent<Animator>().runtimeAnimatorController = selector[0].controller;
        }
        else if (pc == 2)
        {
            GetComponent<SpriteRenderer>().sprite = selector[1].sprite;
            GetComponent<Animator>().runtimeAnimatorController = selector[1].controller;
        }
    }
}
