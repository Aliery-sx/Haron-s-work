using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    
    
    [Header("UI игры")]
    public GameObject player;
    public GameObject waterMan;
    public Player playerScript;
    public EnemySpawner spawnScript;
    public int gameState = 0;

    public Canvas UICanvas;
    public TextMeshProUGUI hpBarText;
    public TextMeshProUGUI spiritHaveText;
    public TextMeshProUGUI spiritMaxText;
    public TextMeshProUGUI daysText;

    public Image bar;
    public float fill;

    [Header("2 фаза игры")]
    public int gameLevel = 0;
    public int days = 0;
    public Transform[] levelPoints;
    [Header("Окно статов")]
    public GameObject statsMenuUI;
    public GameObject hpBarObj;
    public TextMeshProUGUI[] upgradesText;
    public float[] coinsNeeded;
    public int[] upgradeLevels;
    public TextMeshProUGUI coinsHaveText;
    public float coinsHave;



    // Start is called before the first frame update
    void Start()
    {
        fill = 1f;
        player = GameObject.Find("Boat");
        playerScript = player.GetComponent<Player>();
        spawnScript = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        UICanvas = GameObject.Find("UI").GetComponent<Canvas>();

        Invoke(nameof(TakeControlToBoat), 4f);
    }

    // Update is called once per frame
    void Update()
    {
        fill = playerScript.health / playerScript.healthMax;
        bar.fillAmount = fill;

        hpBarText.text = playerScript.health + " / " + playerScript.healthMax;
        spiritHaveText.text = "Spirit in a boat: " + playerScript.spiritHave + " / " + playerScript.spiritNeed;
        daysText.text = "Day: " + days;



        if (playerScript.spiritHave >= playerScript.spiritNeed && gameLevel == 0)
        { 
            FirstLevel();
            //spiritMaxText.enabled = true;
            spiritMaxText.text = "You have collected the required number of souls. It's time to take them to the kingdom of Hades. \n (Press Space)";
        }else spiritMaxText.text = "";

        Shopping();

        if (playerScript.playerIsDead)
        {
            RestartLevel();
            gameState = 0;
            playerScript.playerIsDead = false;
        }

    }

    void FirstLevel()
    {
        if (Input.GetKey("space"))
        {
            if (days == 0)
            {
                gameLevel = 1;
                transform.position = new Vector3(levelPoints[1].position.x, transform.position.y, levelPoints[1].position.z);
                player.transform.position = new Vector3(levelPoints[1].position.x, player.transform.position.y + 2, levelPoints[1].position.z);
                player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (days > 0)
            {
                gameLevel = 2;
                int x = Random.Range(1, 4);
                transform.position = new Vector3(levelPoints[x].position.x, transform.position.y, levelPoints[x].position.z);
                player.transform.position = new Vector3(levelPoints[x].position.x, player.transform.position.y + 2, levelPoints[x].position.z);
                player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            
        }
    }

    void RestartLevel()
    {
        gameLevel = 0;
        days += 1;
        playerScript.health = playerScript.healthMax;
        playerScript.spiritHave = 0;
        playerScript.spiritNeed += Random.Range(1, 4);
        spawnScript.timer = 1000f;

        playerScript.restartLevel = false;
        transform.position = new Vector3(levelPoints[0].position.x, levelPoints[0].transform.position.y + 20f, levelPoints[0].position.z);
        player.transform.position = new Vector3(levelPoints[0].position.x, levelPoints[0].position.y + 3, levelPoints[0].position.z-23f);
        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Invoke(nameof(TakeControlToBoat), 4f);

    }

    public void GoToNextDayB()
    {
        statsMenuUI.SetActive(false);
        hpBarObj.SetActive(true);
        gameState = 0;
        
        RestartLevel();
    }

    void TakeControlToBoat()
    {
        gameState = 1;

    }

    void Shopping()
    {
        if (playerScript.restartLevel)
        {
            statsMenuUI.SetActive(true);
            hpBarObj.SetActive(false);
            //Time.timeScale = 0f;
            gameState = 2;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerScript.endPointShop.transform.position.x - 7f, playerScript.endPointShop.transform.position.y + 20f, playerScript.endPointShop.transform.position.z - 10f), Time.deltaTime * 10f);
            Camera.main.fieldOfView = 50;
        }
    }

    public void HealthUpgrade()
    {
        if (coinsHave >= coinsNeeded[0])
        {
            upgradeLevels[0] += 1;
            coinsNeeded[0] = 10 + 5 * upgradeLevels[0];
            upgradesText[1].text = coinsNeeded[0] + "\n" + upgradeLevels[0] + " lvl";
            playerScript.healthMax = 100 + 50 * upgradeLevels[0];
            upgradesText[0].text = playerScript.healthMax + "\n+50";

            coinsHave -= coinsNeeded[0];
            coinsHaveText.text = coinsHave.ToString();
        }
        
    }

    public void SpeedUpgrade()
    {
        if (coinsHave >= coinsNeeded[1])
        {
            upgradeLevels[1] += 1;
            coinsNeeded[1] = 5 + 3 * upgradeLevels[1];
            upgradesText[3].text = coinsNeeded[1] + "\n" + upgradeLevels[1] + " lvl";
            playerScript.speed = 8 + 1 * upgradeLevels[1];
            upgradesText[2].text = playerScript.speed + "\n+1";

            coinsHave -= coinsNeeded[1];
            coinsHaveText.text = coinsHave.ToString();
        }
        
    }

    public void MobilityUpgrade()
    {
        if (coinsHave >= coinsNeeded[2])
        {
            upgradeLevels[2] += 1;
            coinsNeeded[2] = 10 + 5 * upgradeLevels[2];
            upgradesText[5].text = coinsNeeded[2] + "\n" + upgradeLevels[2] + " lvl";
            playerScript.rotatePower = 1f + 0.1f * upgradeLevels[2];
            upgradesText[4].text = playerScript.rotatePower + "\n+0.1";

            coinsHave -= coinsNeeded[2];
            coinsHaveText.text = coinsHave.ToString();
        }
        
    }

    public void FlowUpgrade()
    {
        if (coinsHave >= coinsNeeded[3])
        {
            upgradeLevels[3] += 1;
            coinsNeeded[3] = 5 + 3 * upgradeLevels[3];
            upgradesText[7].text = coinsNeeded[3] + "\n" + upgradeLevels[3] + " lvl";
            playerScript.flowPower = 20f + 10f * upgradeLevels[3];
            upgradesText[6].text = playerScript.flowPower + "\n+10";

            coinsHave -= coinsNeeded[3];
            coinsHaveText.text = coinsHave.ToString();
        }
        
    }
}
