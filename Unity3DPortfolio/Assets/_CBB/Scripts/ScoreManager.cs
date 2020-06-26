using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;    //싱글톤 만들기
    private void Awake() => Instance = this;

    public Text remainTxt;  //남은 적 텍스트

    public Text goalTxt;    //게임 목표 텍스트

    private int remainCount = 1;   //총 20마리의 적이 있고 다 부숴야 동료에게 갈 수 있음.

    public int RemainCount
    {
        get
        {
            return remainCount;
        }
        set
        {
            remainCount = value;
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        //적이 파괴될 때 라는 조건 넣기
        remainCounting();
    }


    public void remainCounting()
    {
       
        remainTxt.text = "남은 적 : " + remainCount;

        if(remainCount == 0)
        {
            goalTxt.color = Color.white;
            goalTxt.text += " ( 목표 달성 ) ";
            Destroy(gameObject);
        }
    }

}
