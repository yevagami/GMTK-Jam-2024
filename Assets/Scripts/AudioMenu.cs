using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    GameInstanceScript gameInstance;


    private void Awake() {
        gameInstance = GameObject.FindGameObjectWithTag("gameInstance").GetComponent<GameInstanceScript>();
    }

    private void Update() {
        if(gameInstance == null) {
            Debug.Log("Game Instance not found");
            return;
        }
        gameInstance.musicVolume = musicSlider.value;
        gameInstance.SFXVolume = sfxSlider.value;
    }
}
