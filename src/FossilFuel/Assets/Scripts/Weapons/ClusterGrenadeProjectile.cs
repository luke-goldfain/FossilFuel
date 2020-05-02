using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterGrenadeProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float explodeTime;

    [SerializeField]
    private int numberOfClusters;

    public bool IsParentGrenade;

    private float explodeTimer;

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

        explodeTimer += Time.deltaTime;

        if (explodeTimer >= explodeTime)
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
            }

            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9 && !IsParentGrenade)
        {
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

            this.gameObject.SetActive(false);
        }
    }
}
