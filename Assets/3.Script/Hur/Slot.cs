using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    /* player가 아이템과 충돌했을 때 -> E키를 눌러야 Update가 실행됨
     * 
     */
    [SerializeField] private PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        AddItem();
    }
    public void AddItem()
    {
        if (player.eatItem)
        {
            Debug.Log("슬롯 추가");
        }
    }

}
