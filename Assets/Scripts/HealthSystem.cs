using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform faceTowardTarget;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth;
        if (gameObject.tag == "Enemy")
        {
            healthBar.transform.LookAt(faceTowardTarget);
        }
    }

    public void CalculateHealthBarColor()
    {
        healthBar.targetGraphic.color = Color.Lerp(Color.red, Color.green, healthBar.value / healthBar.maxValue);
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
