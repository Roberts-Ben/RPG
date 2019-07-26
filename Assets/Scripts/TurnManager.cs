using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;

    public List<GameObject> ATBBars;
    public List<GameObject> LimitBars;

    public List<GameObject> commandButtons;
    public List<GameObject> attackCommandButtons;
    public List<GameObject> magicCommandButtons;

    public GameObject playerArrow;
    public GameObject targetArrow;

    public bool turnAction = false;
    private int entityTurn;
    private int activeCommand;
    private int selectedSpell;
    private int selectedItem;

    public List<GameObject> players;
    public List<GameObject> playerHighlights;
    public List<GameObject> enemies;
    public int targetEnemy;

    public enum COMMANDSTATE
    {
        IDLE,
        PROCESS,
        ATTACK,
        MAGIC,
        MAGICATTACK,
        DEFEND,
        ITEM,
        ITEMUSE
    }

    public COMMANDSTATE currentState;
    public COMMANDSTATE prevState;

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
                            prevState = COMMANDSTATE.PROCESS;
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
                    Attack(entityTurn, targetEnemy, false);
                    prevState = COMMANDSTATE.ATTACK;
                }
                break;
            case COMMANDSTATE.MAGIC:
                commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + new Vector3(-150, 0, 0);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    for (int i = 0; i < magicCommandButtons.Count; i++)
                    {
                        if (magicCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            selectedSpell = i;

                            if (selectedSpell == 3) // TEMP CURE TEST
                            {
                                // HANDLE ALLY TARGETTING
                            }
                            else
                            {
                                ClearCommandList(0);
                            }
                            
                            prevState = COMMANDSTATE.MAGIC;
                            return;
                        }
                    }
                }
                break;
            case COMMANDSTATE.MAGICATTACK: // Navigating attack command menu after choosign a spell
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
                    Attack(entityTurn, targetEnemy, false);
                    prevState = COMMANDSTATE.MAGICATTACK;
                }
                break;
        }
    }

    public void TurnReady(int entity, bool isATBBar)
    {
        turnAction = true;

        playerArrow.transform.position = players[entity].transform.position + Vector3.up * 2;
        playerArrow.SetActive(true);
        playerHighlights[entity].SetActive(true);

        if (isATBBar)
        {
            entityTurn = entity;
            commandPanel.SetActive(true);
            ClearCommandList(99);
        }
        else
        {
            // NEED TO HANDLE LIMIT ATTACK
        }
    }

    public void ClearCommandList(int state)
    {
        switch (state)
        {
            case 0: // Attacking
                foreach(GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < attackCommandButtons.Count; i++)
                {
                    attackCommandButtons[i].SetActive(true);
                    attackCommandButtons[i].GetComponentInChildren<TMP_Text>().text = enemies[i].GetComponent<BaseClass>().GetName();
                }
                EventSystem.current.SetSelectedGameObject(attackCommandButtons[0]);
                currentState = COMMANDSTATE.ATTACK;
                return;
            case 1: // Choosing Spell
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < magicCommandButtons.Count; i++)
                {
                    if(players[i].GetComponent<BaseClass>().GetSpellCount() >= i)
                    {
                        magicCommandButtons[i].SetActive(true);
                        magicCommandButtons[i].GetComponentInChildren<TMP_Text>().text = players[i].GetComponent<BaseClass>().GetSpells(i);
                    }
                }

                EventSystem.current.SetSelectedGameObject(magicCommandButtons[0]);
                currentState = COMMANDSTATE.MAGIC;
                return;
            case 99: // Default Clearall
                foreach (GameObject go in attackCommandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in magicCommandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(true);
                }
                EventSystem.current.SetSelectedGameObject(commandButtons[0]);
                currentState = COMMANDSTATE.PROCESS;
                return;
            default:
                return;
        }
    }

    public void Attack(int entity, int target, bool enemyAttack)
    {
        Debug.LogWarning(entity + " is attacking " + target);

        if(enemyAttack)
        {
            // NEED TO HANDELE ENEMY TURNS
        }
        else
        {
            Debug.LogError(currentState + " : " + prevState);
            if (currentState == COMMANDSTATE.ATTACK && prevState == COMMANDSTATE.PROCESS)
            {
                CombatManager.instance.MeleeAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>());
            }
            else if(currentState == COMMANDSTATE.ATTACK && prevState == COMMANDSTATE.MAGIC)
            {
                CombatManager.instance.MagicAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>(), selectedSpell);
            }

            currentState = COMMANDSTATE.IDLE;
            turnAction = false;
            commandPanel.SetActive(false);
            playerArrow.SetActive(false);
            playerHighlights[entity].SetActive(false);
            targetArrow.SetActive(false);
            ATBBars[entity].GetComponent<ATBBar>().ResetBar();
        }
    }
}
