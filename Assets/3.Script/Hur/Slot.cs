using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        AddItem();
    }

    private void AddItem()
    {
        if (player.eatItem)
        {
            Debug.Log("슬롯 개수 변경");
        }
    }
}
