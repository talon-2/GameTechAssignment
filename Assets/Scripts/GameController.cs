using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public LineRenderer circleRenderer;
    public float walkDistance;
    public float healStartDistance;
    public float healAmount;
    public float playerHealthAmount;
    public float enemyHealthAmount;
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider enemyHealth;
    public float damage = 1;
    public Text turnIndicator;
    float distance;
    bool canAttack;
    public Text enemyType;
    public Text enemyWeaponUsed;
    public Text playerWeaponUsed;
    public Text gameDifficulty;

    public float playerRangeAtt = 5f;
    public float playerMeleeAtt = 1.5f;

    string heavy = "Heavy";
    string light = "Light";
    string melee = "Melee";
    string range = "Range";
    string easy = "Easy";
    string hard = "Hard";
    string playerTurn = "Player's Turn";
    string enemyTurn = "Enemy's Turn";

    public float playerAttackDistance = 2f;
    public static bool damaged;

    RaycastHit hit;

    public GameObject MainGameUI;
    public GameObject SettingsUI;
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
    }

    void Update()
    {
        OpenSettings();
        ChangeGameSettings();
        if (!detectEsc)
        {
            if (isPlayerTurn)
            {
                turnIndicator.text = playerTurn;
                showRadius(100, walkableDistance);
                if (Input.GetMouseButtonDown(0))
                {
                    canAttack = true;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        distance = Vector3.Distance(player.transform.position, hit.point);
                        if (distance < walkableDistance)
                        {
                            player.GetComponent<AIController>().agent.SetDestination(hit.point);
                            
                            distance = Vector3.Distance(enemy.transform.position, hit.point);
                            if (distance > healDistance)
                            {
                                playerHealth.value += healAmount;
                                playerHealthAmt += healAmount;
                            }
                            if(distance > playerAttackDistance)
                            {
                                canAttack = false;
                            }
                            StartCoroutine(WaitForNextTurn());

                        }
                    }
                }
                if (canAttack)
                {
                    if (Vector3.Distance(player.transform.position, enemy.transform.position) <= playerAttackDistance)
                        //if enemy close enough, damage them
                    {
                        enemyHealth.value -= damage;
                        enemyHealthAmt -= damage;
                        canAttack = false;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(detectEsc)
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
    }

    void ChangeGameSettings()
    {
        if(enemyType.text == heavy)
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
            playerWeapon = range ;
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

    void showRadius(int steps, float  radius)
    {
        circleRenderer.positionCount = steps;

        for(int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float) currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius; 
            Vector3 currentPosition = new Vector3(x,y, 0);

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
    
    IEnumerator WaitForNextTurn()
    {
        yield return new WaitForSeconds(2f);
        ChangeTurn();
    }

    public void BtnEnemyType() { 
        if(enemyType.text == heavy)
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
}


