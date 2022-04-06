using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class AbilitySystem : MonoBehaviour
{
    public List<Ability> abilities;

    [SerializeField] private Transform cam;
    [SerializeField] private RectTransform crosshair;

    public Ability selectedAbility;
    private int selectedAbilityIndex;
    public int maxNbOfAbilities;

    private Event keyPressed;

    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };

    // Start is called before the first frame update
    void Start()
    {
        foreach (var ability in abilities)
        {
            ability.canDo = true;
        }

        selectedAbilityIndex = 0;
        selectedAbility = abilities[selectedAbilityIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            selectedAbilityIndex++;
            if (selectedAbilityIndex >= abilities.Count)
            {
                selectedAbilityIndex = abilities.Count - 1;
            }
            if (selectedAbilityIndex > maxNbOfAbilities)
            {
                selectedAbilityIndex = maxNbOfAbilities;
            }
            selectedAbility = abilities[selectedAbilityIndex];
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            selectedAbilityIndex--;
            if (selectedAbilityIndex < 0)
            {
                selectedAbilityIndex = 0;
            }
            selectedAbility = abilities[selectedAbilityIndex];
        }

        /*try
        {
            if (keyPressed.numeric)
            {
                for (int i = 0; i < keyCodes.Length; i++)
                {
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        selectedAbilityIndex = i;
                        selectedAbility = abilities[selectedAbilityIndex];
                    }
                }
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }*/
    }

    public void UseAbility(Ability abilityToUse)
    {
        if(abilityToUse.canDo == false) return;

        abilityToUse.canDo = false;

        if (abilityToUse.type != Ability.attackType.Base)
        {
            abilityToUse.nbOfTimeUsed++;

            if (abilityToUse.nbOfTimeUsed >= abilityToUse.nbOfUse)
            {
                abilityToUse.nbOfTimeUsed = 0;
                RemoveAbility(abilityToUse);
            }
        }

        StartCoroutine(doAbility(abilityToUse));
    }

    IEnumerator doAbility(Ability abilityToUse)
    {
        if (abilityToUse.projectile != null)
        {
            RaycastHit hit;
            GameObject go = Instantiate(abilityToUse.projectile,
                transform.position + new Vector3(0f, 0f, abilityToUse.projectile.transform.localScale.z / 2),
                Quaternion.identity);
            go.GetComponent<Projectile>().caster = this;

            if (crosshair.gameObject.activeInHierarchy)
            {
                Vector2 crosshairPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(crosshairPosition), cam.forward,
                        out hit, abilityToUse.range, abilityToUse.layermask))
                {
                    go.GetComponent<Rigidbody>().AddForce((hit.point - transform.position).normalized * go.GetComponent<Projectile>().speed); 
                }
                else
                {
                    go.GetComponent<Rigidbody>().AddForce(cam.forward * go.GetComponent<Projectile>().speed);
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward,
                        out hit, abilityToUse.range, abilityToUse.layermask))
                {
                    go.GetComponent<Rigidbody>().AddForce((hit.point - transform.position).normalized * go.GetComponent<Projectile>().speed);
                }
                else
                {
                    go.GetComponent<Rigidbody>().AddForce(transform.forward * go.GetComponent<Projectile>().speed);
                }
            }
        }
        else
        {
            foreach (var genre in abilityToUse.genre)
            {
                if (abilityToUse.genre.Contains(Ability.attackGenre.Damage) ||
                    abilityToUse.genre.Contains(Ability.attackGenre.Dot) ||
                    abilityToUse.genre.Contains(Ability.attackGenre.Debuff))
                {
                    RaycastHit hit;
                    if (crosshair.gameObject.activeInHierarchy)
                    {
                        Vector2 crosshairPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

                        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(crosshairPosition), cam.forward,
                                out hit, abilityToUse.range, abilityToUse.layermask))
                        {
                            if (hit.transform.gameObject.tag == abilityToUse.target)
                            {
                                switch (genre)
                                {
                                    case Ability.attackGenre.Damage:
                                        DoDamage(abilityToUse.damage,
                                            hit.transform.gameObject.GetComponent<HealthSystem>());
                                        break;

                                    case Ability.attackGenre.Dot:
                                        StartCoroutine(DamageOverTime(abilityToUse.damage, abilityToUse.duration,
                                            hit.transform.gameObject.GetComponent<HealthSystem>()));
                                        break;

                                    case Ability.attackGenre.Debuff:
                                        hit.transform.GetComponent<HealthSystem>().DoDebuff(abilityToUse);
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position, transform.forward,
                                out hit, abilityToUse.range, abilityToUse.layermask))
                        {
                            if (hit.transform.gameObject.tag == abilityToUse.target)
                            {
                                switch (genre)
                                {
                                    case Ability.attackGenre.Damage:
                                        DoDamage(abilityToUse.damage,
                                            hit.transform.gameObject.GetComponent<HealthSystem>());
                                        break;

                                    case Ability.attackGenre.Dot:
                                        StartCoroutine(DamageOverTime(abilityToUse.damage, abilityToUse.duration,
                                            hit.transform.gameObject.GetComponent<HealthSystem>()));
                                        break;

                                    case Ability.attackGenre.Debuff:
                                        hit.transform.GetComponent<HealthSystem>().DoDebuff(abilityToUse);
                                        break;
                                }
                            }
                        }
                    }
                }

                if (abilityToUse.genre.Contains(Ability.attackGenre.Heal) ||
                    abilityToUse.genre.Contains(Ability.attackGenre.Buff) ||
                    abilityToUse.genre.Contains(Ability.attackGenre.Hot))
                {
                    switch (genre)
                    {
                        case Ability.attackGenre.Hot:
                            StartCoroutine(HealingOverTime(abilityToUse.damage, abilityToUse.duration,
                                gameObject.GetComponent<HealthSystem>()));
                            break;

                        case Ability.attackGenre.Buff:
                            gameObject.GetComponent<HealthSystem>().DoBuff(abilityToUse);
                            break;
                        case Ability.attackGenre.Heal:
                            gameObject.GetComponent<HealthSystem>().Heal(abilityToUse.damage);
                            break;
                    }
                }
            }
        }

        yield return new WaitForSeconds(abilityToUse.cooldown);

        abilityToUse.canDo = true;
    }

    public void AddAbility(Ability abilityToAdd)
    {
        if (abilities.Contains(abilityToAdd))
        {
            abilities[abilities.IndexOf(abilityToAdd)].nbOfTimeUsed -= abilityToAdd.nbOfUse;
        }
        else if(abilities.Count < maxNbOfAbilities)
        {
            abilities.Add(abilityToAdd);
            abilityToAdd.canDo = true;
        }
        else
        {
            ReplaceAbility();
        }
    }

    public void RemoveAbility(Ability abilityToRemove)
    {
        selectedAbilityIndex = abilities.IndexOf(abilityToRemove) - 1;
        selectedAbility = abilities[selectedAbilityIndex];
        abilities.Remove(abilityToRemove);
    }

    void DoDamage(int damage, HealthSystem target)
    {
        target.TakeDamage(damage);
    }

    public IEnumerator DamageOverTime(int amount, float time, HealthSystem target)
    {
        for (int x = 0; x < time; x++)
        {
            yield return new WaitForSeconds(1f);
            DoDamage(amount, target);
        }
        
    }

    public IEnumerator HealingOverTime(int amount, float time, HealthSystem target)
    {
        for (int x = 0; x < time; x++)
        {
            yield return new WaitForSeconds(1f);
            target.Heal(amount);
        }

    }

    void ReplaceAbility()
    {
        //Write replace ability menu code
    }
}
