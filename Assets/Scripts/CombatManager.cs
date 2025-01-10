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

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
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

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
        Debug.LogWarning("Casting spell " + entity.GetSpells(spell));
        Debug.LogWarning(entity.intelligence - target.constitution + " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicHealing(BaseClass entity, BaseClass target)
    {
        int healing = entity.intelligence += target.constitution;

        target.UpdateHealth(healing, false);
        SpawnDamageText(healing, target, false);
    }

    public void LimitAttack(BaseClass entity, BaseClass target)
    {
        target.UpdateHealth(entity.LimitDamage, true);
        SpawnDamageText(entity.LimitDamage, target, true);
    }

    public void SpawnDamageText(int damage, BaseClass target, bool isDamage)
    {
        GameObject dmgText = Instantiate(damageText);
        if (!isDamage)
        {
            dmgText.GetComponentInChildren<TMP_Text>().color = Color.green;
        }
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.GetObj().transform.position);

        dmgText.transform.SetParent(canvas.transform, false);
        dmgText.transform.position = screenPos;
        dmgText.GetComponentInChildren<TMP_Text>().text = "" + damage;
    }
}
