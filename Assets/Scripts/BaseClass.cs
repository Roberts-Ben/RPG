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

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;

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

    public int GetStrength()
    {
        return strength;
    }
    public int GetDexterity()
    {
        return dexterity;
    }
    public int GetConstitution()
    {
        return constitution;
    }
    public int GetIntelligence()
    {
        return intelligence;
    }
    public int GetWisdom()
    {
        return wisdom;
    }
    public int GetDamage()
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

    public void UpdateHealth(int value, bool damage)
    {
        if(damage)
        {
            currentHealthPoints -= value;
            if(currentHealthPoints <= 0)
            {
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
