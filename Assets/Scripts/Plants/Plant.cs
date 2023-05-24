using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "creature/new plant")]
public class Plant : Creature
{
    public int sunCost;
    public Projectille projectileData;
    public float shootCooldown;

    public GameObject projectille;

}
