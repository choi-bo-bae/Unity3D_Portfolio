using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;    //싱글톤 만들기
    private void Awake() => Instance = this;

    public Text remainTxt;  //남은 적 텍스트

    private int remainCount = 10;   //총 10마리의 적이 있고 다 부숴야 동료에게 갈 수 있음.

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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
