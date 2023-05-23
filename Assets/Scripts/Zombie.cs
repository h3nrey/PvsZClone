using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "creature/new zombie")]
public class Zombie : Creature {
    public int damage;
    public float speed;

    public bool hasShield;
    public Sprite shieldSprite;
    public int shieldLife;

}
