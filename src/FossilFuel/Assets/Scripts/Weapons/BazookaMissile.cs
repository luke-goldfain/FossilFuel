using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : AbstractProjectile
{
    //[SerializeField]
    //private GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8) // Ignore objects on the "barrier" layer
        {
            this.Explode();
        }
    }
}
