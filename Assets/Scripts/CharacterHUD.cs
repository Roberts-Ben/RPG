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
    }
}
