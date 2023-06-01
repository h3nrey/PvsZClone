using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "creature/new plant")]
public class Plant : Creature
{
    public int sunCost;
    public float plantCooldown;
    public bool isShootable;
    public Projectille projectileData;
    public float shootCooldown;
    public float shootRange;

    public GameObject projectille;

}
