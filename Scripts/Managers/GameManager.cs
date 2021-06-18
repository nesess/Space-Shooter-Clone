using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public bool gamePaused = false;
    public bool gamePausedByLoot = false;
    public bool noEnemy = false;

    [SerializeField]
    private GameObject partRight;
    private bool rightCanRemove = true;

    [SerializeField]
    private GameObject partLeft;
    private bool leftCanRemove = true;

    [SerializeField]
    private GameObject partRightBack;
    private bool rightBackCanRemove = true;

    [SerializeField]
    private GameObject PartLeftBack;
    private bool leftBackCanRemove = true;

    private int difficulty = 0;
    [SerializeField]
    private int totalExp = 0;

    public static GameManager instance;


    [SerializeField]
    private GameObject inventorySc;

    private void Awake()
    {
        if (GameManager.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            GameManager.instance = this;
        }
       
    }

    private void Start()
    {
        inventorySc.SetActive(true);
        inventorySc.SetActive(false);
    }

    public GameObject equipPart(GameObject prefab,Vector3 pos,Quaternion rot)
    {
        
        var newPart = Instantiate(prefab,player.transform);
        newPart.transform.position = new Vector3(newPart.transform.position.x + pos.x, newPart.transform.position.y + pos.y, newPart.transform.position.z + pos.z);
        newPart.transform.rotation = Quaternion.Euler(rot.x,rot.y,rot.z);
        return newPart;
    }

    public GameObject equipWeapon(GameObject prefab, Vector3 pos, Quaternion rot,string partName)
    {
        Vector3 parentPos = new Vector3(0,0,0);
        
        

        var newPart = Instantiate(prefab, player.transform);
        newPart.transform.position = new Vector3(newPart.transform.position.x + pos.x, newPart.transform.position.y + pos.y, newPart.transform.position.z + pos.z);
        switch (partName)
        {
            case "WeaponAreaRight":
                parentPos = partRight.GetComponent<ShipPartSlot>().objPos;
                break;
            case "WeaponAreaLeft":
                parentPos = partLeft.GetComponent<ShipPartSlot>().objPos;
                break;
            case "WeaponAreaRightBack":
                parentPos = partRightBack.GetComponent<ShipPartSlot>().objPos;
                break;
            case "WeaponAreaLeftBack":
                parentPos = PartLeftBack.GetComponent<ShipPartSlot>().objPos;
                break;
        }
        newPart.transform.position += parentPos;
        newPart.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        return newPart;
    }



    public int getHealth()
    {
        return player.GetComponent<Player>().health;
    }

    public int getMaxHealth()
    {
        return player.GetComponent<Player>().maxHealth;
    }

    public float getSpeed()
    {
        return player.GetComponent<Player>().speed;
    }

    public void setHealth(int plusHealth)
    {
        if(player.GetComponent<Player>().health + plusHealth > player.GetComponent<Player>().maxHealth)
        {
            player.GetComponent<Player>().health = player.GetComponent<Player>().maxHealth;
        }
        else
        {
            player.GetComponent<Player>().health += plusHealth;
        }
        
    }

    public void setMaxHealth(int plusMaxHealth)
    {
        player.GetComponent<Player>().maxHealth += plusMaxHealth;
        player.GetComponent<Player>().health += plusMaxHealth;
    }

    public int getDifficulty()
    {
        return difficulty;
    }

    public void increaseDifficulty()
    {
        difficulty++;
    }

    public void setSpeed(float plusSpeed)
    {
        player.GetComponent<Player>().speed += plusSpeed;
    }

   public void addPart(string name)
    {
        switch (name)
        {
            case "PartRight":
                partRight.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "PartLeft":
                partLeft.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "PartRightBack":
                partRightBack.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "PartLeftBack":
                PartLeftBack.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "WeaponAreaRight":
                rightCanRemove = false;
                break;
            case "WeaponAreaLeft":
                leftCanRemove = false;
                break;
            case "WeaponAreaRightBack":
                rightBackCanRemove = false;
                break;
            case "WeaponAreaLeftBack":
                leftBackCanRemove = false;
                break;
            default:
               
                break;
        }
       
    }

    public int getTotalExp()
    {
        return totalExp;
    }

    public void setTotalExp(int exp)
    {
        totalExp += exp;
        LevelManager.instance.getExp(totalExp);
    }
    
    public void decreaseTotalExp(int exp)
    {
        totalExp -= exp;
    }

    public bool removePart(string name)
    {
        switch (name)
        {
            case "PartRight":
                if(!rightCanRemove)
                {
                    return false;
                }
                else
                {
                    partRight.transform.GetChild(0).gameObject.SetActive(false);

                }
                break;
            case "PartLeft":
                if (!leftCanRemove)
                {
                    return false;
                }
                else
                {
                    partLeft.transform.GetChild(0).gameObject.SetActive(false);

                }
                break;
            case "PartRightBack":
                if (!rightBackCanRemove)
                {
                    return false;
                }
                else
                {
                    partRightBack.transform.GetChild(0).gameObject.SetActive(false);

                }
                break;
            case "PartLeftBack":
                if (!leftBackCanRemove)
                {
                    return false;
                }
                else
                {
                    PartLeftBack.transform.GetChild(0).gameObject.SetActive(false);

                }
                break;
            case "WeaponAreaRight":
                rightCanRemove = true;
                break;
            case "WeaponAreaLeft":
                leftCanRemove = true;
                break;
            case "WeaponAreaRightBack":
                rightBackCanRemove = true;
                break;
            case "WeaponAreaLeftBack":
                leftBackCanRemove = true;
                break;
            default:
                break;
        }
        return true;
    }

   

}
