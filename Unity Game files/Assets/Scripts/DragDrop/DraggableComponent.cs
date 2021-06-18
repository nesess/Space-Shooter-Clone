using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableComponent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> onBeginDragHnadler;
    public event Action<PointerEventData> onDragHnadler;
    public event Action<PointerEventData , bool> onEndDragHnadler;

    
    public GameObject prefab;
    
    public bool fallowCursor { get; set; } = true;
    public Vector3 startPos;

    public bool canDrag { get; set; } = true;

    private RectTransform myRect;

    [SerializeField]
    private GameObject canvasObj;
    [SerializeField]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject equippedObject;
    public DropArea lastDrop = null;
    
    

    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasObj = GameObject.Find("Canvas");
        canvas = canvasObj.GetComponent<Canvas>();
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!canDrag)
        {
            return;
        }
        myRect.SetAsLastSibling();
        
        onBeginDragHnadler?.Invoke(eventData);
        canvasGroup.alpha = .6f;
        
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if(!canDrag)
        {
            return;
        }

        onDragHnadler?.Invoke(eventData);

        if(fallowCursor)
        {
            myRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
       
        if(!canDrag)
        {
            return;
        }
        canvasGroup.alpha = 1f;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        DropArea dropArea = null;

        foreach(var result in results)
        {
            dropArea = result.gameObject.GetComponent<DropArea>();

            if(dropArea != null)
            {
                break;
            }
        }
       
        if (dropArea != null && !dropArea.isFilled)
        {
            if(dropArea.Accepts(this))
            {  
                if (!GameManager.instance.removePart(lastDrop.slotName) )
                {
                    UIManager.instance.removeWeaponError();
                }
                else
                {
                    dropArea.isFilled = true;
                    if (lastDrop != null)
                    {
                        lastDrop.isFilled = false;
                    }
                    dropArea.Drop(this);
                    onEndDragHnadler?.Invoke(eventData, true);


                    if (equippedObject != null)
                    {
                        if (GetComponent<ShipPart>() != null)
                        {
                            ShipPart shipPart = GetComponent<ShipPart>();
                            GameManager.instance.setMaxHealth(-shipPart.plusHealth);
                            GameManager.instance.setSpeed(-shipPart.plusSpeed);
                            UIManager.instance.refreshUI();
                        }
                        Destroy(equippedObject);
                    }
                    if (dropArea.gameObject.GetComponent<DestroySlot>() != null)
                    {
                        dropArea.isFilled = false;
                        Destroy(this.gameObject);
                    }
                    else if (dropArea.GetComponent<ShipPartSlot>() != null)
                    {
                        if (GetComponent<ShipPart>() != null)
                        {
                            ShipPart shipPart = GetComponent<ShipPart>();
                            GameManager.instance.setMaxHealth(shipPart.plusHealth);
                            GameManager.instance.setSpeed(shipPart.plusSpeed);
                            UIManager.instance.refreshUI();
                        }
                        equippedObject = GameManager.instance.equipPart(prefab, dropArea.objPos, dropArea.objRotation);
                        equippedObject.GetComponent<SpriteRenderer>().sortingOrder = dropArea.order;
                    }
                    else if (dropArea.GetComponent<WeaponSlot>() != null)
                    {

                        if (GetComponent<Weapon>() != null)
                        {
                            Weapon weapon = GetComponent<Weapon>();
                            equippedObject = GameManager.instance.equipWeapon(prefab, dropArea.objPos, dropArea.objRotation,dropArea.slotName);
                            equippedObject.GetComponent<SpriteRenderer>().sortingOrder = dropArea.order;
                            if(equippedObject.GetComponent<Shooter>() != null)
                            {
                                Shooter equippedObjectShooter = equippedObject.GetComponent<Shooter>();
                                equippedObjectShooter.fireRate = weapon.fireRate;
                                equippedObjectShooter.damage = weapon.damage;
                            }
                            else if(equippedObject.GetComponent<DoubleShooter>() != null)
                            {
                                DoubleShooter equippedObjectShooter = equippedObject.GetComponent<DoubleShooter>();
                                equippedObjectShooter.fireRate = weapon.fireRate;
                                equippedObjectShooter.damage = weapon.damage;
                            }
                        }
                    }
                    GameManager.instance.addPart(dropArea.slotName);
                    lastDrop = dropArea;
                    return;
                }
            }
        }
        
        myRect.SetAsFirstSibling();
        myRect.anchoredPosition = startPos;

        onEndDragHnadler?.Invoke(eventData, false);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        
        startPos = myRect.anchoredPosition;
        UIManager.instance.itemInfo.SetActive(true);
        UIManager.instance.itemImageObj.GetComponent<Image>().sprite = this.gameObject.transform.Find("MyImage").GetComponent<Image>().sprite;
        UIManager.instance.itemImageObj.transform.rotation = Quaternion.Euler(0, 0, 180);


        if(GetComponent<ShipPart>() != null)
        {
            ShipPart shipPart = GetComponent<ShipPart>();

            UIManager.instance.itemNameText.text = shipPart.myName;
            
            
            if (shipPart.raity == "Common")
            {
                UIManager.instance.itemRaityText.color = Color.green;
            }
            else if (shipPart.raity == "Rare")
            {
                UIManager.instance.itemRaityText.color = Color.blue;
                
            }
            else if (shipPart.raity == "Epic")
            {
                UIManager.instance.itemRaityText.color = Color.magenta;
            }
            else if (shipPart.raity == "Legendary")
            {
                UIManager.instance.itemRaityText.color = Color.yellow;
            }
            UIManager.instance.itemRaityText.text = shipPart.raity;
            

            UIManager.instance.itemattribute1Text.text = "Health: " + shipPart.plusHealth;
            UIManager.instance.itemattribute2Text.text = "Speed: " + shipPart.plusSpeed;
            UIManager.instance.specialText.text = "Special: " + shipPart.special;

        }
        else if(GetComponent<Weapon>() != null)
        {
            Weapon weapon = GetComponent<Weapon>();
            UIManager.instance.itemNameText.text = weapon.myName;

            if (weapon.raity == "Common")
            {
                UIManager.instance.itemRaityText.color = Color.green;
            }
            else if (weapon.raity == "Rare")
            {
                UIManager.instance.itemRaityText.color = Color.blue;

            }
            else if (weapon.raity == "Epic")
            {
                UIManager.instance.itemRaityText.color = Color.magenta;
            }
            else if (weapon.raity == "Legendary")
            {
                UIManager.instance.itemRaityText.color = Color.yellow;
            }
            UIManager.instance.itemRaityText.text = weapon.raity;

            UIManager.instance.itemattribute1Text.text = "Damage: " + weapon.damage;
            UIManager.instance.itemattribute2Text.text = "Fire Rate: " + weapon.fireRate;
            UIManager.instance.specialText.text = "Special: " + weapon.special;

        }
        


    }
}
