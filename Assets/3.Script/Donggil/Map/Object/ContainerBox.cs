using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBox : MonoBehaviour
{
    [SerializeField] private bool is2F;

    [SerializeField] private GameObject[] containerBox;
    void Start()
    {
        IsContainer2F();
    }

    private void IsContainer2F()
    {
        is2F = (Random.value > 0.5f);

        if (is2F)
        {
            Instantiate(containerBox[Random.Range(0, containerBox.Length)], transform.position + new Vector3(0, 2.6f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-25.0f, 25.0f))), transform);
        }
    }

}
