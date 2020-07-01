using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    
    public static Restart Instance;

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
    public void Quit()
    {
       LoadScene("StartScene");
    }

    public void Reset()
    {
        LoadScene("GameScene");
    }

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }



}
