using System;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public float[] playerStats; // [0] Health, [1] Food, [2] Water
    public float[] playerPositionAndRotation; //pos x,y,z ve rotation x,y,z
    //public string [] inventoryContent;

    public PlayerData(float[]_playerStats, float[] _playerPosAndRot)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;

    }


}
