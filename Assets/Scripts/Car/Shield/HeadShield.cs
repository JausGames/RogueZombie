using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShield : CarShield
{
    private void Awake()
    {
        damage = 2f;
        health = 300f;
        area = "head";
    }
    
}

