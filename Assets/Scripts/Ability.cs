using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability")]
public class Ability : ScriptableObject
{
    public enum attackGenre {Damage, Heal, Dot, Buff, Debuff, Hot}
    public enum attackElement {Normal, Fire, Water, Earth, Air, Lightning, Light, Dark}
    public enum attackType {Base, Physical, Magical, Special}
    public enum specialEffect{None, Speed, Slow, BonusHealth, ReduceHealth, Stun}

    public GameObject projectile;
    public float range;
    public int damage;
    public float cooldown;
    public float duration;
    public List<attackGenre> genre;
    public attackElement element;
    public attackType type;
    public specialEffect effect;
    public int nbOfUse;
    public string target;
    public LayerMask layermask;
    public bool canDo;
}
