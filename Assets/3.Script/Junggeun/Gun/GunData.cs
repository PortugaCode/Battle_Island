using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Gun / GunData", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    public float damage = 10f;

    public int magCapcity = 30; // ÇÑ ÅºÃ¢ÀÇ ÃÖ´ë ÃÑ¾Ë

    public float timebetFire = 0.18f;
    public float reloadTime = 3f;

}
