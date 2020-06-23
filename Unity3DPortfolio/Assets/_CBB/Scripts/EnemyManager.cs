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
        if (target != null) //플레이어가 존재하면
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (max < 1)    //10마리. 전부 부숴야 동료에게 갈 수 있도록 설정하기
        {
            curTime += Time.deltaTime;

            if (curTime > spawnTime)
            {
                spawnTime = Random.Range(3.0f, 5.0f);   //스폰 시간 다양하게 랜덤으로

                GameObject enemy = Instantiate(enemyFactory);   //적 프리팹 데려오기

                enemy.transform.position = spawnPoint.transform.position;   //위치 맞춰주기
                
                max++;  //최대 갯수 맞춰주기

                curTime = 0.0f;
            }
        }
    }


}
