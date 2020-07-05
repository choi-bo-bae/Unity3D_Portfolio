using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnifeAttackHitDamage : MonoBehaviour
{

    public GameObject player;

  

    public void OnTriggerEnter(Collider other)
    {


        //Debug.Log(" 검 충돌됨 ");
        if (player.GetComponent<PlayerMove>().state != PlayerMove.PlayerState.KnifeAttack) return;
        if (this.gameObject.GetComponent<PlayerChangeWeapon>().ShotGun.activeSelf == true) return;  
       

            if (other.gameObject.tag == "Enemy")    //적이 맞는지 확인
                {
                  
                     EnemyMove enemyDamage = other.GetComponent<EnemyMove>();
                     enemyDamage.HitDamage(10);
                }
          

    }
    

}