
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Sprite icon;
    public abstract void Attack(GameObject attacker);
}
