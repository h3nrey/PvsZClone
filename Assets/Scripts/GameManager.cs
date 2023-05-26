using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public UnityEvent onZombiesReach;

    private void Awake() {
        game = this;
    }

    private void Start() {
        onZombiesReach.AddListener(() => {
            print("zombies reached");
            Time.timeScale = 0;
        });
    }
}

public class GameTags {
    public static readonly string house = "house";
    public static readonly string sun = "sun";
    public static readonly string projectille = "projectille";
}

