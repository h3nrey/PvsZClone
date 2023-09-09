using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager game;
    public UnityEvent onZombiesReach;

    [Header("UI")]
    [SerializeField]
    private TMP_Text sunText;

    [SerializeField]
    private Image ZWavesDisplayFill;

    [Header("Objects")]
    [SerializeField]
    private PlayerBehaviour player;

    [SerializeField]
    private ZombiesSpawner zSpawner;

    private void Awake() {
        game = this;
    }

    private void Start() {
        onZombiesReach.AddListener(() => {
            print("zombies reached");
            Time.timeScale = 0;
        });
    }

    private void Update() {
        UpdateSunText();
        UpdateZWavesDisplay();
    }

    private void UpdateZWavesDisplay() {
        float percentage = zSpawner.wavesCurtime / zSpawner.wavesDuration;
        float percenteClamped = Mathf.Clamp01(percentage);
        ZWavesDisplayFill.fillAmount = percenteClamped;
    }

    private void UpdateSunText() {
        sunText.text = player.currentEnergy.ToString();
    }
}

public class GameTags {
    public static readonly string house = "house";
    public static readonly string sun = "sun";
    public static readonly string projectille = "projectille";
}