using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Ability abilityReference;
    public AbilitySystem caster;

    void Awake()
    {
        spawnPosition = transform.position;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawnPosition) > abilityReference.range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != abilityReference.target)return;
        foreach (var genre in abilityReference.genre)
        {
            if (abilityReference.genre.Contains(Ability.attackGenre.Damage) ||
                abilityReference.genre.Contains(Ability.attackGenre.Dot) ||
                abilityReference.genre.Contains(Ability.attackGenre.Debuff))
            {
                if (other.gameObject.tag == abilityReference.target)
                {
                    switch (genre)
                    {
                        case Ability.attackGenre.Damage:
                            other.GetComponent<HealthSystem>().TakeDamage(abilityReference.damage);
                            break;

                        case Ability.attackGenre.Dot:
                            caster.StartCoroutine(caster.DamageOverTime(abilityReference.damage,
                                abilityReference.duration,
                                other.gameObject.GetComponent<HealthSystem>()));
                            break;

                        case Ability.attackGenre.Debuff:
                            other.GetComponent<HealthSystem>().DoDebuff(abilityReference);
                            break;
                    }
                }
            }
            else
            {
                if (other.gameObject.tag == abilityReference.target)
                {
                    switch (genre)
                    {
                        case Ability.attackGenre.Hot:
                            caster.StartCoroutine(caster.HealingOverTime(abilityReference.damage,
                                abilityReference.duration,
                                other.gameObject.GetComponent<HealthSystem>()));
                            break;

                        case Ability.attackGenre.Buff:
                            caster.GetComponent<HealthSystem>().DoBuff(abilityReference);
                            break;

                        case Ability.attackGenre.Heal:
                            caster.GetComponent<HealthSystem>().Heal(abilityReference.damage);
                            break;
                    }
                }
            }
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(timeToLive());
    }

    IEnumerator timeToLive()
    {
        yield return new WaitForSeconds(abilityReference.duration);
        Destroy(gameObject);
    }
}
