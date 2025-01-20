using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHUD : MonoBehaviour
{
    public BaseClass characterReference;

    public TMP_Text characterName;
    public TMP_Text characterHP;
    public TMP_Text characterMP;

    void Awake()
    {
        characterName.text = characterReference.GetName();
        characterHP.text = "" + characterReference.GetHealth();
        characterMP.text = "" + characterReference.GetMana();
    }

    public void UpdateStats()
    {
        characterHP.text = "" + characterReference.GetHealth();
        characterMP.text = "" + characterReference.GetMana();

        float healthPercentage = (float)characterReference.GetHealth() / (float)characterReference.GetMaxHealth() * 100;
        float manaPercentage = (float)characterReference.GetMana() / (float)characterReference.GetMaxMana() * 100;

        if (healthPercentage <= 33) // Set health to red if near death
        {
            characterHP.color = Color.red;
        }
        else if (healthPercentage <= 66) // Set health to yellow if weak
        {
            characterHP.color = Color.yellow;
        }
        else
        {
            characterHP.color = Color.white;
        }

        if (manaPercentage <= 33) // Set mana to red if near empty
        {
            characterMP.color = Color.red;
        }
        else if (manaPercentage <= 66) // Set mana to yellow if low
        {
            characterMP.color = Color.yellow;
        }
        else
        {
            characterMP.color = Color.white;
        }
    }
}
