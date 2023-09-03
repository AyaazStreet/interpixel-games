using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool A1_Unlocked;
    public bool A2_Unlocked;
    public bool A3_Unlocked;

    public bool T1_Unlocked;
    public bool T2_Unlocked;
    public bool T3_Unlocked;

    public int level;

    public PlayerData(PlayerController pc, int l)
    {
        A1_Unlocked = pc.A1_Unlocked;
        A2_Unlocked = pc.A2_Unlocked;
        A3_Unlocked = pc.A3_Unlocked;

        T1_Unlocked = pc.T1_Unlocked;
        T2_Unlocked = pc.T2_Unlocked;
        T3_Unlocked = pc.T3_Unlocked;

        level = l;
    }
}
