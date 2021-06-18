using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartSlot : EquipmentSlot
{
    protected override void Awake()
    {

        base.Awake();
        dropArea.dropConditions.Add(new IsShipPartCondition());

    }
}
