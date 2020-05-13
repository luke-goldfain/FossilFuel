using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractProjectile : MonoBehaviour, IProjectile
{
    [SerializeField]
    public GameObject explosionPrefab;

    [SerializeField]
    public float ExplodeTime;

    [HideInInspector]
    public float ExplodeTimer { get; protected set; }

    public void Explode()
    {
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);

        this.gameObject.SetActive(false);
    }
}
