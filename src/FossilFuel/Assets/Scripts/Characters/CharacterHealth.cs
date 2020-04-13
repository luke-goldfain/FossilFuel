using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    private int currentHealth;

    private UnityGridManager gridMgr;

    private void Start()
    {
        currentHealth = maxHealth;

        gridMgr = FindObjectOfType<UnityGridManager>();
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
        gridMgr.ActiveChars.Remove(this.gameObject); // At the end of the turn, this list will be used to set a character inactive
    }
}
