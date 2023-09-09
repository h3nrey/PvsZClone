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
    private Slider ZWavesDisplayFill;

    [Header("Objects")]
    [SerializeField]
    private PlayerBehaviour player;

    [SerializeField]
    private ZombiesSpawner zSpawner;

    [Header("Zombies Progress Bar")]
    [SerializeField] private RectTransform progressBarObj;

    [SerializeField]
    private float currZombieProgress;

    [SerializeField]
    private float smoothSpeed;

    [SerializeField] private GameObject flagPrefab;

    private void Awake() {
        game = this;
    }

    private void Start() {
        onZombiesReach.AddListener(() => {
            print("zombies reached");
            Time.timeScale = 0;
        });

        SetFlagsPos();
    }

    private void Update() {
        UpdateSunText();
        UpdateZWavesDisplay();
        UpdateZombiePointer();
    }

    private void UpdateZWavesDisplay() {
        float total = zSpawner.totalOfZombiesPerPhase;
        float some = zSpawner.totalOfZombiesCreated;

        if (zSpawner.totalOfZombiesCreated > 0) {
            float percentage = some / total;
            float percenteClamped = Mathf.Clamp01(percentage);
            currZombieProgress = percenteClamped;
        }
    }

    private void UpdateZombiePointer() {
        float curValue = ZWavesDisplayFill.value;
        ZWavesDisplayFill.value = Mathf.Lerp(curValue, currZombieProgress, smoothSpeed);
    }

    private void SetFlagsPos() {
        float total = zSpawner.totalOfZombiesPerPhase;
        float zombiesAccumulated = 0;
        float zombiesBarOffset = -200;
        float yPos = -2.3f;

        for (int i = 0; i < zSpawner.waves.Length - 1; i++) {
            float waveLength = zSpawner.waves[i].zombiesPool.Length;
            float zombiesQtd = waveLength + zombiesAccumulated;
            zombiesAccumulated += waveLength;
            float percentage = waveLength / total;

            float x = percentage * zombiesBarOffset;
            Vector2 pos = new Vector2(x - 1, yPos);
            GameObject flag = Instantiate(flagPrefab, progressBarObj);
            flag.GetComponent<RectTransform>().anchoredPosition = pos;
            flag.transform.SetSiblingIndex(progressBarObj.childCount - 2);
        }
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