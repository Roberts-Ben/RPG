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
            TurnManager.instance.TurnReady(entityID, isATBBar, isPlayer);
            return;
        }

        fillAmount += 1.0f / fillDuration * Time.deltaTime;
        fillBar.fillAmount = fillAmount;
    }

    public void ResetBar()
    {
        fillAmount = 0;
    }
}
