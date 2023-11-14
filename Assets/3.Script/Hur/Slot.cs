using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region MyRegion
    ///* player�� �����۰� �浹���� �� -> EŰ�� ������ Update�� �����
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
    //        Debug.Log("���� �߰�");
    //        player.getItem = false;
    //    }
    //}
    #endregion

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //���� �θ�
    [SerializeField] private PlayerController player;
    //[SerializeField] private Image slot_img;

    public int count; //�ʱ� ���� ����
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

    private void MeetItem() //������ �����ϸ� ���԰��� ++
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
