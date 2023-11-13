using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData : MonoBehaviour
{
    [SerializeField] public List<GameObject> SurItemSlot = new List<GameObject>();
    public bool touch = false;
    
    //먹고 리스트에 넣는 구조
    //주변 리스트 만들고 아이템 정보 넣고 클릭했을 때 몇번째 슬롯인지 확인 
    
    private void Update()
    {
        AddSlot();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touch = true;
        }
    }
    private void AddSlot()
    {
        if (touch)//Player tag에 충돌했을 때
        {
            for (int i = 0; i < SurItemSlot.Count; i++)
            {
                Debug.Log("슬롯 추가");
            }
        }
        
    }

    
}
