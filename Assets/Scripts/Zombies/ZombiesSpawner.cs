using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using NaughtyAttributes;

public class ZombiesSpawner : MonoBehaviour
{
    [Header("Static stuff")]
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeReference]
    private float startSpawnCooldown;   

    [Header("Waves Manager")]
    [SerializeField]
    private ZombieWave[] waves;
    [SerializeField]
    private List<GameObject> zombiesPool;
    [SerializeField]
    [ReadOnly] int currentWave;
    [SerializeField]
    float waveCooldown; // the cooldown between waves  

    [Header("Wave")]
    [SerializeField]
    private float spawnCooldown;

    [SerializeField] 
    [ReadOnly] int waveZombiesCreated; // all the zombies that was created in the current wave

    [SerializeField]
    [ReadOnly] List<GameObject> activeZombies;



    private void Start() {
        InstantiateZombies();
        StartCoroutine(HandleWave());
    }

    IEnumerator HandleWave() {
        yield return new WaitForSeconds(startSpawnCooldown);

        foreach (ZombieWave wave in waves) {
            bool waveFinished = false;

            while (!waveFinished) {        
                if(zombiesPool.Count > 0 && waveZombiesCreated < wave.zombiesPool.Length) {
                    GameObject curZombie = zombiesPool[0];
                    curZombie.SetActive(true);
                    zombiesPool.Remove(curZombie);
                    activeZombies.Add(curZombie);
                    waveZombiesCreated++;
                }

                if (waveZombiesCreated > 0 && activeZombies.Count == 0) waveFinished = true;
                yield return new WaitForSeconds(spawnCooldown);
            }
            waveZombiesCreated = 0;
            activeZombies.Clear();
            currentWave += 1;
            yield return new WaitForSeconds(waveCooldown);
        }

        print("zombies ended");
        yield return null;
    }

    private void Update() {
       
    }

    private void InstantiateZombies() {
        foreach (ZombieWave wave in waves) {
            foreach (ZombieGroup group in wave.zombiesPool) {
                GameObject zombie = CreateZombie(group);
                zombie.SetActive(false);
                zombiesPool.Add(zombie);
            }
        }   
    }
    private GameObject CreateZombie(ZombieGroup group) {
        Transform point = spawnPoints[(int)group.spawnPoint];
        GameObject zombie = Instantiate(zombiePrefab, point.position, point.rotation, point);
        zombie.GetComponent<zombieBehaviour>().LoadData(group.zombieData);
        zombie.GetComponent<CreatureBehaviour>().GetData(group.zombieData);
        zombie.GetComponent<CreatureBehaviour>().onDie.AddListener(() => activeZombies.Remove(zombie));
        return zombie;
    }

    //private Transform GetRandomLane() {
    //    int randomIndex = Random.Range(0, lanePoints.Length);
    //    return lanePoints[randomIndex];
    //}
}
