using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject[] rightDoors;
    [SerializeField] private GameObject[] leftDoors;

    private Vector3 openLeftDoor = new Vector3(0, 135, 0);
    private Vector3 openRightDoor = new Vector3(0, -135, 0);
    private Vector3 closeDoor = Vector3.zero;
    void Start()
    {
        DoorCloseOpen();
    }


    private void DoorCloseOpen()
    {
        for (int i = 0; i < leftDoors.Length; i++)
        {
            bool randomBool = (Random.value > 0.5f);
            leftDoors[i].transform.rotation = Quaternion.Euler(randomBool ? openLeftDoor : openLeftDoor);
        }

        for (int i = 0; i < rightDoors.Length; i++)
        {
            bool randomBool = (Random.value > 0.5f);
            rightDoors[i].transform.rotation = Quaternion.Euler(randomBool ? openRightDoor : openRightDoor);
        }
    }
}
