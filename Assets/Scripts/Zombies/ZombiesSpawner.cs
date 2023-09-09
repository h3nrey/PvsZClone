using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using NaughtyAttributes;

public class ZombiesSpawner : MonoBehaviour {

    [Header("Static stuff")]
    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeReference]
    private float startSpawnCooldown;

    [Header("Waves Manager")]
    [SerializeField]
    [OnValueChanged(nameof(SetWavesDuration))]
    private ZombieWave[] waves;

    [SerializeField]
    private List<GameObject> zombiesPool;

    [SerializeField]
    [ReadOnly]
    private List<float> spawnCooldownPool;

    [SerializeField]
    [ReadOnly]
    public float wavesDuration;

    [SerializeField]
    [ReadOnly]
    public float wavesCurtime;

    [SerializeField]
    [ReadOnly] private int currentWaveIndex;

    private ZombieWave currentWave;

    [SerializeField]
    private float waveCooldown; // the cooldown between waves

    [Header("Wave")]
    [SerializeField]
    [ReadOnly] private int waveZombiesCreated; // all the zombies that was created in the current wave

    [SerializeField]
    [ReadOnly] private List<GameObject> activeZombies;

    [ReadOnly] public float currentSpawnCooldown;
    [ReadOnly] public bool canSpawn;
    [ReadOnly] public float realTime;

    private void SetWavesDuration() {
        wavesDuration = 0;
        foreach (ZombieWave wave in waves) {
            foreach (ZombieGroup group in wave.zombiesPool) {
                wavesDuration += 1;
            }
        }
    }

    private void Start() {
        InstantiateZombies();
        canSpawn = true;
    }

    private void Update() {
        realTime = Time.time;
        EnableZombie();
        CheckZombiesWaveState();
    }

    private void EnableZombie() {
        if (canSpawn && currentSpawnCooldown < Time.time && currentWaveIndex < waves.Length) {
            currentWave = waves[currentWaveIndex];
            GameObject curZombie = zombiesPool[0];
            curZombie.SetActive(true);
            zombiesPool.Remove(curZombie);
            activeZombies.Add(curZombie);
            waveZombiesCreated++;

            float cooldown = spawnCooldownPool[0];
            spawnCooldownPool.RemoveAt(0);
            currentSpawnCooldown = cooldown + Time.time;

            if (waveZombiesCreated >= currentWave.zombiesPool.Length) {
                canSpawn = false;
            }
        }
    }

    private void CheckZombiesWaveState() {
        if (activeZombies.Count > 0) return;

        if (waveZombiesCreated > 0 && currentWaveIndex < waves.Length) {
            waveZombiesCreated = 0;
            canSpawn = true;
            currentSpawnCooldown = Time.time + currentWave.nextWaveCooldown;
            currentWaveIndex += 1;
        }
    }

    //Put in the scene and disable
    private void InstantiateZombies() {
        foreach (ZombieWave wave in waves) {
            foreach (ZombieGroup group in wave.zombiesPool) {
                GameObject zombie = CreateZombie(group);
                zombie.SetActive(false);
                zombiesPool.Add(zombie);
            }
        }
    }

    // The fucntion that actually instantiate the zombies
    private GameObject CreateZombie(ZombieGroup group) {
        Transform point = spawnPoints[(int)group.spawnPoint];
        GameObject zombie = Instantiate(zombiePrefab, point.position, point.rotation, point);
        zombie.GetComponent<zombieBehaviour>().LoadData(group.zombieData);
        zombie.GetComponent<CreatureBehaviour>().GetData(group.zombieData);
        zombie.GetComponent<CreatureBehaviour>().onDie.AddListener(() => activeZombies.Remove(zombie));
        spawnCooldownPool.Add(group.nextCooldown);
        return zombie;
    }
}