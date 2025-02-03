using UnityEngine;
using UnityEngine.UI;

public class ATBBar : MonoBehaviour
{
    public GameObject referenceObj;
    private BaseClass baseClassRef;
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

    private void Start()
    {
        if (isPlayer)
        {
            baseClassRef = TurnManager.instance.players[entityID].GetComponent<BaseClass>();
        }
        else
        {
            baseClassRef = TurnManager.instance.enemies[entityID - 4].GetComponent<BaseClass>();
        }
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
                if (baseClassRef.isAlive)
                {
                    TurnManager.instance.TurnReady(entityID, isATBBar, isPlayer);
                    if (isPlayer)
                    {
                        AudioManager.instance.PlayAudio("Menu Navigation", false);
                    }
                }
            }
            if(baseClassRef.isAlive)
            {
                fillAmount += 1.0f * Time.deltaTime;
                fillBar.fillAmount = fillAmount / fillDuration;
            } 
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
    public void DisableBar()
    {
        fillBar.color = Color.grey;
    }
}
