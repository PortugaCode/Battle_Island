using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Test_select : MonoBehaviour
{
    public GameObject creat;	// �÷��̾� �г��� �Է�UI
    public Text[] slotText;		// ���Թ�ư �Ʒ��� �����ϴ� Text��
    public Text newItemName;	// ���� �Էµ� �÷��̾��� �г���

    bool[] savefile = new bool[3];	// ���̺����� �������� ����

    void Start()
    {
        // ���Ժ��� ����� �����Ͱ� �����ϴ��� �Ǵ�.
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(Test_Json.instance.path + $"{i}"))	// �����Ͱ� �ִ� ���
            {
                savefile[i] = true;			// �ش� ���� ��ȣ�� bool�迭 true�� ��ȯ
                Test_Json.instance.nowSlot = i;	// ������ ���� ��ȣ ����
                Test_Json.instance.LoadData();	// �ش� ���� ������ �ҷ���
                slotText[i].text = Test_Json.instance.nowItem.itemName;	// ��ư�� �г��� ǥ��
            }
            else	// �����Ͱ� ���� ���
            {
                slotText[i].text = "�������";
            }
        }
        // �ҷ��� �����͸� �ʱ�ȭ��Ŵ.(��ư�� �г����� ǥ���ϱ������̾��� ����)
        Test_Json.instance.DataClear();
    }

    public void Slot(int number)	// ������ ��� ����
    {
        Test_Json.instance.nowSlot = number;	// ������ ��ȣ�� ���Թ�ȣ�� �Է���.

        if (savefile[number])	// bool �迭���� ���� ���Թ�ȣ�� true��� = ������ �����Ѵٴ� ��
        {
            Test_Json.instance.LoadData();	// �����͸� �ε��ϰ�
            GoGame();	// ���Ӿ����� �̵�
        }
        else	// bool �迭���� ���� ���Թ�ȣ�� false��� �����Ͱ� ���ٴ� ��
        {
            Creat();	// �÷��̾� �г��� �Է� UI Ȱ��ȭ
        }
    }

    public void Creat()	// �÷��̾� �г��� �Է� UI�� Ȱ��ȭ�ϴ� �޼ҵ�
    {
        creat.gameObject.SetActive(true);
    }

    public void GoGame()	// ���Ӿ����� �̵�
    {
        if (!savefile[Test_Json.instance.nowSlot])	// ���� ���Թ�ȣ�� �����Ͱ� ���ٸ�
        {
            Test_Json.instance.nowItem.itemName = newItemName.text; // �Է��� �̸��� �����ؿ�
            Test_Json.instance.SaveData(); // ���� ������ ������.
        }
        SceneManager.LoadScene(1); // ���Ӿ����� �̵�
    }
}
