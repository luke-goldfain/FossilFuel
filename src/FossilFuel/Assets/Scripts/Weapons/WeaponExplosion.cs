using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplosion : MonoBehaviour
{
    [SerializeField]
    private int damage;

    [SerializeField]
    private float explosionForce;

    private float damageRadius;

    private float timeToStay = 0.4f;
    private float timeExisted;

    private void Start()
    {
        damageRadius = this.transform.localScale.x / 2;

        timeExisted = 0f;
    }

    private void Update()
    {
        timeExisted += Time.deltaTime;

        this.transform.position = this.transform.position + Vector3.zero; // Hacky fix for OnTriggerEnter not working unless this gets position updated

        if (timeExisted > timeToStay)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, damageRadius, 1f);
            other.GetComponent<CharacterHealth>().TakeDamage((int)(damage - (damage * 0.8f * Vector3.Distance(this.transform.position, other.transform.position))));
        }
    }
}
