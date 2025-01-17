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

    public int LimitDamage;

    public List<SPELLS> spells;
    public List<ITEMS> items;

    public float maxHealthPoints;
    public float currentHealthPoints;
    public float maxManaPoints;
    public float currentManaPoints;

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

    public int GetSpellCount()
    {
        return spells.Count;
    }
    public SPELLS GetSpells (int index)
    {
        return spells[index];
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

    public GameObject GetObj()
    {
        return referenceObj;
    }
}
