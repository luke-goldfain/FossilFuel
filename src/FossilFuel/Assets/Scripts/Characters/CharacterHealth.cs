using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private TextMeshPro hpText;

    [SerializeField]
    private TextMeshPro dmgText;

    private RectTransform initialDmgTransform;

    private int currentHealth;

    private UnityGridManager gridMgr;

    private bool dmgTextEnabled;

    private void Start()
    {
        currentHealth = maxHealth;

        hpText.text = currentHealth.ToString();

        dmgText.text = "";
        dmgTextEnabled = false;

        initialDmgTransform = dmgText.rectTransform;

        gridMgr = FindObjectOfType<UnityGridManager>();
    }

    private void Update()
    {
        if (dmgTextEnabled)
        {
            UpdateDriftDmgUpwards();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        hpText.text = currentHealth.ToString();
        dmgText.text = damage.ToString();
        dmgText.GetComponent<MeshRenderer>().enabled = true;
        dmgTextEnabled = true;

        if (currentHealth <= 0) // TODO: Let em bounce around first
        {
            Die();
        }
    }

    void UpdateDriftDmgUpwards()
    {
        dmgText.gameObject.transform.Translate(Vector3.up * 0.01f);

        dmgText.color = new Color(dmgText.color.r, dmgText.color.g, dmgText.color.b, dmgText.color.a - 0.01f);

        // Reset values of damage text and stop coroutine when it goes invisible
        if (dmgText.color.a <= 0f)
        {
            dmgText.GetComponent<MeshRenderer>().enabled = false;
            dmgText.color = new Color(dmgText.color.r, dmgText.color.g, dmgText.color.b, 1f);
            dmgText.rectTransform.position = initialDmgTransform.position;

            dmgTextEnabled = false;
        }
    }

    private void Die()
    {
        gridMgr.ActiveChars.Remove(this.gameObject); // At the end of the turn, this list will be used to set a character inactive
    }
}
