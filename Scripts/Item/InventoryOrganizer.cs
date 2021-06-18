using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryOrganizer : MonoBehaviour
{
    public static InventoryOrganizer instance;

    private EquipmentSlot[] inventoryList;

    [SerializeField]
    private GameObject primaryWeapon;

    

    private void Awake()
    {
        if (InventoryOrganizer.instance)
        {
            Destroy(base.gameObject);
        }
        else
        {
            InventoryOrganizer.instance = this;
        }
        inventoryList = this.gameObject.GetComponentsInChildren<EquipmentSlot>();
    }

    private void Start()
    {
        getNewItem(primaryWeapon);
    }

    public bool getNewItem(GameObject item)
    {
        int i;
        bool found = false;
        for ( i = 0; i < inventoryList.Length; i++)
        {
            if (!inventoryList[i].dropArea.isFilled)
            {
                found = true;
                break;
                
            }
        }
        if(!found)
        {
        
            return false;
        }
        DraggableComponent draggable = item.GetComponent<DraggableComponent>();
        inventoryList[i].isFilled = true;
        DropArea dropArea = inventoryList[i].gameObject.GetComponent<DropArea>();
        dropArea.isFilled = true;
        if (draggable.lastDrop != null)
        {
            draggable.lastDrop.isFilled = false;
            inventoryList[i].GetComponent<EquipmentSlot>().OnItemDropped(draggable);
            inventoryList[i].isFilled = true;
        }
        draggable.lastDrop = dropArea;
        dropArea.Drop(draggable);

        return true;
    }

    

}
