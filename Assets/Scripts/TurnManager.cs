using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;

    public List<GameObject> commandButtons;
    public List<GameObject> attackCommandButtons;

    public GameObject targetArrow;

    public bool turnAction = false;
    private int entityTurn;
    private int activeCommand;

    public List<GameObject> enemies;
    public int targetEnemy;

    public enum COMMANDSTATE
    {
        IDLE,
        PROCESS,
        ATTACK,
        MAGIC,
        DEFEND,
        ITEM
    }

    public COMMANDSTATE currentState;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case COMMANDSTATE.IDLE: // ATB Bar filling
                    break;
            case COMMANDSTATE.PROCESS: // Navigating first command menu
                commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + new Vector3(-150, 0, 0);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    for (int i = 0; i < commandButtons.Count; i++)
                    {
                        if (commandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            activeCommand = i;
                            ClearCommandList(activeCommand);
                            return;
                        }
                    }
                }
                break;
            case COMMANDSTATE.ATTACK: // Navigating attack command menu
                for (int i = 0; i < attackCommandButtons.Count; i++)
                {
                    if (attackCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                    {
                        targetEnemy = i;
                        break;
                    }
                }

                targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                targetArrow.SetActive(true);

                commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + new Vector3(-150, 0, 0);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Attack(entityTurn, targetEnemy);
                }
                break;      
        }
    }

    public void TurnReady(int entity)
    {
        turnAction = true;
        currentState = COMMANDSTATE.PROCESS;
        entityTurn = entity;
        commandPanel.SetActive(true);
    }

    public void ClearCommandList(int state)
    {
        switch (state)
        {
            case 0:
                foreach(GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                foreach(GameObject go in attackCommandButtons)
                {
                    go.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(attackCommandButtons[0]);
                }
                currentState = COMMANDSTATE.ATTACK;
                return;
            default:
                return;
        }
    }

    public void Attack(int entity, int target)
    {
        Debug.LogWarning(entity + " is attacking " + target);
        currentState = COMMANDSTATE.IDLE;
        turnAction = false;
        commandPanel.SetActive(false);
    }
}
