using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "creature/base creature")]
public class Creature : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public RuntimeAnimatorController animController;
    public int life;
    public bool canTakeDamage;
}
