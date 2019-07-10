using UnityEngine;
using UnityEngine.UI;

public class ATBBar : MonoBehaviour
{
    public int entityID;
    public Image fillBar;

    public float fillDuration;
    private float fillAmount;

    void Update()
    {
        if(TurnManager.instance.turnAction)
        {
            return;
        }

        if(fillAmount >= 1)
        {
            TurnManager.instance.TurnReady(entityID);
            return;
        }

        fillAmount += 1.0f / fillDuration * Time.deltaTime;
        fillBar.fillAmount = fillAmount;
    }
}
