using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : MonoBehaviour
{
    [SerializeField]
    private GameObject items;

    [SerializeField]
    private GameObject[] weaponList;

    [SerializeField]
    private GameObject[] partList;

    [SerializeField]
    private GameObject[] uiElementsList;

    [SerializeField]
    private GameObject lootScreen;

    private int totalPartDifference = 0;

    public GameObject[] loots;

    public bool testGetLoot = false;

    public static LootManager instance;

    private void Awake()
    {
        if (LootManager.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            LootManager.instance = this;
        }
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if(testGetLoot)
        {
            testGetLoot = false;
            giveLoot();
        }
    }


    public void giveLoot()
    {
        organizeUI(create3xLoot());
    }

    private GameObject[] create3xLoot()
    {
       
        loots = new GameObject[3];
        
        

        for (int i=0; i < 3; i++)
        {
            int k = Random.Range(1, 11);
            
            if (k < Mathf.Max(2, Mathf.Min(6 + totalPartDifference, 9)))
            {
                totalPartDifference--;
                loots[i] = Instantiate(partList[Random.Range(0, partList.Length)], new Vector3(0, 0, 0), Quaternion.identity,items.transform);
                loots[i].SetActive(false);
                int lootRate = Random.Range(1  , 11 + LevelManager.instance.currentLevel );
                ShipPart lootShipPart = loots[i].GetComponent<ShipPart>();
                if (lootRate >9 && lootRate < 18)
                {
                    
                    lootShipPart.raity = "Rare";
                    lootShipPart.plusHealth = (int) (lootShipPart.plusHealth * 2.0);
                    lootShipPart.plusSpeed = lootShipPart.plusSpeed * 2.0f;
                    lootShipPart.GetComponentsInChildren<Image>()[0].color = Color.blue;
                    
                }
                else if(lootRate > 17 && lootRate < 26)
                {
                    lootShipPart.raity = "Epic";
                    lootShipPart.plusHealth = (int)(lootShipPart.plusHealth * 3.5);
                    lootShipPart.plusSpeed = lootShipPart.plusSpeed * 3.5f;
                    lootShipPart.GetComponentsInChildren<Image>()[0].color = Color.magenta;
                }
                else if(lootRate > 25 )
                {
                    lootShipPart.raity = "Legendary";
                    lootShipPart.plusHealth = (int)(lootShipPart.plusHealth * 6.0);
                    lootShipPart.plusSpeed = lootShipPart.plusSpeed * 6.0f;
                    lootShipPart.GetComponentsInChildren<Image>()[0].color = Color.yellow;
                }
                

            }
            else
            {
                totalPartDifference++;
                loots[i] =  Instantiate(weaponList[Random.Range(0, weaponList.Length)],new Vector3(20,20,20),Quaternion.identity, items.transform);
                int lootRate = Random.Range(1 + GameManager.instance.getDifficulty(), 11 + GameManager.instance.getDifficulty());
                Weapon lootWeapon = loots[i].GetComponent<Weapon>();
                loots[i].SetActive(false);
                if (lootRate > 9 && lootRate < 18)
                {
                    lootWeapon.raity = "Rare";
                    lootWeapon.damage = (int)(lootWeapon.damage * 2.0);
                    lootWeapon.fireRate = lootWeapon.fireRate * 2.0f;
                    lootWeapon.GetComponentsInChildren<Image>()[0].color = Color.blue;

                }
                else if (lootRate > 17 && lootRate < 26)
                {
                    lootWeapon.raity = "Epic";
                    lootWeapon.damage = (int)(lootWeapon.damage * 3.5);
                    lootWeapon.fireRate = lootWeapon.fireRate * 3.5f;
                    lootWeapon.GetComponentsInChildren<Image>()[0].color = Color.magenta;
                }
                else if (lootRate > 25)
                {
                    lootWeapon.raity = "Legendary";
                    lootWeapon.damage = (int)(lootWeapon.damage * 6.0);
                    lootWeapon.fireRate = lootWeapon.fireRate * 6.0f;
                    lootWeapon.GetComponentsInChildren<Image>()[0].color = Color.yellow;
                }

            }
        }

        return loots;
    }

    private void organizeUI(GameObject[] loots)
    {
        lootScreen.SetActive(true);
        GameManager.instance.gamePaused = true;
        GameManager.instance.gamePausedByLoot = true;
        Time.timeScale = 0;
        UIManager.instance.gameSc.SetActive(false);
        for (int i = 0;i<3;i++)
        {
            if (loots[i].GetComponent<ShipPart>() != null)
            {
                
                ShipPart shipPart = loots[i].GetComponent<ShipPart>();

                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Name").GetComponent<Text>().text = shipPart.myName;


                if (shipPart.raity == "Common")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.green;
                }
                else if (shipPart.raity == "Rare")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.blue;

                }
                else if (shipPart.raity == "Epic")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.magenta;
                }
                else if (shipPart.raity == "Legendary")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.yellow;
                }
                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().text = shipPart.raity;


                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("BonusHealth").GetComponent<Text>().text = "Health: " + shipPart.plusHealth;
                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("BonusSpeed").GetComponent<Text>().text = "Speed: " + shipPart.plusSpeed;
                uiElementsList[i].transform.Find("Special").GetComponent<Text>().text = "Special: " + shipPart.special;
                
            }
            else if (loots[i].GetComponent<Weapon>() != null)
            {
                
                Weapon weapon = loots[i].GetComponent<Weapon>();
                uiElementsList[i].transform.Find("ItemTextLayout").Find("Name").GetComponent<Text>().text = weapon.myName;

                if (weapon.raity == "Common")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.green;
                }
                else if (weapon.raity == "Rare")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.blue;

                }
                else if (weapon.raity == "Epic")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.magenta;
                }
                else if (weapon.raity == "Legendary")
                {
                    uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().color = Color.yellow;
                }
                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("Raity").transform.Find("RaityText").GetComponent<Text>().text = weapon.raity;

                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("BonusHealth").GetComponent<Text>().text = "Damage: " + weapon.damage;
                uiElementsList[i].transform.Find("ItemTextLayout").transform.Find("BonusSpeed").GetComponent<Text>().text = "Fire Rate: " + weapon.fireRate;
                uiElementsList[i].transform.Find("Special").GetComponent<Text>().text = "Special: " + weapon.special;
            }

            uiElementsList[i].transform.Find("ItemImage").GetComponent<Image>().sprite = loots[i].transform.Find("MyImage").GetComponent<Image>().sprite;
            uiElementsList[i].transform.Find("ItemImage").transform.rotation = Quaternion.Euler(0, 0, 180);

        }

    }

    public void exitLootScreen()
    {
        UIManager.instance.closelootScreen();
        Destroy(loots[1]);
        Destroy(loots[2]);
        Destroy(loots[0]);
    }

    public void lootChoice0()
    {
        GameObject newItem = loots[0];
        newItem.SetActive(true);
        bool itemCheck = InventoryOrganizer.instance.getNewItem(newItem);


        if(itemCheck)
        {
            UIManager.instance.closelootScreen();
            Destroy(loots[1]);
            Destroy(loots[2]);
            
        }
        else
        {
            UIManager.instance.inventoryFullError();
        }
    }

    public void lootChoice1()
    {
        GameObject newItem = loots[1];
        newItem.SetActive(true);
        bool itemCheck = InventoryOrganizer.instance.getNewItem(newItem);


        if (itemCheck)
        {
            UIManager.instance.closelootScreen();
            Destroy(loots[0]);
            Destroy(loots[2]);
            

        }
        else
        {
            UIManager.instance.inventoryFullError();
        }
    }

    public void lootChoice2()
    {
        GameObject newItem = loots[2];
        newItem.SetActive(true);
        bool itemCheck = InventoryOrganizer.instance.getNewItem(newItem);


        if (itemCheck)
        {
            UIManager.instance.closelootScreen();
            Destroy(loots[0]);
            Destroy(loots[1]);
           
        }
        else
        {
            UIManager.instance.inventoryFullError();
        }
    }

    
    
    

}
