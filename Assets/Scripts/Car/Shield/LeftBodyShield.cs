using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBodyShield : CarShield
{
    private void Awake()
    {
        damage = 1f;
        health = 250f;
        area = "left";
    }
}
