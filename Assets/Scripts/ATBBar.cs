using UnityEngine;
using UnityEngine.UI;

public class ATBBar : MonoBehaviour
{
    public Image fillBar;

    public float fillDuration;
    private float fillAmount;

    void Update()
    {
        if(fillAmount >= 1)
        {
            //Stop time and process turn
        }

        fillAmount += 1.0f / fillDuration * Time.deltaTime;
        fillBar.fillAmount = fillAmount;
    }
}
