using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WearableInv : MonoBehaviour
{
    /*
     아이템 type 검사
     Uisngtype -> itemType 순으로 질문
     */

    public List<GameObject> Head = new List<GameObject>();
    public List<GameObject> Armor = new List<GameObject>();
    public List<GameObject> Bag = new List<GameObject>();
    public List<GameObject> Weapon = new List<GameObject>();

    [SerializeField] private SlotManager slot;

    public int selecteditem; 

    private void Start()
    {
        slot = FindObjectOfType<SlotManager>();

    }
    public void CheckType()
    {
        
    }
}
