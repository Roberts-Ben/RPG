using UnityEngine;
using System.Collections.Generic;

public class BaseClass : MonoBehaviour
{
    public enum ROLES
    {
        WARRIOR, // Melee DPS
        ROGUE, // Melee DPS
        MAGE, // Ranged DPS
        ARCHER, // Ranged DPS
        PALADIN, // Tank
        CLERIC // Healer
    }

    public enum SPELLS
    {
        FIRE,
        BLIZZARD,
        THUNDER,
        CURE
    }

    public enum ITEMS
    {
        POTION,
        ELIXER,
        PHOENIXDOWN
    }

    public new string name;
    public ROLES role;

    public float strength;
    public float dexterity;
    public float constitution;
    public float intelligence;
    public float wisdom;

    public bool isAlive = true;
    public bool isDefending = false;

    public int LimitDamage;

    public List<SPELLS> spells;
    public List<ITEMS> items;

    public GameObject shieldObject;

    public float maxHealthPoints;
    public float currentHealthPoints;
    public float maxManaPoints;
    public float currentManaPoints;

    private float spellCost = 5f;

    public GameObject referenceObj;
    public Vector3 startingPosition;

    private void Awake()
    {
        referenceObj = this.gameObject;
        startingPosition = referenceObj.transform.position;
    }

    public string GetName()
    {
        return name;
    }

    public float GetStrength()
    {
        return strength;
    }
    public float GetDexterity()
    {
        return dexterity;
    }
    public float GetConstitution()
    {
        return constitution;
    }
    public float GetIntelligence()
    {
        return intelligence;
    }
    public float GetWisdom()
    {
        return wisdom;
    }
    public float GetDamage()
    {
        return strength;
    }
    public int GetHealth()
    {
        return (int)currentHealthPoints;
    }
    public int GetMana()
    {
        return (int)currentManaPoints;
    }
    public int GetMaxHealth()
    {
        return (int)maxHealthPoints;
    }
    public int GetMaxMana()
    {
        return (int)maxManaPoints;
    }

    public int GetSpellCount()
    {
        return spells.Count;
    }
    public SPELLS GetSpells (int index)
    {
        return spells[index];
    }
    public float GetSpellCost()
    {
        return spellCost; ;
    }
    public bool CanAffordSpell()
    {
        return spellCost <= currentManaPoints;
    }
    public int GetItemCount()
    {
        return items.Count;
    }

    public Vector3 GetStartingPos()
    {
        return startingPosition;
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
    public bool GetDefending()
    {
        return isDefending;
    }
    public void SetDefending(bool defending)
    {
        isDefending = defending;
    }

    public void SetShieldState(bool shieldAtive)
    {
        shieldObject.SetActive(shieldAtive);
    }

    public void IncreaseStats()
    {
        maxHealthPoints += 10;
        maxManaPoints += 5;
        strength += 1;
        dexterity += 1;
        constitution += 1;
        intelligence += 1;
        wisdom += 1;
    }

    public void UpdateHealth(float value, bool damage)
    {
        if(damage)
        {
            currentHealthPoints -= value;
            if(currentHealthPoints <= 0)
            {
                currentHealthPoints = 0;
                isAlive = false;
            }
        }
        else
        {
            currentHealthPoints += value;
            if (currentHealthPoints > maxHealthPoints)
            {
                currentHealthPoints = maxHealthPoints;
            }
        } 
    }
    public void UpdateMana()
    {
        currentManaPoints -= spellCost;
        if (currentManaPoints <= 0)
        {
            currentManaPoints = 0;
        }
    }

    public GameObject GetObj()
    {
        return referenceObj;
    }
}
