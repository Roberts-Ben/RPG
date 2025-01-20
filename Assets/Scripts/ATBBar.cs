using UnityEngine;
using UnityEngine.UI;

public class ATBBar : MonoBehaviour
{
    public GameObject referenceObj;
    public int entityID;
    public bool isPlayer;
    public bool isATBBar;
    public Image fillBar;

    public float fillDuration;
    public float fillAmount;
    public float startingFill;

    void Awake()
    {
        referenceObj = this.gameObject;
        fillAmount = startingFill;
    }

    void Update()
    {
        if(!TurnManager.instance.GetBattleOver())
        {
            if (TurnManager.instance.turnAction)
            {
                return;
            }

            if (fillAmount >= fillDuration)
            {
                if (!isPlayer)
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

                AudioManager.instance.PlayAudio("Menu Navigation");
            }

            fillAmount += 1.0f  * Time.deltaTime;
            fillBar.fillAmount = fillAmount / fillDuration;
        }
    }

    public void ResetBar()
    {
        fillAmount = 0;
    }
    public void ResetBarNewRound()
    {
        fillAmount = startingFill;
    }
}
