using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

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
        Debug.LogWarning(entity.strength - target.constitution + " damage inflicted to " + target.GetName());
        Debug.LogWarning(target.GetName() + " has " + target.GetHealth() + "HP remaining");
    }
}
