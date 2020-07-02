using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionClear : MonoBehaviour
{
    
    public Text missionClear;
    
    // Start is called before the first frame update
    void Start()
    {
        
        missionClear.enabled = false;
    }

  
    private void OnTriggerEnter(Collider other)
    {
        ScoreManager remainCount = GameObject.Find("ScoreMgr").GetComponent<ScoreManager>();
        if (remainCount.remainCount == 0 && other.gameObject.name == "Player")
        {
            missionClear.enabled = true;  
        }

    }


}
