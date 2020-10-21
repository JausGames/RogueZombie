using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shieldometer : MonoBehaviour
{
    //[SerializeField] private Player player;
    [SerializeField] private Image head;
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    [SerializeField] private Image tail;
    [SerializeField] private Image roof;


    public void SetHealth(float health, string side)
    {
        var maxhealth = 0f;
        var trans = 1f;

        if (side == "head") maxhealth = 300f;
        if (side == "left" || side == "right") maxhealth = 250f;
        if (side == "tail") maxhealth = 100f;
        if (side == "roof") maxhealth = 200f;

        var r = Mathf.Clamp(0.6f - (2/ maxhealth) *0.4f + ((health/ maxhealth) + 1 / maxhealth) * 0.4f, 0.6f, 1f);
        var gb = Mathf.Clamp(2 / maxhealth + ((health / maxhealth) + 1 / maxhealth), 0f, 1f);

        if (health == 0f) { r = 0f; trans = 0.3f; }

        if (side == "head") head.color = new Color(r, gb, gb, trans);
        if (side == "left") left.color = new Color(r, gb, gb, trans);
        if (side == "right") right.color = new Color(r, gb, gb, trans);
        if (side == "tail") tail.color = new Color(r, gb, gb, trans);
        if (side == "roof") roof.color = new Color(r, gb, gb, trans);



    }
}
