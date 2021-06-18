using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameObject canvasObj;
    private Canvas canvas;

    [SerializeField]
    public GameObject gameSc;
    [SerializeField]
    private GameObject inventorySc;
    [SerializeField]
    private GameObject lootSc;

    [SerializeField]
    private GameObject fillHealth;
    [SerializeField]
    private Text maxHealthText;
    [SerializeField]
    private Text currentHealthText;
    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Text inventoryFullText;
    [SerializeField]
    private Text removeWeaponErrorText;

    [SerializeField]
    private Text deadLvlText;
    [SerializeField]
    private GameObject deadSc;



    public GameObject itemInfo;
    public GameObject itemImageObj;
    public Text itemNameText;
    public Text itemRaityText;
    public Text itemattribute1Text;
    public Text itemattribute2Text;
    public Text specialText;

    public static UIManager instance;

    private bool fullInventoryErrorShowing = false;

    private bool removeWeaponErrorShowing = false;



    
    private void Awake()
    {
        if (UIManager.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            UIManager.instance = this;
        }

       
    }



    public void closeInventory()
    {
        if(!GameManager.instance.gamePausedByLoot)
        {
            GameManager.instance.gamePaused = false;
            gameSc.SetActive(true);
            Time.timeScale = 1;
        }
        
        UIManager.instance.itemInfo.SetActive(false);
        
        inventorySc.SetActive(false);
 

    }

    public void openInventory()
    {
        refreshUI();
        GameManager.instance.gamePaused = true;
        gameSc.SetActive(false);
        inventorySc.SetActive(true);
        Time.timeScale = 0;

    }


    public void refreshUI()
    {
        maxHealthText.text = "" + GameManager.instance.getMaxHealth();
        currentHealthText.text = "" + GameManager.instance.getHealth();
        fillHealth.GetComponent<Image>().fillAmount = (float) GameManager.instance.getHealth() / (float) GameManager.instance.getMaxHealth();
        speedText.text = "Speed: " + GameManager.instance.getSpeed();
    }

    public void inventoryFullError()
    {
        if(!fullInventoryErrorShowing)
        {
            fullInventoryErrorShowing = true;
            inventoryFullText.gameObject.SetActive(true);
            inventoryFullText.CrossFadeAlpha(0.0f, 1.0f, true);
            StartCoroutine(closeError(1.0f));
        }
        
        
    }

    private IEnumerator closeError(float seconds)
    {
        
        yield return new WaitForSecondsRealtime(seconds);
        inventoryFullText.CrossFadeAlpha(1.0f, 0.0f, true);
        inventoryFullText.gameObject.SetActive(false);
        fullInventoryErrorShowing = false;
        
    }

    public void removeWeaponError()
    {
        if (!removeWeaponErrorShowing)
        {
            removeWeaponErrorShowing = true;
            removeWeaponErrorText.gameObject.SetActive(true);
            removeWeaponErrorText.CrossFadeAlpha(0.0f, 1.0f, true);
            StartCoroutine(closeWeaponError());
        }


    }

    private IEnumerator closeWeaponError()
    {
        
        yield return new WaitForSecondsRealtime(1f);
        removeWeaponErrorText.CrossFadeAlpha(1.0f, 0.0f, true);
        removeWeaponErrorText.gameObject.SetActive(false);
        removeWeaponErrorShowing = false;

    }


    public void closelootScreen()
    {

        GameManager.instance.gamePausedByLoot = false;
        GameManager.instance.gamePaused = false;
        gameSc.SetActive(true);
        lootSc.SetActive(false);
        Time.timeScale = 1;
    }

    public void deadScreen()
    {
        gameSc.SetActive(false);
        deadLvlText.text = "YOU DIED \n Current level: " + LevelManager.instance.currentLevel; 
        deadSc.SetActive(true);
    }

    public void restrartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

}
