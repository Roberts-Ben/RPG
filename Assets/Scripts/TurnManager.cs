using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public GameObject commandPanel;
    public GameObject commandArrow;
    private RectTransform commandArrowRectTransform;
    public GameObject topPanel;
    public GameObject nextBattleButton;

    private Vector3 commandArrowOffset = new(-185, 0, 0);
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
    public GameObject shieldObject;

    private GameObject selectedEventGameObject;
    private RectTransform selectedEventrectTransform;

    private bool victory = false;
    private bool defeat = false;

    public bool turnAction = false;
    private bool hasSpells = false;
    private bool hasItems = false;
    private int entityTurn;
    private int activeCommand;
    private int selectedSpell;
    private int selectedItem;
    private bool canAffordSpell;

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
        LIMIT,
        BATTLEOVER
    }

    public COMMANDSTATE currentState;
    public COMMANDSTATE prevState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        commandArrowRectTransform = commandArrow.GetComponent<RectTransform>();
        selectedEventGameObject = EventSystem.current.firstSelectedGameObject;
        selectedEventrectTransform = selectedEventGameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != selectedEventGameObject)
        {
            AudioManager.instance.PlayAudio("Menu Navigation", false);
            selectedEventGameObject = EventSystem.current.currentSelectedGameObject;
        }

        if (selectedEventGameObject != null)
        {
            selectedEventrectTransform = selectedEventGameObject.GetComponent<RectTransform>();
        }

        if (!victory && !defeat)
        {
            switch (currentState)
            {
                case COMMANDSTATE.IDLE: // ATB Bar filling
                    break;
                case COMMANDSTATE.PROCESS: // Navigating first command menu
                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        for (int i = 0; i < commandButtons.Count; i++)
                        {
                            if (commandButtons[i] == selectedEventGameObject)
                            {
                                if (i == 1 && !hasSpells || i == 1 && !canAffordSpell)
                                {
                                    AudioManager.instance.PlayAudio("Menu Invalid", false);
                                    break;
                                }
                                else if (i == 3 && !hasItems)
                                {
                                    AudioManager.instance.PlayAudio("Menu Invalid", false);
                                    break;
                                }
                                AudioManager.instance.PlayAudio("Menu Select", false);
                                activeCommand = i;
                                prevState = COMMANDSTATE.PROCESS;
                                ClearCommandList(activeCommand);
                                return;
                            }
                        }
                    }
                    break;
                case COMMANDSTATE.ATTACK: // Navigating attack command menu (selecting target)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == selectedEventGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.instance.PlayAudio("Menu Select", false);
                        Attack(entityTurn, targetEnemy, true);
                        prevState = COMMANDSTATE.ATTACK;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        AudioManager.instance.PlayAudio("Menu Back", false);
                        currentState = prevState;
                        ClearCommandList(99);
                        return;
                    }
                    break;
                case COMMANDSTATE.MAGIC: // Navigating magic command menu (selecting spell)
                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        for (int i = 0; i < magicCommandButtons.Count; i++)
                        {
                            if (magicCommandButtons[i] == selectedEventGameObject)
                            {
                                AudioManager.instance.PlayAudio("Menu Select", false);
                                selectedSpell = i;
                                prevState = COMMANDSTATE.MAGIC;
                                if (players[entityTurn].GetComponent<BaseClass>().GetSpells(i) == BaseClass.SPELLS.CURE)
                                {
                                    ClearCommandList(5); // Healing
                                }
                                else
                                {
                                    ClearCommandList(0); // Attack
                                }
                                return;
                            }
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        AudioManager.instance.PlayAudio("Menu Back", false);
                        currentState = prevState;
                        ClearCommandList(99);
                        return;
                    }
                    break;
                case COMMANDSTATE.MAGICATTACK: // Navigating attack command menu after choosing a spell (enemies)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == selectedEventGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.instance.PlayAudio("Menu Select", false);
                        Attack(entityTurn, targetEnemy, true);
                        prevState = COMMANDSTATE.MAGICATTACK;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        AudioManager.instance.PlayAudio("Menu Back", false);
                        currentState = prevState;
                        ClearCommandList(1);
                        return;
                    }
                    break;
                case COMMANDSTATE.MAGICHEAL: // Navigating attack command menu after choosing a spell (allies)
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (attackCommandButtons[i] == selectedEventGameObject)
                        {
                            targetAlly = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = players[targetAlly].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.instance.PlayAudio("Menu Select", false);
                        Heal(entityTurn, targetAlly);
                        prevState = COMMANDSTATE.MAGICHEAL;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        AudioManager.instance.PlayAudio("Menu Back", false);
                        currentState = prevState;
                        ClearCommandList(1);
                        return;
                    }
                    break;
                case COMMANDSTATE.PROCESSLIMIT: // Navigating first menu (limit is the only option)
                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.instance.PlayAudio("Menu Select", false);
                        prevState = COMMANDSTATE.PROCESSLIMIT;
                        ClearCommandList(0);
                        return;
                    }
                    break;
                case COMMANDSTATE.LIMIT: // Navigating attack menu (enemy target)
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (attackCommandButtons[i] == selectedEventGameObject)
                        {
                            targetEnemy = i;
                            break;
                        }
                    }

                    targetArrow.transform.position = enemies[targetEnemy].transform.position + Vector3.up * 2;
                    targetArrow.SetActive(true);

                    commandArrowRectTransform.localPosition = selectedEventrectTransform.localPosition + commandArrowOffset;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.instance.PlayAudio("Menu Select", false);
                        Limit(entityTurn, targetEnemy);
                        prevState = COMMANDSTATE.LIMIT;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        AudioManager.instance.PlayAudio("Menu Back", false);
                        currentState = prevState;
                        ClearCommandList(99);
                        return;
                    }
                    break;
                case COMMANDSTATE.DEFEND:
                    SetDefend(entityTurn, true);
                    EndAction(entityTurn, entityTurn, false);
                    break;
            }
        }
        else if (currentState != COMMANDSTATE.BATTLEOVER)
        {
            if (victory)
            {
                Victory();
            }
            else if (defeat)
            {
                Defeat();
            }
            currentState = COMMANDSTATE.BATTLEOVER;
        }
    }

    public void TurnReady(int entity, bool isATBBar, bool isPlayer)
    {
        turnAction = true;
        entityTurn = entity;

        if (isPlayer)
        {
            SetDefend(entity, false);
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
                ClearCommandList(98); // Limit
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
                    if (enemies[i].GetComponent<BaseClass>().GetAlive())   
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
                else if(prevState == COMMANDSTATE.MAGIC)
                {
                    currentState = COMMANDSTATE.MAGICATTACK;
                }
                else
                {
                    currentState = COMMANDSTATE.ATTACK;
                }
                return;
            case 1: // Selecting spell to use
                targetArrow.SetActive(false);
                foreach (GameObject go in commandButtons)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in attackCommandButtons)
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
            case 2:
                currentState = COMMANDSTATE.DEFEND;
                return;
            case 5: // Selecting ally to heal
                bool foundfirstAliveAlly= false;
                int firstAliveAlly = 0;
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
                    if (players[i].GetComponent<BaseClass>().GetAlive())
                    {
                        if (!foundfirstAliveAlly)
                        {
                            firstAliveAlly = i;
                            foundfirstAliveAlly = true;
                        }

                        attackCommandButtons[i].SetActive(true);
                        attackCommandButtons[i].GetComponentInChildren<TMP_Text>().text = players[i].GetComponent<BaseClass>().GetName();
                    }
                }
                EventSystem.current.SetSelectedGameObject(attackCommandButtons[0]);
                currentState = COMMANDSTATE.MAGICHEAL;
                return;
            case 98: // Clear all commands and display limit command
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
                targetArrow.SetActive(false);

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
                            if(players[entityTurn].GetComponent<BaseClass>().CanAffordSpell())
                            {
                                canAffordSpell = true;
                                commandButtons[i].GetComponentInChildren<TMP_Text>().color = Color.white;
                            }
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
            entity -= 4; //Offset as players & enemies are in this list

            BaseClass baseClassRef = enemies[entity].GetComponent<BaseClass>();
            
            bool meleeAttack = Random.Range(0, baseClassRef.GetSpellCount()) == 0;

            bool hasTarget = false;

            do
            {
                target = Random.Range(0, players.Count);
                if(players[target].GetComponent<BaseClass>().GetAlive())
                {
                    hasTarget = true;
                }
            } while (!hasTarget);
            

            BaseClass targetBaseClassRef = players[target].GetComponent<BaseClass>();

            if (meleeAttack)
            {
                targetPosition = targetBaseClassRef.GetStartingPos() + attackOffset;
                enemies[entity].GetComponent<Animator>().SetTrigger("MeleeAttack");
                StartCoroutine(enemies[entity].GetComponent<AnimationManager>().AttackAnimLerp(entity, targetPosition, 0.5f));
                CombatManager.instance.MeleeAttack(baseClassRef, targetBaseClassRef);
            }
            else
            {
                selectedSpell = Random.Range(0, baseClassRef.GetSpellCount());
                enemies[entity].GetComponent<Animator>().SetTrigger("SpellAttack");
                CombatManager.instance.MagicAttack(baseClassRef, targetBaseClassRef, selectedSpell);
            }

            players[target].GetComponent<Animator>().SetTrigger("Damaged");

            currentState = COMMANDSTATE.IDLE;
            turnAction = false;
            ATBBars[entity + 4].GetComponent<ATBBar>().ResetBar();

            playerHUDs[target].GetComponent<CharacterHUD>().UpdateStats();

            CheckAlive(target, false);
        }
        else
        {
            BaseClass baseClassRef = players[entity].GetComponent<BaseClass>();
            BaseClass targetBaseClassRef = enemies[target].GetComponent<BaseClass>();

            if (currentState == COMMANDSTATE.ATTACK)
            {
                targetPosition = targetBaseClassRef.GetStartingPos() - attackOffset;
                players[entity].GetComponent<Animator>().SetTrigger("MeleeAttack");
                StartCoroutine(players[entity].GetComponent<AnimationManager>().AttackAnimLerp(entity, targetPosition, 0.5f));
                CombatManager.instance.MeleeAttack(baseClassRef, targetBaseClassRef);
            }
            else if (currentState == COMMANDSTATE.MAGICATTACK)
            {
                players[entity].GetComponent<Animator>().SetTrigger("SpellAttack");
                CombatManager.instance.MagicAttack(baseClassRef, targetBaseClassRef, selectedSpell);
            }

            enemies[target].GetComponent<Animator>().SetTrigger("Damaged");
            EndAction(entity, target, false);
        }
        CheckAlive(target, true);
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

        CombatManager.instance.LimitAttack(players[entity].GetComponent<BaseClass>(), enemies[target].GetComponent<BaseClass>());
        EndAction(entity, target, true);
        CheckAlive(target, true);
    }

    public void SetDefend(int entity, bool isDefending)
    {
        BaseClass baseClassRef = players[entity].GetComponent<BaseClass>();
        baseClassRef.SetDefending(isDefending);
        baseClassRef.SetShieldState(isDefending);
        players[entity].GetComponent<Animator>().SetBool("Defending", isDefending);

        if(isDefending)
        {
            AudioManager.instance.PlayAudio("Defend", false);
        }
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

        playerHUDs[entity].GetComponent<CharacterHUD>().UpdateStats();
        playerHUDs[target].GetComponent<CharacterHUD>().UpdateStats();
    }
    public void CheckAlive(int entity, bool isEnemy)
    {
        if (isEnemy)
        {
            if (!enemies[entity].GetComponent<BaseClass>().GetAlive())
            {
                StartCoroutine(DeathAudio());
                enemies[entity].GetComponent<Animator>().SetTrigger("Death");
            }
            if (IsTeamWiped(isEnemy))
            {
                victory = true;
            }
        }
        else
        {
            if (!players[entity].GetComponent<BaseClass>().GetAlive())
            {
                StartCoroutine(DeathAudio());
                players[entity].GetComponent<Animator>().SetTrigger("Death");
                ATBBars[entity].GetComponent<ATBBar>().DisableBar();
                LimitBars[entity].GetComponent<ATBBar>().DisableBar();
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
                if (enemies[i].GetComponent<BaseClass>().GetAlive())
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
                if (players[i].GetComponent<BaseClass>().GetAlive())
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
        nextBattleButton.SetActive(true);
        topPanel.GetComponentInChildren<TMP_Text>().text = "VICTORY";
        AudioManager.instance.StopAudio("BGM");
        AudioManager.instance.PlayAudio("Victory", true);
    }
    void Defeat()
    {
        topPanel.SetActive(true);
        nextBattleButton.SetActive(false);
        topPanel.GetComponentInChildren<TMP_Text>().text = "DEFEAT";
        AudioManager.instance.StopAudio("BGM");
        AudioManager.instance.PlayAudio("Defeat", false);
    }
    public void ResetAfterBattle()
    {
        for (int i = 0; i < enemies.Count + players.Count; i++)
        {
            ATBBars[i].GetComponent<ATBBar>().ResetBarNewRound();
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            BaseClass classref = enemies[i].GetComponent<BaseClass>();

            classref.IncreaseStats();

            classref.currentHealthPoints = classref.maxHealthPoints;
            classref.currentManaPoints = classref.maxManaPoints;
            classref.SetAlive(true);
            enemies[i].GetComponent<Animator>().ResetTrigger("Death");
            enemies[i].GetComponent<Animator>().SetTrigger("Respawn");

            
        }
        AudioManager.instance.StopAudio("Victory");
        AudioManager.instance.StopAudio("Defeat");
        AudioManager.instance.PlayAudio("BGM", true);

        victory = false;
        defeat = false;
        topPanel.SetActive(false);

        currentState = COMMANDSTATE.IDLE;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator DeathAudio()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlayAudio("Death", false);
    }
}