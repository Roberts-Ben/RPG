using UnityEngine;

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

    public string name;
    public ROLES role;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;

    public float maxHealthPoints;
    public float currentHealthPoints;
    public float maxManaPoints;
    public float currentManaPoints;

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


    public void UpdateHealth(int value)
    {
        currentHealthPoints -= value;
    }
}
