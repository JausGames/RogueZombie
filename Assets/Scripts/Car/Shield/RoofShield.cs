using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofShield : CarShield
{
    private void Awake()
    {
        damage = 1f;
        health = 200f;
        area = "roof";
    }
}

