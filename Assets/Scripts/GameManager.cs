using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public UnityEvent onZombiesReach;

    [Header("UI")]
    [SerializeField]
    TMP_Text sunText;

    [Header("Objects")]
    [SerializeField]
    PlayerBehaviour player;

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

