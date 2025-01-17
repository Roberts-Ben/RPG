using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public GameObject damageText;
    public GameObject canvas;

    public List<GameObject> spellPrefabs;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void MeleeAttack(BaseClass entity, BaseClass target)
    {
        float damage = entity.strength;

        if(damage < 0)
        {
            damage = 0;
        }

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
        Debug.LogWarning(entity.strength+ " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicAttack(BaseClass entity, BaseClass target, int spell)
    {
        float damage = entity.intelligence;

        Instantiate(spellPrefabs[spell], target.GetStartingPos(), Quaternion.identity);

        if (damage < 0)
        {
            damage = 0;
        }

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
        Debug.LogWarning("Casting spell " + entity.GetSpells(spell));
        Debug.LogWarning(entity.intelligence + " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicHealing(BaseClass entity, BaseClass target)
    {
        float healing = entity.intelligence;

        Instantiate(spellPrefabs[3], target.GetStartingPos(), Quaternion.identity);

        target.UpdateHealth(healing, false);
        SpawnDamageText(healing, target, false);
    }

    public void LimitAttack(BaseClass entity, BaseClass target)
    {
        target.UpdateHealth(entity.LimitDamage, true);
        SpawnDamageText(entity.LimitDamage, target, true);
    }

    public void SpawnDamageText(float damage, BaseClass target, bool isDamage)
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
