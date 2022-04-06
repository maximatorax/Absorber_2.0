using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public Ability abilityToGive;

    [SerializeField] private AbilitySystem playerAbilities;

    public UnityEvent death = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        death.AddListener(GiveAbility);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GiveAbility()
    {
        playerAbilities.AddAbility(abilityToGive);
    }
}
