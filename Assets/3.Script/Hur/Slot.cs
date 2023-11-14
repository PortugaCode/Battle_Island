using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    /* player�� �����۰� �浹���� �� -> EŰ�� ������ Update�� �����
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
            Debug.Log("���� �߰�");
        }
    }

}
