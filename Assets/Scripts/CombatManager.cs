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

        if (target.GetDefending())
        {
            damage = Mathf.Round(damage /= 2f);
        }

        if (damage < 0)
        {
            damage = 0;
        }

        AudioManager.instance.PlayAudio("Melee Attack");

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
        Debug.Log(entity.strength+ " damage inflicted to " + target.GetName());
        Debug.Log(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicAttack(BaseClass entity, BaseClass target, int spell)
    {
        float damage = entity.intelligence;

        if (target.GetDefending())
        {
            damage = Mathf.Round(damage /= 1.5f);
        }

        Instantiate(spellPrefabs[spell], target.GetStartingPos(), Quaternion.identity);

        Debug.LogWarning(spellPrefabs[spell].name);
        AudioManager.instance.PlayAudio(spellPrefabs[spell].name);

        if (damage < 0)
        {
            damage = 0;
        }

        entity.UpdateMana();

        target.UpdateHealth(damage, true);
        SpawnDamageText(damage, target, true);
        Debug.Log("Casting spell " + entity.GetSpells(spell));
        Debug.Log(entity.intelligence + " damage inflicted to " + target.GetName());
        Debug.Log(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }

    public void MagicHealing(BaseClass entity, BaseClass target)
    {
        float healing = entity.intelligence;

        Instantiate(spellPrefabs[3], target.GetStartingPos(), Quaternion.identity);

        AudioManager.instance.PlayAudio("Cure");

        entity.UpdateMana();

        target.UpdateHealth(healing, false);
        SpawnDamageText(healing, target, false);
    }

    public void LimitAttack(BaseClass entity, BaseClass target)
    {
        AudioManager.instance.PlayAudio("Limit");
        target.UpdateHealth(entity.LimitDamage, true);
        SpawnDamageText(entity.LimitDamage, target, true);
    }

    public void SpawnDamageText(float damage, BaseClass target, bool isDamage)
    {
        GameObject dmgText = Instantiate(damageText);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.GetObj().transform.position);

        dmgText.transform.SetParent(canvas.transform, false);
        dmgText.transform.position = screenPos;
        dmgText.GetComponentInChildren<TMP_Text>().text = "" + damage;
    }
}
