using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Editables")]
    public Text enemyType;
    public Text enemyWeaponUsed;
    public Text playerWeaponUsed;
    public Text gameDifficulty;
    public GameObject MainGameUI;
    public GameObject SettingsUI;
    public float playerRangeAtt = 5f;
    public float playerMeleeAtt = 1.5f;

    [Header("Required Information(Uneditable)")]
    public LineRenderer circleRenderer;
    [SerializeField] private float walkDistance;
    [SerializeField] private float healStartDistance;
    [SerializeField] private float healAmount;
    [SerializeField] private float playerHealthAmount;
    [SerializeField] private float enemyHealthAmount;
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider enemyHealth;
    [SerializeField] private float damage = 1;
    [SerializeField] private Text turnIndicator;
    [SerializeField] private Text enemyStatus;
    private float playerAttackDistance = 2.5f;
    float distance;
    bool canDoAction;
    public Button attBtn;

    string heavy = "Heavy";
    string light = "Light";
    string melee = "Melee";
    string range = "Range";
    string easy = "Easy";
    string hard = "Hard";
    string playerTurn = "Player's Turn";
    string enemyTurn = "Enemy's Turn";
    string idle = "Idle";
    string patrol = "Patroling";
    string chase = "Chasing";
    string attack = "Attacking";
    string run = "Running";
    public static bool damaged;
    RaycastHit hit;

    bool detectEsc = false;

    //to be used by BT
    public static string enemyTyping;
    public static string enemyWeapon;
    public static string playerWeapon;
    public static string difficulty;
    public static float healDistance = 6f;
    public static float healingAmt = 0.5f;
    public static float walkableDistance = 6f;
    public static float enemyHealthAmt = 3f;
    public static float playerHealthAmt = 3f;
    public static string enemyCurrentState = "Idle";
    public static GameObject player;
    public static GameObject enemy;

    public static bool isPlayerTurn = true;       //first turn is player


    void Start()
    {
        walkableDistance = walkDistance;
        healDistance = healStartDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyHealthAmt = enemyHealthAmount;
        playerHealthAmt = playerHealthAmount;
        attBtn.interactable = false;
    }

    void Update()
    {
        OpenSettings();
        ChangeGameSettings();

        if (!detectEsc)
        {
            if (isPlayerTurn)
            {
                if (Vector3.Distance(player.transform.position, enemy.transform.position) <= playerAttackDistance)//allow player attack option if close
                {
                    ShowAttackRange(100, playerAttackDistance);
                    attBtn.interactable = true;
                }
                turnIndicator.text = playerTurn;
                ShowWalkableRadius(100, walkableDistance);

                if (Input.GetMouseButtonDown(0) && !canDoAction)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        distance = Vector3.Distance(player.transform.position, hit.point);
                        if (distance < walkableDistance)
                        {
                            canDoAction = true;
                            player.GetComponent<AIController>().agent.SetDestination(hit.point);//move player to hit position
                            if (Vector3.Distance(hit.point, enemy.transform.position) > playerAttackDistance)
                            {
                                canDoAction = false;
                                StartCoroutine(Wait());
                                ChangeTurn();
                            }
                        }
                    }
                }
                if (Vector3.Distance(player.transform.position, hit.point) <= 1.5f && canDoAction)//wait till player reach hit position
                {
                    distance = Vector3.Distance(enemy.transform.position, hit.point);
                    if (distance > healDistance)
                    {
                        playerHealth.value += healAmount;
                        playerHealthAmt += healAmount;
                    }
                }
               
            }
            else
            {
                //tree will run here when reach here due to isPlayerTurn condition apply to whole project
                turnIndicator.text = enemyTurn;
                if (!damaged)
                {
                    distance = Vector3.Distance(enemy.transform.position, player.transform.position);
                    if (distance <= GoToPlayer.enemyHitDistance)
                    {
                        enemyCurrentState = attack;
                        playerHealth.value -= damage;
                        playerHealthAmt -= damage;
                        damaged = true;
                    }
                }
            }
            if (playerHealth.value == 0)
            {
                player.SetActive(false);
                turnIndicator.text = "You Lose";
            }
            else if (enemyHealth.value == 0)
            {
                enemy.SetActive(false);
                turnIndicator.text = "You Win";
            }
        }
    }

    void OpenSettings()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (detectEsc)
            {
                MainGameUI.SetActive(true);
                SettingsUI.SetActive(false);
                detectEsc = false;
            }
            else
            {
                MainGameUI.SetActive(false);
                SettingsUI.SetActive(true);
                detectEsc = true;
            }

        }
        if (!isPlayerTurn)
        {
            attBtn.interactable = false;
        }
    }

    void ChangeGameSettings()
    {
        if(enemyCurrentState == idle)
        {
            enemyStatus.text = idle;
        }
        else if( enemyCurrentState == patrol)
        {
            enemyStatus.text = patrol;
        }
        else if (enemyCurrentState == chase)
        {
            enemyStatus.text = chase;
        }
        else if (enemyCurrentState == attack)
        {
            enemyStatus.text = attack;
        }
        else if (enemyCurrentState == run)
        {
            enemyStatus.text = run;
        }

        if (enemyType.text == heavy)
        {
            enemyTyping = heavy;
        }
        else
        {
            enemyTyping = light;
        }
        if (enemyWeaponUsed.text == range)
        {
            enemyWeapon = range;
        }
        else
        {
            enemyWeapon = melee;
        }
        if (playerWeaponUsed.text == range)
        {
            playerWeapon = range;
            playerAttackDistance = playerRangeAtt;
        }
        else
        {
            playerWeapon = melee;
            playerAttackDistance = playerMeleeAtt;
        }
        if (gameDifficulty.text == easy)
        {
            difficulty = easy;
        }
        else
        {
            difficulty = hard;
        }

    }

    void ShowWalkableRadius(int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;
            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

    void ShowAttackRange(int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;
            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

    public static void ChangeTurn()
    {
        if (isPlayerTurn)
        {
            isPlayerTurn = false;
        }
        else
        {
            isPlayerTurn = true;
        }
        damaged = false;
    }

    public void BtnEnemyType()
    {
        if (enemyType.text == heavy)
        {
            enemyType.text = light;
        }
        else
        {
            enemyType.text = heavy;
        }

    }
    public void BtnEnemyWeapon()
    {
        if (enemyWeaponUsed.text == melee)
        {
            enemyWeaponUsed.text = range;
        }
        else
        {
            enemyWeaponUsed.text = melee;
        }

    }
    public void BtnPlayerWeapon()
    {
        if (playerWeaponUsed.text == melee)
        {
            playerWeaponUsed.text = range;
        }
        else
        {
            playerWeaponUsed.text = melee;
        }
    }
    public void BtnDifficulty()
    {
        if (gameDifficulty.text == easy)
        {
            gameDifficulty.text = hard;
        }
        else
        {
            gameDifficulty.text = easy;
        }

    }
    public void BtnAttack()
    {
        enemyHealth.value -= damage;
        enemyHealthAmt -= damage;
        StartCoroutine(Wait());
        ChangeTurn();

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }
}


