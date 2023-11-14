using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region MyRegion
    ///* player가 아이템과 충돌했을 때 -> E키를 눌러야 Update가 실행됨
    // * 
    // */
    //[SerializeField] private PlayerController player;
    //private void Start()
    //{
    //    player = FindObjectOfType<PlayerController>();
    //}
    //private void Update()
    //{
    //    AddItem();
    //}
    //public void AddItem()
    //{
    //    if (player.getItem)
    //    {
    //        Debug.Log("슬롯 추가");
    //        player.getItem = false;
    //    }
    //}
    #endregion

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //슬롯 부모
    [SerializeField] private PlayerController player;
    //[SerializeField] private Image slot_img;

    public int count; //초기 슬롯 개수
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (player.getItem)
        {
            player.getItem = false;
            MeetItem();
        }
    }

    private void MeetItem() //아이템 감지하면 슬롯개수 ++
    {
       count++;
       PrefabPlus(count);
    }

    private void PrefabPlus(int count)
    {
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 5f);
    }
    
}
