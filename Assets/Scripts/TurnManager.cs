using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;
    public List<Image> commandButtons;

    public GameObject targetArrow;

    public bool turnAction = false;
    public int entityTurn;
    private int activeCommand;

    public List<GameObject> enemies;
    public int targetEnemy;

    public enum COMMANDMENUSTATE
    {
        Attack,
        Magic,
        Defend,
        Item
    }
    public enum COMMANDSTATE
    {
        Idle,
        Attack,
        Magic
    }

    public enum MAGICSTATE
    {
        Fire,
        Blizzard,
        Thunder
    }

    public COMMANDMENUSTATE cmdMenuState;
    public COMMANDSTATE cmdState;
    public MAGICSTATE magicState;

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
            if(cmdState == COMMANDSTATE.Idle)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    activeCommand -= 1;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    activeCommand += 1;
                }

                if(activeCommand > 3)
                {
                    activeCommand = 0;
                }
                if(activeCommand < 0)
                {
                    activeCommand = 3;
                }

                cmdMenuState = (COMMANDMENUSTATE)activeCommand;
                commandArrow.GetComponent<RectTransform>().localPosition = commandButtons[activeCommand].rectTransform.localPosition + new Vector3(0, -80, 0);

                switch (cmdMenuState)
                {
                    case COMMANDMENUSTATE.Attack:
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            cmdState = COMMANDSTATE.Attack;
                            targetArrow.transform.position = enemies[0].transform.position + Vector3.up;
                            targetArrow.SetActive(true);
                        }
                        break;
                    case COMMANDMENUSTATE.Magic:
                        break;
                    case COMMANDMENUSTATE.Defend:
                        break;
                    case COMMANDMENUSTATE.Item:
                        break;
                }
            }
            
            if(cmdState == COMMANDSTATE.Attack)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    targetEnemy += 1;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    targetEnemy -= 1;
                }

                if (targetEnemy > enemies.Count)
                {
                    targetEnemy = 0;
                }
                if (targetEnemy < 0)
                {
                    targetEnemy = enemies.Count;
                }

                targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 3;
            }
        }
    }

    public void TurnReady(int entity)
    {
        turnAction = true;
        entityTurn = entity;
        commandPanel.SetActive(true);
    }
}
