using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;

    public bool turnAction = false;
    public int entityTurn;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if(turnAction)
        {
            
        }
    }

    public void TurnReady(int entity)
    {
        turnAction = true;
        entityTurn = entity;
        commandPanel.SetActive(true);
    }
}
