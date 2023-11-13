using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemData
{
    public string itemName;
    public int itemID;
    public int itemCount;
}

public class Test_Json : MonoBehaviour
{
    public static Test_Json instance;

    public ItemData nowItem = new ItemData();

    public string path;
    public int nowSlot; 
    //public string filename = "save";

    private void Awake()
    {
        #region ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/save";
        Debug.Log(path);
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowItem);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowItem = JsonUtility.FromJson<ItemData>(data);
    }
    public void DataClear()
    {
        nowSlot = -1;
        nowItem = new ItemData();
    }
}
