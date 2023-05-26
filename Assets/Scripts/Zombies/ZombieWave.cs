using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class ZombieWave
{
    public ZombieGroup[] zombiesPool;
}

[System.Serializable]
public class ZombieGroup {
    public enum spawnPoints {
        first,
        second,
        third,
        forth,
        fifth,
    }

    [EnumFlags]
    public spawnPoints spawnPoint;
    public float nextCooldown;
    public Zombie zombieData;
}
