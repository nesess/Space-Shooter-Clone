using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Text currentLvlText,nextLvlText;
    [SerializeField]
    private GameObject fillExpImg;

    private int[] levelReq = new int[100];

    public int currentLevel = 0;
    private int totalLevelUp = 0;

    public int enemyCount;
    public bool levelComplete = false;

    public static LevelManager instance;

    [SerializeField]
    private GameObject[] enemyList;

    [SerializeField]
    private Transform botLeftLimit;
    [SerializeField]
    private Transform topRightLimit;

    [SerializeField]
    private GameObject enemyContainer;

    [SerializeField]
    private Text levelUpText;

    [SerializeField]
    private Text levelCompleteText;

    private void Awake()
    {
        if (LevelManager.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            LevelManager.instance = this;
        }
        for (int i = 0; i < levelReq.Length; i++)
        {
            levelReq[i] = (i + 1) * 1000;
            
        }

        
    }

    private void Start()
    {


        organizeLevelUI();
        levelStarter();
        
    }

    private void Update()
    {
       


    }
    public void getExp(int totalExp)
    {
        float fillAmount = (0f + totalExp) / (0f + levelReq[currentLevel]);
        organizeLevelUI();
        if (fillAmount > 1f)
        {
            fillExpImg.GetComponent<Image>().fillAmount = 0;
            levelUp();
        }
        
        
        fillExpImg.GetComponent<Image>().fillAmount = Mathf.Min(1f, fillAmount);
        
        

    }

    public void enemyDead()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {

            levelComplete = true;
            GameManager.instance.noEnemy = true;
            StartCoroutine(levelCompleteTextEnum());

        }
    }

    private IEnumerator levelCompleteTextEnum()
    {
        levelCompleteText.gameObject.SetActive(true);
        levelCompleteText.gameObject.transform.localScale = new Vector3(0,0,0);
        float x = 0f;
        while(true)
        {
            x += 0.05f;
            levelCompleteText.gameObject.transform.localScale = new Vector3(x, x, x);
            if(x >= 1f)
            {
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            x -= 0.05f;
            levelCompleteText.gameObject.transform.localScale = new Vector3(x, x, x);
            if (x <= 0f)
            {
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        levelCompleteText.gameObject.SetActive(false);
        StartCoroutine(levelCompleteLootChecker());
    }

    private IEnumerator levelCompleteLootChecker()
    {
        if(totalLevelUp > 0)
        {
            LootManager.instance.giveLoot();
            totalLevelUp--;
        }
        
        while (levelComplete)
        {

            if(!GameManager.instance.gamePausedByLoot && totalLevelUp > 0)
            {
                LootManager.instance.giveLoot();
                totalLevelUp--;
                yield return new WaitForSeconds(3f);
            }
            else if(!GameManager.instance.gamePausedByLoot && totalLevelUp <= 0)
            {
                if(currentLevel/2 -1 == GameManager.instance.getDifficulty())
                {
                    GameManager.instance.increaseDifficulty();
                    
                }
                levelStarter();
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            
        }
        
    }

    private void levelUp()
    {
        GameManager.instance.decreaseTotalExp(levelReq[currentLevel]);
        currentLevel++;
        totalLevelUp++;
        StartCoroutine(levelupEnum());
    }

    private IEnumerator levelupEnum()
    {
        StartCoroutine(levelUpColorChangeEnum());
        levelUpText.gameObject.SetActive(true);
        levelUpText.CrossFadeAlpha(1.0f, 2f, false);
        yield return new WaitForSeconds(2f);
        levelUpText.CrossFadeAlpha(0.0f, 2f, false);
        levelUpText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        StopCoroutine(levelUpColorChangeEnum());
    }

    private IEnumerator levelUpColorChangeEnum()
    {
        while(true)
        {
            levelUpText.color = Random.ColorHSV();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void spawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, new Vector3(Random.Range(botLeftLimit.position.x, topRightLimit.position.x), Random.Range(botLeftLimit.position.y, topRightLimit.position.y), 0), Quaternion.identity, enemyContainer.transform);
    }

    private void levelStarter()
    {
        levelComplete = false;
        GameManager.instance.noEnemy = false;
        int difficulty = GameManager.instance.getDifficulty();
        int enemyAmountToSpawn = (difficulty/2+1 )* (levelReq[currentLevel] / 1000 * 2);
        enemyCount = enemyAmountToSpawn;
        StartCoroutine(spawnEnumerator(enemyAmountToSpawn));

    }

    private IEnumerator spawnEnumerator(int enemyAmountToSpawn)
    {
        while(enemyAmountToSpawn > 0)
        {

            spawnEnemy(enemyList[Random.Range(0, enemyList.Length)]);
            enemyAmountToSpawn--;
            yield return new WaitForSeconds(3f/(GameManager.instance.getDifficulty()+1));

        }

    }

    private void organizeLevelUI()
    {
        currentLvlText.text = currentLevel + "";
        nextLvlText.text = "" + (currentLevel + 1) + "";
        float fillAmount = (0f + GameManager.instance.getTotalExp()) / (0f + levelReq[currentLevel]);
        fillExpImg.GetComponent<Image>().fillAmount = Mathf.Min(1f, fillAmount);
    }

}
