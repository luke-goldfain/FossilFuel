using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMissile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

        this.gameObject.SetActive(false);
    }
}
