using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour
{
    SpriteMaker maker;
    [SerializeField] Sprite skinArm;
    [SerializeField] Sprite clothArm;
    [SerializeField] Sprite body;
    [SerializeField] Sprite legs;
    [SerializeField] List<Color> colors = new List<Color>();

    private void Start()
    {
        colors.Add(Color.clear);
        maker = GetComponent<SpriteMaker>();
        var enemies = GetComponentsInChildren<EnemyZombie>();
        for (var i = 0; i < enemies.Length; i++)
        {
            var rndColor = Random.Range(0, colors.Count + 1);
            var rndPants = Random.Range(0, 10);
            if (rndColor != colors.Count) enemies[i].SetColor(colors[rndColor], rndPants >= 2);
            if (rndColor == colors.Count && rndPants >= 2) enemies[i].SetPants();

        }
        
    }

}
