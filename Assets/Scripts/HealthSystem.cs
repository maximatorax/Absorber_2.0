using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healingAmount)
    {
        currentHealth += healingAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        if (gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<EnemyController>().death.Invoke();
        }
        Destroy(gameObject);
    }

    public void DoBuff(Ability ability)
    {

    }

    public void DoDebuff(Ability ability)
    {

    }
}
