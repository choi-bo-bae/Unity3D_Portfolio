using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerChangeWeapon : MonoBehaviour
{
    private int weapon = 0; // %2면 총, 아니면 칼로
    public Image Gun;
    public Image Knife;
    public GameObject ShotGun;
    public GameObject Dagger;
    public Text Fire;
    public Text Attack;

    // Start is called before the first frame update
    void Start()
    {
        Gun.enabled = true;
        Knife.enabled = false;

        ShotGun.SetActive(true);
        Dagger.SetActive(false);

        Fire.enabled = true;
        Attack.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeWeapon()
    {
       
        weapon++;
        if (weapon % 2 != 0)
        {
            Gun.enabled = false;
            Knife.enabled = true;
            //칼 이미지 도출
            ShotGun.SetActive(false);
            Dagger.SetActive(true);
            //무기 대거 도출
            Fire.enabled = false;
            Attack.enabled = true;
        }
        else
        {
            Gun.enabled = true;
            Knife.enabled = false;
            //총 이미지 도출
            ShotGun.SetActive(true);
            Dagger.SetActive(false);
            //무기 총 도출
            Fire.enabled = true;
            Attack.enabled = false;
        }
       
    }

}
