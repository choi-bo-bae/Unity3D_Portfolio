using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFire : MonoBehaviour
{

    private float rayDis = 10.0f;

    public GameObject bulletEffectFactory;  //총알 이펙트

    public GameObject firePos;   //총열

    private int attackPower = 10;

    

    // Update is called once per frame
    void Update()
    {
      
            Fire();
      
    }


    public void Fire()
    {
       
        Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo;


            if (Physics.Raycast(ray, out hitInfo))
            {

                Debug.DrawRay(ray.origin, hitInfo.transform.position - firePos.transform.position, Color.blue, 0.3f);
                //Debug.DrawRay(ray.origin, ray.direction * rayDis, Color.blue, 0.3f);  

                GameObject bulletEffect = Instantiate(bulletEffectFactory);

                bulletEffect.transform.position = hitInfo.point;

                bulletEffect.transform.forward = hitInfo.normal;

                if (hitInfo.transform.name.Contains("Enemy"))
                {
                    EnemyMove enemy = hitInfo.collider.gameObject.GetComponent<EnemyMove>();
                    enemy.HitDamage(attackPower);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDis, Color.red, 0.3f);
            }


        }
       
    }
    





}
