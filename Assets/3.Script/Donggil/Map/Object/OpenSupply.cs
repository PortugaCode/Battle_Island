using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSupply : MonoBehaviour
{
    public GameObject FrontLeftDoor;       //120
    public GameObject FrontRightDoor;      //-120
    public GameObject BackLeftDoor;
    public GameObject BackRightDoor;

    public Collider Front;
    public Collider Back;


    Quaternion OpenLeft = Quaternion.Euler(0, 120f, 0);
    Quaternion OpenRight = Quaternion.Euler(0, -120f, 0);

    Quaternion CloseLeft = Quaternion.Euler(0, 0, 0);
    Quaternion CloseRight = Quaternion.Euler(0, 0, 0);

    public bool isFrontOpen = false;
    public bool isBackOpen = false;

    private float smooth = 5.0f;

    public LayerMask layer;

    private void Update()
    {
        OpenFrontDoor();
        OpenBackDoor();
    }

    private void OpenFrontDoor()
    {
        Vector3 sizeF = Front.bounds.size;

        Collider[] collsFront = Physics.OverlapBox(Front.transform.position, sizeF / 2, Quaternion.identity, layer);



        foreach (Collider col in collsFront)
        {
            if (col.CompareTag("Player"))
            {
                if (collsFront.Length >= 1 && Input.GetKeyDown(KeyCode.F))
                {
                    isFrontOpen = true;
                }
            }
        }

        if (isFrontOpen)
        {
            FrontLeftDoor.transform.localRotation = Quaternion.Slerp(FrontLeftDoor.transform.localRotation, OpenLeft, smooth * Time.deltaTime);
            FrontRightDoor.transform.localRotation = Quaternion.Slerp(FrontRightDoor.transform.localRotation, OpenRight, smooth * Time.deltaTime);
        }
    }

    private void OpenBackDoor()
    {
        Vector3 sizeB = Back.bounds.size;

        Collider[] collsBack = Physics.OverlapBox(Back.transform.position, sizeB / 2, Quaternion.identity, layer);



        foreach (Collider col in collsBack)
        {
            if (col.CompareTag("Player"))
            {
                if (collsBack.Length >= 1 && Input.GetKeyDown(KeyCode.F))
                {
                    isBackOpen = true;
                }
            }
        }

        if (isBackOpen)
        {
            BackLeftDoor.transform.localRotation = Quaternion.Slerp(BackLeftDoor.transform.localRotation, OpenLeft, smooth * Time.deltaTime);
            BackRightDoor.transform.localRotation = Quaternion.Slerp(BackRightDoor.transform.localRotation, OpenRight, smooth * Time.deltaTime);
        }
    }
}
