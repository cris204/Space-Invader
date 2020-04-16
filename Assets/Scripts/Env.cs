using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env 
{
    //Storage
    public const string HIGHSCORE_KEY = "HighScore";
    public const string CURRENT_SHIP_KEY = "CurrentShip";

    //Controllers
    public const Ship CURRENT_SHIP = Ship.green;
    public const float SPEED_MULTIPLIER = 1.05f;

    //Objects Paths
    public const string ENEMY_PATH = "Prefabs/Enemy";
    public const string AUDIO_SOURCE = "Prefabs/AudioSourceObject";

    //Bullets
    public const string BULLET_BLUE_PATH = "Prefabs/Bullets/BulletBlue";
    public const string BULLET_BLUE_VFX_PATH = "Prefabs/Bullets/BulletBlueVfx";


    //PowerUps Paths
    public static int POWER_UPS_COUNT = 1;
    public const string SHIELD_POWER = "Prefabs/PowerUps/ShieldPowerUp";
    public const string SHIELD = "Prefabs/PowerUps/Shield";


    //Audio Paths
    public static string SOUND_LASER = "Sounds/sfx_laser1";
    public static string SOUND_LASER_ENEMY = "Sounds/sfx_laser2";
    public static string SOUND_LOSE = "Sounds/sfx_lose";
    public static string SOUND_SHIELD_UP = "Sounds/sfx_shieldUp";
    public static string SOUND_SHIELD_DOWN = "Sounds/sfx_shieldDown";
    public static string SOUND_TWO_TONE = "Sounds/sfx_twoTone";
    public static string SOUND_ZAP = "Sounds/sfx_zap";


    //Scene Names
    public const string GAME_SCENE = "Game";
    public const string MENU_SCENE = "Menu";


    public static Ship ReturnShipEnum(string currentShip)
    {
        switch (currentShip) {
            case "Blue":
                return Ship.blue;
            case "Green":
                return Ship.green;
            case "Orange":
                return Ship.orange;
            case "Red":
                return Ship.red;
            default:
                return Ship.green;
        }
    }



}
