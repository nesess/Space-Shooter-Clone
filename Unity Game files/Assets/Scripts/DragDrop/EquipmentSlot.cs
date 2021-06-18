using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    public int slotNum;
    public Quaternion objRotation;
    public Vector3 objPos;
    public DropArea dropArea;
    public int order;
    private GameObject equippedObject;
    public bool isFilled = false;
    public string slotName = null;

    protected virtual void Awake()
    {
        dropArea = GetComponent<DropArea>() ?? gameObject.AddComponent<DropArea>();
        dropArea.onDropHnadler += OnItemDropped;

    }

    

    public void OnItemDropped(DraggableComponent draggable)
    {
        draggable.transform.position = transform.position;
        dropArea.objRotation = objRotation;
        dropArea.objPos = objPos;
        dropArea.order = order;
        dropArea.slotName = slotName;
        

    }
}
