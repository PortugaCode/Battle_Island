using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RendomSpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject EnemyAi;
    [SerializeField] private GameObject Helic;
    public List<GameObject> enemyList;

    private Vector3[] Spawnpoint;
    private GameObject point;

    private void Awake()
    {
        enemyList = new List<GameObject>();
        point = GetRandomPoint();

        if (GameManager.instance.Level == 0)
        {
            Spawnpoint = new Vector3[5];
        }
        else
        {
            Spawnpoint = new Vector3[GameManager.instance.Level];
        }

        if(GameManager.instance.Level == 7)
        {
            GameObject ColneHelicopter = Instantiate(Helic, point.transform.position, Quaternion.identity);
        }

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
        GameManager.instance.randomSpawner = GameObject.FindObjectOfType<RendomSpawner>().GetComponent<RendomSpawner>();

        GameManager.instance.EnemyCount();
    }





    private Vector3 GetRandomPoint(Vector3 center, float MaxDistance)
    {
        NavMeshHit hit;

        //NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);
        do
        {
            Vector3 randomPos = Random.insideUnitSphere * MaxDistance + center;
            randomPos.y = 1f;
            NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);
        } while (hit.position.y > 3f);

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

    private GameObject GetRandomPoint()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Finish");
        int index = Random.Range(0, 7);

        return a[index];
    }
}
