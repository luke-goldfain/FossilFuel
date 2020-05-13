using System;
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

    private bool dmgTextEnabled;

    private CameraBillboard billboardScript;

    private void Start()
    {
        billboardScript = this.gameObject.GetComponent<CameraBillboard>();

        currentHealth = maxHealth;

        hpText.text = currentHealth.ToString();

        dmgText.text = "";
        dmgTextEnabled = false;

        initialDmgTransform = dmgText.rectTransform;
    }

    private void Update()
    {
        if (dmgTextEnabled)
        {
            UpdateDriftDmgUpwards();
        }

        UpdateBillboardText();
    }

    private void UpdateBillboardText()
    {
        Quaternion rot = billboardScript.GetUnflippedRotation();

        dmgText.gameObject.transform.rotation = rot;
        hpText.gameObject.transform.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Instantly die if this character hits a hazard (for example, the oil hazard below the ground)
        if (other.tag == "Hazard")
        {
            TakeDamage(this.currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
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
        TurnCharacter dataChar = this.gameObject.GetComponent<UnityCharacterTurnInfo>().DataCharacter;

        // If the character dying is the active character, in case they died without firing, set the turn time to 7 seconds
        if (dataChar.CurrentState == CharacterState.active)
        {
            TurnTimer t = TurnTimer.Instance;

            t.SetTurnTime(7f);
        }

        dataChar.Die(); // At the end of the turn, the character will be set inactive since its state is now "dead"
    }
}
