using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnifeAttackHitDamage : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Enemy")
        {
            GameObject.Find("Player").transform.GetComponent<PlayerMove>()
                .OnTriggerEnter(GameObject.FindGameObjectWithTag("Enemy")
                .GetComponent<CapsuleCollider>());
        }

    }
    


}