using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionClear : MonoBehaviour
{
    
    public Text missionClear;

    public AudioClip missionClearSound;

    private AudioSource audio;

    private float curTime = 0.0f;

    private float resetTime = 3.0f;

    public static MissionClear Instance;

    public Image fadeOut;

    public Color zeroColor;

    private float fadeTime = 3.0f;


    private void Awake()
    {
        if(Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


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

   
    private void Update()
    {
        if(missionClear.enabled == true)
        {
            curTime += Time.deltaTime;

            fadeOut.color = Color.Lerp(fadeOut.color, zeroColor, fadeTime * Time.deltaTime);

            if(curTime >= resetTime)
            {
                LoadScene("StartScene");
            }
        }
    }

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }

}
