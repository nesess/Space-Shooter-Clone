using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{

    public List<DropCondition> dropConditions = new List<DropCondition>();
    public event Action<DraggableComponent> onDropHnadler;

    public Quaternion objRotation;
    public Vector3 objPos;
    public int order;
    public DraggableComponent draggable;
    public bool isFilled = false;
    public string slotName = null;
    
    

    public bool Accepts(DraggableComponent draggable)
    {
        return dropConditions.TrueForAll(cond => cond.Check(draggable));
    }

    public void Drop(DraggableComponent draggable)
    {
        onDropHnadler?.Invoke(draggable);
       
    }

}
