using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaker : MonoBehaviour
{
    SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public Texture2D OverwriteSprite(Sprite overwrotten, Sprite overwriter)
    {
        var skin = overwrotten.texture;
        var cloth = overwriter.texture;

        byte[] skinTmp = skin.GetRawTextureData();
        byte[] clothTmp = cloth.GetRawTextureData();

        var height = skin.height;
        var width = skin.width;

        Texture2D skinTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D clothTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D mixTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        clothTex.LoadRawTextureData(clothTmp);
        skinTex.LoadRawTextureData(skinTmp);

        for (int x = 0; x < mixTex.width; x++)
        {
            for (int y = 0; y < mixTex.height; y++)
            {
                mixTex.SetPixel(x, y, skinTex.GetPixel(x, y));
                if (clothTex.GetPixel(x, y).a >= 0.1f) mixTex.SetPixel(x, y, clothTex.GetPixel(x, y));
            }
        }
        mixTex.Apply();

        return mixTex;

    }
    public Sprite ColorSprite(Sprite sprite, Color color)
    {
        var cloth = sprite.texture;

        byte[] clothTmp = cloth.GetRawTextureData();

        var height = cloth.height;
        var width = cloth.width;

        Texture2D clothTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D colorTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        clothTex.LoadRawTextureData(clothTmp);

        for (int x = 0; x < clothTex.width; x++)
        {
            for (int y = 0; y < clothTex.height; y++)
            {
                if (clothTex.GetPixel(x, y).a >= 0.1f) colorTex.SetPixel(x, y, color);
                else colorTex.SetPixel(x, y, Color.clear);
            }
        }

        colorTex.Apply();

        Sprite newSprite = Sprite.Create(colorTex, new Rect(0, 0, colorTex.width, colorTex.height), Vector2.one * 0.5f);

        return newSprite;

    }

    public void SetRend(Sprite sprite)
    {
        rend.sprite = sprite;
    }
}
