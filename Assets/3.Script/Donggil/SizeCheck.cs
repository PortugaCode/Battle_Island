using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCheck : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    void Start()
    {
        Debug.Log("바다 사이즈 X: " + mesh.bounds.size.x);
        Debug.Log("바다 사이즈 Z: " + mesh.bounds.size.z);
    }

}
