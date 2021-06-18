using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsShipPartCondition : DropCondition
{
    public override bool Check(DraggableComponent draggable)
    {
        return draggable.GetComponent<ShipPart>() != null;
    }
}
