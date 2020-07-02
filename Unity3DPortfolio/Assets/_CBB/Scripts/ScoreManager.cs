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

    public Text NoticeTxt;  //알아차림 텍스트

    public Color zeroColor;

    public int remainCount = 1;   //총 20마리의 적이 있고 다 부숴야 동료에게 갈 수 있음.

    public Light light;

    float fadeTime = 1.5f;

    private bool remainCheck = false;

    private bool noticeCheck = false;

    

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

    private void Start()
    {
        light.enabled = false;

        GameObject colleague = GameObject.Find("Colleague");
        colleague.GetComponentInChildren<Light>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
       
       remainCounting();
       
    }


    public void remainCounting()
    {
       
        remainTxt.text = "남은 적 : " + remainCount;


        if(remainCount == 0 && remainCheck == false)
        {
            goalTxt.color = Color.white;
            goalTxt.text += " ( 목표 달성 ) ";
            remainCheck = true;
            GameObject colleague = GameObject.Find("Colleague");
            colleague.GetComponentInChildren<Light>().enabled = true;
        }

        if(remainCount <= 19 && noticeCheck == false)
        {
            light.enabled = true;
       
            NoticeTxt.text = " 적이 플레이어의 탈옥을 알아차렸습니다! ";

            NoticeTxt.color = Color.Lerp(NoticeTxt.color, zeroColor , fadeTime * Time.deltaTime);
            if (NoticeTxt.color.a <= 0)
            {
                noticeCheck = true;
            }
        }

      
    }





}
