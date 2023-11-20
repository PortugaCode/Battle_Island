using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int Level;

    public RendomSpawner randomSpawner;

    public int enemyCount; // ������
    public int killCount; // ų��
    public int Ranking; // EnemyCount���� +1 => �÷��̾ �׾��� ��
    public bool isLastEnemy; // ���� �� 1������ �� true


    public bool isLoading = true; // �ε�



    public bool isPlayerDead; //Player �׾��� �� (��)

    public bool isWin; // enemyCount == 0 (��)
    public bool isGameOver; // isWin == true || isplayerDead == true (��)



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void EnemyCount()
    {
        enemyCount = randomSpawner.enemyList.Count;
    }
}
