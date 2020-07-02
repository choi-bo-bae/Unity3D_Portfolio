using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionClear : MonoBehaviour
{
    
    public Text missionClear;

    public AudioClip missionClearSound;

    private AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        missionClear.enabled = false;
    }

  
    private void OnTriggerEnter(Collider other)
    {
        ScoreManager remainCount = GameObject.Find("ScoreMgr").GetComponent<ScoreManager>();
        if (remainCount.remainCount == 0 && other.gameObject.name == "Player")
        {
            audio.PlayOneShot(missionClearSound);
            missionClear.enabled = true;  
        }

    }


}
