using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int Level;

    public RendomSpawner randomSpawner;

    public int enemyCount; // 남은적
    public int killCount; // 킬수
    public int Ranking; // EnemyCount에서 +1 => 플레이어가 죽었을 때
    public bool isLastEnemy; // 남은 적 1남았을 때 true


    public bool isLoading = true; // 로딩



    public bool isPlayerDead; //Player 죽었을 때 (완)

    public bool isWin; // enemyCount == 0 (완)
    public bool isGameOver; // isWin == true || isplayerDead == true (완)



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
