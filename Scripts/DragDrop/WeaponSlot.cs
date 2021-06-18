using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : EquipmentSlot
{
    
    protected override void Awake()
    {

        base.Awake();
        dropArea.dropConditions.Add(new IsWeaponCondition());

    }

}
