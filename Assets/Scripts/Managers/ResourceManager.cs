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
    private Sprite GetSpriteFromSpriteSheet(string path)
    {
        for (int i = 0; i < spritesInSpriteSheet.Length; i++) {
            if (path == spritesInSpriteSheet[i].name) {
                return spritesInSpriteSheet[i];
            }
        }
        return null;
    }

    private string ReturnShipSpriteName(Ship currentShip)
    {
        switch (currentShip) {
            case Ship.blue:
                return "playerShip1_blue.png";
            case Ship.red:
                return "playerShip3_red.png";
            case Ship.green:
                return "playerShip3_green.png";
            case Ship.orange:
                return "playerShip2_orange.png";
            default:
                return "playerShip3_green.png";
        }
    }

    public Sprite GetShipSprite(Ship currentShip)
    {
       return this.GetSpriteFromSpriteSheet(this.ReturnShipSpriteName(currentShip));
    }
    public GameObject GetShip(Ship currentShip)
    {
        return PoolManager.Instance.GetObject(string.Format("Prefabs/Player/{0}Ship", currentShip.ToString()));
    }

    public string GetBulletPath(Ship currentShip)
    {
        switch (currentShip) {
            case Ship.blue:
                return Env.BULLET_BLUE_PATH;
            case Ship.red:
                return Env.BULLET_RED_PATH;
            case Ship.green:
                return Env.BULLET_GREEN_PATH;
            case Ship.orange:
                return Env.BULLET_ORANGE_PATH;
            default:
                return Env.BULLET_GREEN_PATH;
        }
    }


}
