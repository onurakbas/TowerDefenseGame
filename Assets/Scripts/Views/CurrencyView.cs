using UnityEngine;
using UnityEngine.UI; // Standart UI Text için
// using TMPro; // Eğer TextMeshPro kullanıyorsan bunu aç

public class CurrencyView : MonoBehaviour
{
    [Header("UI Metinleri")]
    // Arkadaşın Unity'de Text objelerini buralara sürükleyecek
    [SerializeField] private Text healthText;    // Can Yazısı
    [SerializeField] private Text currencyText;  // Para Yazısı
    [SerializeField] private Text waveText;      // Dalga Yazısı (Opsiyonel)

    // GameManager bu fonksiyonu çağıracak
    public void UpdateHealthUI(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "MAINFRAME: " + currentHealth.ToString() + "%";
            // Renk değişimi efekti (Can azalınca kızarsın)
            if (currentHealth < 30) healthText.color = Color.red;
            else healthText.color = Color.green;
        }
    }

    // GameManager bu fonksiyonu çağıracak
    public void UpdateCurrencyUI(int currentCurrency)
    {
        // AJAN LOG: Bakalım bu fonksiyon hiç çağrılıyor mu?
        Debug.Log("Tabelacıya Emir Geldi! Yeni Para: " + currentCurrency);

        if (currencyText != null)
        {
            currencyText.text = "CRYPTO: " + currentCurrency.ToString();
        }
        else
        {
            Debug.LogError("HATA: Tabelacı kalemi (ParaText) bulamıyor!");
        }
    }
}