using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_getitemButton : MonoBehaviour
{
    [SerializeField] private Test_Image image;

    private void Start()
    {
        image = FindObjectOfType<Test_Image>();
    }
    public void OnClickBtn()
    {

    }
}
