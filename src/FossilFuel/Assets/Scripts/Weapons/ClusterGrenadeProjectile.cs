using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterGrenadeProjectile : AbstractProjectile
{
    //[SerializeField]
    //private GameObject explosionPrefab;

    //[SerializeField]
    //private float explodeTime;

    //private float explodeTimer;

    [SerializeField]
    private int numberOfClusters;

    public bool IsParentGrenade;

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
            if (IsParentGrenade)
            {
                for (int i = 0; i < numberOfClusters; i++)
                {
                    GameObject currentChildGrenade = Instantiate(this.gameObject, this.transform.position + (Vector3.up * 0.5f), this.transform.rotation);

                    currentChildGrenade.GetComponent<ClusterGrenadeProjectile>().IsParentGrenade = false;

                    float randX = UnityEngine.Random.Range(-0.5f, 0.5f);
                    float randZ = UnityEngine.Random.Range(-0.5f, 0.5f);

                    currentChildGrenade.GetComponent<Rigidbody>().velocity = new Vector3(randX, 2f, randZ);
                }

                this.Explode();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9 && !IsParentGrenade)
        {
            this.Explode();
        }
    }
}
