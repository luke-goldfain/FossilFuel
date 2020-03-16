using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) // TODO: Let em bounce around first
        {
            Die();
        }
    }

    private void Die()
    {
        this.gameObject.SetActive(false); // TODO: Add gravestone, maybe death animation
    }
}
