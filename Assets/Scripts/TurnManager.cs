using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;
    public GameObject topPanel;

    private Vector3 commandArrowOffset = new(-90, 0, 0);
    private Vector3 attackOffset = new(2, 0, 0);
    private Vector3 targetPosition;

    public List<GameObject> ATBBars;
    public List<GameObject> LimitBars;

    public List<GameObject> commandButtons;
    public List<GameObject> attackCommandButtons;
    public List<GameObject> magicCommandButtons;
    public GameObject limitCommandButton;

    public GameObject playerArrow;
    public GameObject targetArrow;

    private bool victory = false;
    private bool defeat = false;

    public bool turnAction = false;
    private bool hasSpells = false;
    private bool hasItems = false;
    private int entityTurn;
    private int activeCommand;
    private int selectedSpell;
    private int selectedItem;

    public List<GameObject> players;
    public List<GameObject> playerHUDs;
    public List<GameObject> playerHighlights;
    public List<GameObject> enemies;
    public int targetEnemy;
    public int targetAlly;

    public enum COMMANDSTATE
    {
        IDLE,
        PROCESS,
        ATTACK,
        MAGIC,
        MAGICATTACK,
        MAGICHEAL,
        DEFEND,
        ITEM,
        ITEMUSE,
        PROCESSLIMIT,
        LIMIT
    }

    public COMMANDSTATE currentState;
    public COMMANDSTATE prevState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (!victory && !defeat)
        {
            switch (currentState)
            {
                case COMMANDSTATE.IDLE: // ATB Bar filling
                    break;
                case COMMANDSTATE.PROCESS: // Navigating first command menu
                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        for (int i = 0; i < commandButtons.Count; i++)
                        {
                            if (commandButtons[i] == EventSystem.current.currentSelectedGameObject)
                            {
                                if (i == 1 && !hasSpells)
                                {
                                    break;
                                }
                                else if (i == 3 && !hasItems)
                                {
                                    break;
                                }
                                activeCommand = i;
                                ClearCommandList(activeCommand);
                                prevState = COMMANDSTATE.PROCESS;
                                return;
                            }
                        }
                    }
                    break;
                case COMMANDSTATE.ATTACK: // Navigating attack command menu (selecting target)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Attack(entityTurn, targetEnemy, true);
                        prevState = COMMANDSTATE.ATTACK;
                    }
                    break;
                case COMMANDSTATE.MAGIC: // Navigating magic command menu (selecting spell)
                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        for (int i = 0; i < magicCommandButtons.Count; i++)
                        {
                            if (magicCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                            {
                                selectedSpell = i;
                                if (players[entityTurn].GetComponent<BaseClass>().GetSpells(i) == BaseClass.SPELLS.CURE)
                                {
                                    ClearCommandList(2); // Healing
                                }
                                else
                                {
                                    ClearCommandList(0); // Attack
                                }
                                prevState = COMMANDSTATE.MAGIC;
                                return;
                            }
                        }
                    }
                    break;
                case COMMANDSTATE.MAGICATTACK: // Navigating attack command menu after choosing a spell (enemies)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Attack(entityTurn, targetEnemy, true);
                        prevState = COMMANDSTATE.MAGICATTACK;
                    }
                    break;
                case COMMANDSTATE.MAGICHEAL: // Navigating attack command menu after choosing a spell (allies)
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (attackCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            targetAlly = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = players[targetAlly].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Heal(entityTurn, targetAlly);
                        prevState = COMMANDSTATE.MAGICHEAL;
                    }
                    break;
                case COMMANDSTATE.PROCESSLIMIT: // Navigating first menu (limit is the only option)
                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        prevState = COMMANDSTATE.PROCESSLIMIT;
                        ClearCommandList(0);
                        return;
                    }
                    break;
                case COMMANDSTATE.LIMIT: // Navigating attack menu (enemy target)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == EventSystem.current.currentSelectedGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrow.GetComponent<RectTransform>().localPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Limit(entityTurn, targetEnemy);
                        prevState = COMMANDSTATE.LIMIT;
                    }
                    break;
            }
        }
        else if (victory)
        {
            Victory();
        }
        else if (defeat)
        {
            Defeat();
        }
    }

    public void TurnReady(int entity, bool isATBBar, bool isPlayer)
    {
        turnAction = true;
        entityTurn = entity;

        if (isPlayer)
        {
            playerArrow.transform.position = players[entity].transform.position + Vector3.up * 2;
            playerArrow.SetActive(true);
            playerHighlights[entity].SetActive(true);

            if (isATBBar)
            {
                commandPanel.SetActive(true);
                ClearCommandList(99); // Idle commands
            }
            else
            {
                commandPanel.SetActive(true);
                ClearCommandList(3); // Limit
            }
        }
        else if (!isPlayer)
        {
            Attack(entityTurn, 0, false);
        }
    }

    public void ClearCommandList(int state)
    {
        switch (state)
        {
            case 0: // Setting enemy targets after attack declaration
                limitCommandButton.SetActive(false);
                bool foundfirstAliveEnemy = false;
                int firstAliveEnemy = 0;
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in magicCommandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].GetComponent<BaseClass>().isAlive)
                    {
                        if (!foundfirstAliveEnemy)
                        {
                            firstAliveEnemy = i;
                            foundfirstAliveEnemy = true;
                        }

                        attackCommandButtons[i].SetActive(true);
                        attackCommandButtons[i].GetComponentInChildren<TMP_Text>().text = enemies[i].GetComponent<BaseClass>().GetName();
                    }
                }
                EventSystem.current.SetSelectedGameObject(attackCommandButtons[firstAliveEnemy]);
                if (prevState == COMMANDSTATE.PROCESSLIMIT)
                {
                    currentState = COMMANDSTATE.LIMIT;
                }
                else
                {
                    currentState = COMMANDSTATE.ATTACK;
                }
                return;
            case 1: // Selecting spell to use
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < magicCommandButtons.Count; i++)
                {
                    if (players[entityTurn].GetComponent<BaseClass>().GetSpellCount() > i)
                    {
                        magicCommandButtons[i].SetActive(true);
                        magicCommandButtons[i].GetComponentInChildren<TMP_Text>().text = players[entityTurn].GetComponent<BaseClass>().GetSpells(i).ToString();
                    }
                }

                EventSystem.current.SetSelectedGameObject(magicCommandButtons[0]);
                currentState = COMMANDSTATE.MAGIC;
                return;
            case 2: // Selecting ally to heal
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in magicCommandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < players.Count; i++)
                {
                    attackCommandButtons[i].SetActive(true);
                    attackCommandButtons[i].GetComponentInChildren<TMP_Text>().text = players[i].GetComponent<BaseClass>().GetName();
                }
                EventSystem.current.SetSelectedGameObject(attackCommandButtons[0]);
                currentState = COMMANDSTATE.MAGICHEAL;
                return;
            case 3: // Clear all commands and display limit command
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in magicCommandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in attackCommandButtons)
                {
                    go.SetActive(false);
                }
                limitCommandButton.SetActive(true);

                EventSystem.current.SetSelectedGameObject(limitCommandButton);
                currentState = COMMANDSTATE.PROCESSLIMIT;
                return;
            case 99: // Default Clearall, back to the initial action select commands
                foreach (GameObject go in attackCommandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in magicCommandButtons)
                {
                    go.SetActive(false);
                }
                for (int i = 0; i < commandButtons.Count; i++)
                {
                    commandButtons[i].SetActive(true);

                    if (i == 1) // Spells
                    {
                        if (players[entityTurn].GetComponent<BaseClass>().GetSpellCount() > 0)
                        {
                            hasSpells = true;
                            commandButtons[i].GetComponentInChildren<TMP_Text>().color = Color.white;
                        }
                        else
                        {
                            hasSpells = false;
                            commandButtons[i].GetComponentInChildren<TMP_Text>().color = Color.gray;
                        }
                    }
                    if (i == 3) // Items
                    {
                        if (players[entityTurn].GetComponent<BaseClass>().GetItemCount() > 0)
                        {
                            hasItems = true;
                            commandButtons[i].GetComponentInChildren<TMP_Text>().color = Color.white;
                        }
                        else
                        {
                            hasItems = false;
                            commandButtons[i].GetComponentInChildren<TMP_Text>().color = Color.gray;
                        }
                    }
                }
                EventSystem.current.SetSelectedGameObject(commandButtons[0]);
                currentState = COMMANDSTATE.PROCESS;
                return;
            default:
                return;
        }
    }

    public void Attack(int entity, int target, bool playerAttack)
    {
        if (!playerAttack)
        {
            entity -= 4;
            bool meleeAttack = Random.Range(0, 1) == 0;
            target = Random.Range(0, players.Count);

            if (meleeAttack)
            {
                targetPosition = players[target].GetComponent<BaseClass>().GetStartingPos() + attackOffset;
                enemies[entity].GetComponent<Animator>().SetTrigger("MeleeAttack");
                StartCoroutine(enemies[entity].GetComponent<AnimationManager>().AttackAnimLerp(entity, targetPosition, 0.5f));
                CombatManager.instance.MeleeAttack(enemies[entity].GetComponent<BaseClass>(), players[target].GetComponent<BaseClass>());
            }
            else
            {
                // NEED TO HANDLE ENEMY MAGIC ATTACKS
                enemies[entity].GetComponent<Animator>().SetTrigger("MagicAttack");
                CombatManager.instance.MeleeAttack(enemies[entity].GetComponent<BaseClass>(), players[target].GetComponent<BaseClass>());
            }

            players[target].GetComponent<Animator>().SetTrigger("Damaged");

            currentState = COMMANDSTATE.IDLE;
            turnAction = false;
            ATBBars[entity + 4].GetComponent<ATBBar>().ResetBar();

            playerHUDs[target].GetComponent<CharacterHUD>().UpdateStats();
        }
        else
        {
            if (currentState == COMMANDSTATE.ATTACK && prevState == COMMANDSTATE.PROCESS)
            {
                targetPosition = enemies[target].GetComponent<BaseClass>().GetStartingPos() - attackOffset;
                players[entity].GetComponent<Animator>().SetTrigger("MeleeAttack");
                StartCoroutine(players[entity].GetComponent<AnimationManager>().AttackAnimLerp(entity, targetPosition, 0.5f));
                CombatManager.instance.MeleeAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>());
            }
            else if (currentState == COMMANDSTATE.ATTACK && prevState == COMMANDSTATE.MAGIC)
            {
                players[entity].GetComponent<Animator>().SetTrigger("SpellAttack");
                CombatManager.instance.MagicAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>(), selectedSpell);
            }

            enemies[target].GetComponent<Animator>().SetTrigger("Damaged");
            EndAction(entity, target, false);
        }
        CheckAlive(target, !playerAttack);
    }

    public void Heal(int entity, int target)
    {
        players[entity].GetComponent<Animator>().SetTrigger("SpellAttack");
        CombatManager.instance.MagicHealing(players[entity].GetComponent<BaseClass>(), players[target].GetComponent<BaseClass>());
        EndAction(entity, target, false);
    }

    public void Limit(int entity, int target)
    {
        targetPosition = enemies[target].GetComponent<BaseClass>().GetStartingPos() - attackOffset;
        players[entity].GetComponent<Animator>().SetTrigger("Limit");
        enemies[target].GetComponent<Animator>().SetTrigger("Damaged");

        StartCoroutine(players[entity].GetComponent<AnimationManager>().AttackAnimLerp(entity, targetPosition, 0.5f));
        CombatManager.instance.LimitAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>());
        EndAction(entity, target, true);
        CheckAlive(target, true);
    }

    public void EndAction(int entity, int target, bool isLimit)
    {
        currentState = COMMANDSTATE.IDLE;
        turnAction = false;
        commandPanel.SetActive(false);
        playerArrow.SetActive(false);
        playerHighlights[entity].SetActive(false);
        targetArrow.SetActive(false);
        if (isLimit)
        {
            LimitBars[entity].GetComponent<ATBBar>().ResetBar();
        }
        else
        {
            ATBBars[entity].GetComponent<ATBBar>().ResetBar();
        }

        playerHUDs[target].GetComponent<CharacterHUD>().UpdateStats();
    }
    public void CheckAlive(int entity, bool isEnemy)
    {
        if (isEnemy)
        {
            if (!enemies[entity].GetComponent<BaseClass>().isAlive)
            {
                enemies[entity].GetComponent<Animator>().SetTrigger("Death");
            }
            if (IsTeamWiped(isEnemy))
            {
                victory = true;
            }
        }
        else
        {
            if (!players[entity].GetComponent<BaseClass>().isAlive)
            {
                players[entity].GetComponent<Animator>().SetTrigger("Death");
            }
            if (IsTeamWiped(isEnemy))
            {
                defeat = true;
            }
        }
    }

    public bool IsTeamWiped(bool isEnemy)
    {
        if (isEnemy)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetComponent<BaseClass>().isAlive)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetComponent<BaseClass>().isAlive)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public bool GetBattleOver()
    {
        if (victory || defeat)
        {
            return true;
        }
        return false;
    }

    void Victory()
    {
        topPanel.SetActive(true);
        topPanel.GetComponent<TMP_Text>().text = "VICTORY";
    }
    void Defeat()
    {
        topPanel.SetActive(true);
        topPanel.GetComponent<TMP_Text>().text = "DEFEAT";
    }
    public void ResetAfterBattle()
    {
        for (int i = 0; i < enemies.Count + players.Count; i++)
        {
            ATBBars[i].GetComponent<ATBBar>().ResetBar();
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            BaseClass classref = enemies[i].GetComponent<BaseClass>();
            
            classref.currentHealthPoints = classref.maxHealthPoints;
            classref.currentManaPoints = classref.maxManaPoints;
            classref.isAlive = true;
            enemies[i].GetComponent<Animator>().ResetTrigger("Death");
            enemies[i].GetComponent<Animator>().SetTrigger("Respawn");
        }
        victory = false;
        defeat = false;
        topPanel.SetActive(false);
    }
}