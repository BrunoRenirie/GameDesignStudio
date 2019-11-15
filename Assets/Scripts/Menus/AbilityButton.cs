using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Abilities
{
    None,
    Shoot,
    Ability2,
    Ability3,
    Ability4
}
public class AbilityButton : MonoBehaviour
{
    [SerializeField] private Abilities _CurrentAbility;
    [SerializeField] private TextMeshProUGUI _AbilityText;

    void Start()
    {
        _AbilityText = GetComponentInChildren<TextMeshProUGUI>();

        switch (_CurrentAbility)
        {
            case Abilities.Shoot:
                _AbilityText.text = Abilities.Shoot.ToString();
                break;
            case Abilities.Ability2:
                _AbilityText.text = Abilities.Ability2.ToString();
                break;
            case Abilities.Ability3:
                _AbilityText.text = Abilities.Ability3.ToString();
                break;
            case Abilities.Ability4:
                _AbilityText.text = Abilities.Ability4.ToString();
                break;
            case Abilities.None:
                gameObject.SetActive(false);
                break;
        }
    }


    void Update()
    {

    }

    public void PressButton()
    {
        switch (_CurrentAbility)
        {
            case Abilities.Shoot:

                break;
            case Abilities.Ability2:

                break;
            case Abilities.Ability3:

                break;
            case Abilities.Ability4:

                break;
        }
    }
}