using UnityEngine;
using UnityEngine.UI;

public class ATBBar : MonoBehaviour
{
    public GameObject thisObj;
    public int entityID;
    public bool isPlayer;
    public bool isATBBar;
    public Image fillBar;

    public float fillDuration;
    private float fillAmount;

    void Awake()
    {
        thisObj = this.gameObject;
    }

    void Update()
    {
        if(TurnManager.instance.turnAction)
        {
            return;
        }

        if(fillAmount >= 1)
        {
            if(!isPlayer)
            {
                if (TurnManager.instance.enemies[entityID - 4].GetComponent<BaseClass>().isAlive)
                {
                    TurnManager.instance.TurnReady(entityID, isATBBar, isPlayer);
                }
            }
            else if (TurnManager.instance.players[entityID].GetComponent<BaseClass>().isAlive)
            {
                TurnManager.instance.TurnReady(entityID, isATBBar, isPlayer);
            }
        }

        fillAmount += 1.0f / fillDuration * Time.deltaTime;
        fillBar.fillAmount = fillAmount;
    }

    public void ResetBar()
    {
        fillAmount = 0;
    }
}
