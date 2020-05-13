using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : AbstractProjectile
{
    //[SerializeField]
    //private GameObject explosionPrefab;

    //[SerializeField]
    //private float explodeTime;

    //private float explodeTimer;

    private Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.z = 0;

        rb.velocity = transform.TransformDirection(localVelocity);

        ExplodeTimer += Time.deltaTime;

        if (ExplodeTimer >= ExplodeTime)
        {
            this.Explode();
        }
    }
}
