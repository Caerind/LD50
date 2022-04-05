using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxComponent : MonoBehaviour
{
    [SerializeField] private GameObject bagPrefab = null;

    public void Explose()
    {
        GlobalManager.Instance.ActivateAlarm();

        if (bagPrefab != null)
        {
            GameObject bag = Instantiate(bagPrefab);
            bag.transform.position = transform.position;

            BagComponent bagComp = bag.GetComponent<BagComponent>();
            bagComp.FromStrongbox();
        }

        Destroy(gameObject);

        AudioManager.PlaySound("vault");
    }
}
