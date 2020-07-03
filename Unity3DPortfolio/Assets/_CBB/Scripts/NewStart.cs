using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class NewStart : MonoBehaviour
{

    public Image NewStartPressed;
    public GameObject Button;

    public static NewStart Instance;

    private AudioSource audio;

    private float startFade = 1f;

    private void Awake()
    {
        if(Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

    }


    public string GetScanaName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    private void Start()
    {
       
        Button.gameObject.SetActive(true);
        
        NewStartPressed.enabled = false;

        audio = GetComponent<AudioSource>();
       
    }


    public void ChangeScene()
    {
        Button.gameObject.SetActive(true);

        NewStartPressed.enabled = false;

        LoadScene("GameScene");

    }


    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }

    private void Update()
    {
        Color fadeColor = Button.GetComponent<Image>().color;

        fadeColor.a = Mathf.PingPong(Time.time, startFade);
        Button.GetComponent<Image>().color = fadeColor;
    }



}
