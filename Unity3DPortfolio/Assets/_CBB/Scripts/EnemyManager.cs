using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject target;   //플레이어

    public GameObject enemyFactory;     //애너미 공장

    private float spawnTime = 3.0f;     //스폰 시간. 업데이트에서 랜덤으로 처리함
    private float curTime = 0.0f;

    public GameObject spawnPoint;        //스폰 장소

    private int max = 0;    //최대로 필드에 나와있을 갯수

    private void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (max < 1)
        {
            curTime += Time.deltaTime;

            if (curTime > spawnTime)
            {
                spawnTime = Random.Range(2.0f, 3.0f);

                GameObject enemy = Instantiate(enemyFactory);

                enemy.transform.position = spawnPoint.transform.position;

               

                max++;

                curTime = 0.0f;
            }
        }
    }


}
