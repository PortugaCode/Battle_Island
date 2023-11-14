using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RendomSpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject EnemyAi;
    public List<GameObject> enemyList;

    private Vector3[] Spawnpoint;

    private void Awake()
    {
        enemyList = new List<GameObject>();
        Spawnpoint = new Vector3[5];
        //Spawnpoint = new Vector3[GameManager.instance.Level];

        for (int i = 0; i < Spawnpoint.Length; i++)
        {
            Spawnpoint[i] = GetRandomPoint(new Vector3(16, 0, -31), 60f);
            if (EnemyAi==null)
            {
                Debug.Log("Null");
            }
            
            GameObject CloneEnemy = Instantiate(EnemyAi, Spawnpoint[i], Quaternion.identity);
            enemyList.Add(CloneEnemy);
        }
    }




    private Vector3 GetRandomPoint(Vector3 center, float MaxDistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * MaxDistance + center;
        randomPos.y = 2f;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);

        /*        while (true)
                {
                    NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);
                    if (hit.position.y < 3f)
                    {
                        break;
                    }
                }*/

        return hit.position;
    }
}
