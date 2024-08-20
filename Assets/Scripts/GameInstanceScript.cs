using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstanceScript : MonoBehaviour
{
    [Header("Sound")]
    public float SFXVolume = 1.0f;
    public float musicVolume = 1.0f;

    [Header("Audio Source")]
    public AudioSource bgm;
    public AudioClip Jump;
    public AudioClip SplitSelect;
    public AudioClip Split;

    private static GameInstanceScript instance = null;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if(GameInstanceScript.instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
