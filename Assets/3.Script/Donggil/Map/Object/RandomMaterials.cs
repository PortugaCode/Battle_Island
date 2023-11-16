using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterials : MonoBehaviour
{
    public Material[] materials;
    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
    }
}
