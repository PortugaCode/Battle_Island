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

    public LayerMask layer;

    private void Update()
    {
        
    }

    private void FrontOpenDoor()
    {
        Vector3 size = Vector3.one;
        Collider[] colls = Physics.OverlapBox(Front.transform.position, size / 2, Quaternion.identity, layer);

        foreach(Collider col in colls)
        {
            if(colls.Length >=1 && Input.GetKeyDown(KeyCode.F))
            {
                FrontLeftDoor.transform.Rotate(new Vector3(0, 120, 0));
            }
        }
    }
}
