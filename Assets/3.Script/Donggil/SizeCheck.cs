using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCheck : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    void Start()
    {
        Debug.Log("�ٴ� ������ X: " + mesh.bounds.size.x);
        Debug.Log("�ٴ� ������ Z: " + mesh.bounds.size.z);
    }

}
