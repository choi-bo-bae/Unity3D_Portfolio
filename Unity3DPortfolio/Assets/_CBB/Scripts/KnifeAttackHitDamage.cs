using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnifeAttackHitDamage : MonoBehaviour
{

    public GameObject player;

  

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(" 검 충돌됨 ");

        if (player.GetComponent<PlayerMove>().state == PlayerMove.PlayerState.KnifeAttack)  //나이프 어택 상태
        {
            Debug.Log(" 플레이어 어택상태임");
            if (player.GetComponent<PlayerChangeWeapon>().Dagger.activeSelf == false)  //나이프 모드
            {
                Debug.Log(" 플레이어 나이프모드임");
                if (other.gameObject.tag == "Enemy")    //적이 맞는지 확인
                {
                    Debug.Log(" 아더가 적임");
                    GameObject.Find("Player").transform.GetComponent<PlayerMove>()
                        .OnTriggerEnter(GameObject.FindGameObjectWithTag("Enemy")
                        .GetComponent<CapsuleCollider>());  //적의 콜라이더 정보를 플레이어에게 넘김
                }
            }
        }

    }
    



}