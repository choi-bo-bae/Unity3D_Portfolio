using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnifeAttackHitDamage : MonoBehaviour
{

    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (player.GetComponent<PlayerMove>().state == PlayerMove.PlayerState.KnifeAttack)  //나이프 어택 상태
        {
            if (player.GetComponent<PlayerChangeWeapon>().ShotGun.activeSelf == false)  //나이프 모드
            {
                if (other.gameObject.tag == "Enemy")
                {
                    GameObject.Find("Player").transform.GetComponent<PlayerMove>()
                        .OnTriggerEnter(GameObject.FindGameObjectWithTag("Enemy")
                        .GetComponent<CapsuleCollider>());
                }
            }
        }

    }
    



}