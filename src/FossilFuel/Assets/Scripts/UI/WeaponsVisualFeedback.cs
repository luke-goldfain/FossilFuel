using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsVisualFeedback : MonoBehaviour
{
    [SerializeField]
    private Image weaponImage;

    [SerializeField]
    private TextMeshProUGUI weaponText;

    private TurnManager turnMgr;

    private TurnCharacter activeChar;

    private CharacterSliceMovement activeCharSliceMov;

    private Sprite activeCharWeaponSprite;

    private void Awake()
    {
        if (turnMgr == null)
        {
            turnMgr = TurnManager.Instance;
        }

        TurnManager.NotifyOfSwitch += UpdateFocusedPlayer;
    }

    public void UpdateFocusedPlayer()
    {
        foreach (TurnCharacter ch in turnMgr.ActiveCharacters)
        {
            if (ch.CurrentState == CharacterState.active)
            {
                activeChar = ch;

                activeCharSliceMov = ch.CharGO.GetComponent<CharacterSliceMovement>();

                break;
            }
        }
    }

    private void Update()
    {
        UpdateWeaponDisplay();
    }

    public void UpdateWeaponDisplay()
    {
        if (activeChar != null)
        {
            activeCharWeaponSprite = activeCharSliceMov.CurrentWeapon.GetComponent<SpriteRenderer>().sprite;

            weaponImage.sprite = activeCharWeaponSprite;

            weaponText.text = activeCharSliceMov.CurrentWeapon.GetComponent<AbstractWeapon>().weaponName;
        }
    }
}
