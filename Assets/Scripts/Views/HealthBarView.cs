using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    [Header("Bileşenler")]
    [SerializeField] private Transform yesilBar; // Slider yerine Transform kullanıyoruz
    [SerializeField] private SpriteRenderer enemyRenderer;

    private Color originalColor;
    private Vector3 originalScale;

    private void Awake()
    {
        if (enemyRenderer == null) enemyRenderer = GetComponent<SpriteRenderer>();
        if (enemyRenderer != null) originalColor = enemyRenderer.color;
        
        // Başlangıç boyutunu hafızaya al
        if (yesilBar != null) originalScale = yesilBar.localScale;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (yesilBar != null)
        {
            // Yüzdeyi hesapla (0 ile 1 arası)
            float oran = currentHealth / maxHealth;
            // X eksenini (Genişliği) orana göre küçült
            yesilBar.localScale = new Vector3(originalScale.x * oran, originalScale.y, originalScale.z);
        }
    }

    public void SetSlowEffect(bool isSlowed)
    {
        if (enemyRenderer != null)
            enemyRenderer.color = isSlowed ? Color.cyan : originalColor;
    }
}