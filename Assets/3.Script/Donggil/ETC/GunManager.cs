using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public enum Gun
    {
        Rifle1,
        Rifle2,
        Rifle3,
        Rifle4,
        Rifle5
    }

    public GameObject gunObject;
    public GameObject[] gunPrefabs;

    public Gun gun;

    public void GunObject()
    {
        switch(gun)
        {
            case Gun.Rifle1:
                gunObject = Instantiate(gunPrefabs[0], transform);
                break;
            case Gun.Rifle2:
                gunObject = Instantiate(gunPrefabs[1], transform);
                break;
            case Gun.Rifle3:
                gunObject = Instantiate(gunPrefabs[2], transform);
                break;
            case Gun.Rifle4:
                gunObject = Instantiate(gunPrefabs[3], transform);
                break;
            case Gun.Rifle5:
                gunObject = Instantiate(gunPrefabs[4], transform);
                break;
            default:
                break;
        }
    }

    

}
