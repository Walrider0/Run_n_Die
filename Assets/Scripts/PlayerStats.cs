using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    
    private float currentHealth;
    
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        
        healthBar.SetSliderMax(maxHealth);
    }

    private void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20f);
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        healthBar.SetSlider(currentHealth);
    }

    private void Die()
    {
        Debug.Log("You Died !");
    }
}
