using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailShield : CarShield
{

    private void Awake()
    {
        damage = 1f;
        health = 100f;
        area = "tail";
    }
}

