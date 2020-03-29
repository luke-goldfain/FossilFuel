using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8) // Ignore objects on the "barrier" layer
        {
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

            this.gameObject.SetActive(false);
        }
    }
}
