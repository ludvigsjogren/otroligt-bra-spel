using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalHealthColor, criticalHealthColor;
    private int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateHealthbar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHealthbar()
    {
        healthSlider.value = currentHealth;

        if(currentHealth <= 2)
        {
            fillImage.color = criticalHealthColor;
        }
        else
        {
            fillImage.color = normalHealthColor;
        }

    }
    

}
