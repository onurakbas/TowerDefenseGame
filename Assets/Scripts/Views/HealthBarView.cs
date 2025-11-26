using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [Header("Bileşenler")]
    [SerializeField] private Slider healthSlider; // Can Barı (Slider)
    [SerializeField] private SpriteRenderer enemyRenderer; // Düşmanın resmi (Renk değişimi için)

    private Color originalColor; // Düşmanın orijinal rengini sakla

    private void Awake()
    {
        if (enemyRenderer == null)
            enemyRenderer = GetComponent<SpriteRenderer>();

        if (enemyRenderer != null)
            originalColor = enemyRenderer.color;
    }

    public void InitHealth(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            // Can %0 ise barı gizle (Patlama efekti oynarken bar görünmesin)
            if (currentHealth <= 0) healthSlider.gameObject.SetActive(false);
        }
    }

    // HackerCat (Buz Kulesi) bu fonksiyonu çağırıp düşmanı mavi yapacak
    public void SetSlowEffect(bool isSlowed)
    {
        if (enemyRenderer != null)
        {
            if (isSlowed)
                enemyRenderer.color = Color.cyan; // Mavi tonu
            else
                enemyRenderer.color = originalColor; // Eski rengine dön
        }
    }
}