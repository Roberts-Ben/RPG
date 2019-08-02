using UnityEngine;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public GameObject damageText;
    public GameObject canvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void MeleeAttack(BaseClass entity, BaseClass target)
    {
        int damage = entity.strength - target.constitution;

        if(damage < 0)
        {
            damage = 0;
        }

        target.UpdateHealth(damage);
        SpawnDamageText(damage, target);
        Debug.LogWarning(entity.strength - target.constitution + " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicAttack(BaseClass entity, BaseClass target, int spell)
    {
        int damage = entity.intelligence - target.constitution;

        if (damage < 0)
        {
            damage = 0;
        }

        target.UpdateHealth(damage);
        SpawnDamageText(damage, target);
        Debug.LogWarning("Casting spell " + entity.GetSpells(spell));
        Debug.LogWarning(entity.intelligence - target.constitution + " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void SpawnDamageText(int damage, BaseClass target)
    {
        GameObject dmgText = Instantiate(damageText);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.GetObj().transform.position);

        dmgText.transform.SetParent(canvas.transform, false);
        dmgText.transform.position = screenPos;
        dmgText.GetComponentInChildren<TMP_Text>().text = "" + damage;
    }
}
