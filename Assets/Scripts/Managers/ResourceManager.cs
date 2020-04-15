using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Sprite[] spritesInSpriteSheet;

    private void Awake()
    {
        spritesInSpriteSheet = Resources.LoadAll<Sprite>("SpriteSheet/sheet");
    }

    public GameObject GetGameObject(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    public AudioClip GetAudio(string path)
    {
        return Resources.Load<AudioClip>(path);
    }
    public Sprite GetSprite(string path)
    {
        for (int i = 0; i < spritesInSpriteSheet.Length; i++) {
            if (path == spritesInSpriteSheet[i].name) {
                return spritesInSpriteSheet[i];
            }
        }
        return null;
    }

}
