using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    Weapon1,
    Weapon2,
    Weapon3,
    Weapon4,
    Weapon5
}

public enum ArmorState
{
    Armor1,
    Armor2,
    Armor3
}

public class GunEnum : MonoBehaviour
{
    public GunState gunState;
    public ArmorState armorState;
}
