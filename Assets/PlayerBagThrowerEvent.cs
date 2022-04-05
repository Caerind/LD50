using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagThrowerEvent : MonoBehaviour
{
    [SerializeField] private GameObject bagThrowPrefab;

    public void CreateThrowedBag()
    {
        GameObject bagThrow = Instantiate(bagThrowPrefab);
        bagThrow.GetComponent<BagComponent>().FromThrow(transform.position, transform.parent.GetComponent<PlayerController>().lastValidMoveInput.normalized, 1.0f);
    }
    public void CreateThrowedBag2()
    {
        GameObject bagThrow = Instantiate(bagThrowPrefab);
        bagThrow.GetComponent<BagComponent>().FromThrow(transform.position, transform.parent.GetComponent<PlayerController>().lastValidMoveInput.normalized, 1.0f);
    }
}
