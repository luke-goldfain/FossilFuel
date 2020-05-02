using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float explodeTime;

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
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

            this.gameObject.SetActive(false);
        }
    }
}
