using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }
}
