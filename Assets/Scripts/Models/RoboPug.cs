using UnityEngine;

// Enemy sınıfından MİRAS alıyoruz
public class RoboPug : Enemy
{
    // Başlangıç ayarlarını burada yapıyoruz
    protected override void Start()
    {
        // === KONSEPT AYARLARI (Standart) ===
        enemyNameID = "Robo-Pug v1";
        maxHealth = 50f; // Standart Can 
        armor = 0f; // Zırhsız 
        speed = 50f; // Normal Hız 
        reward = 10; // Ödül: 10 Para 
        baseDamage = 5; // Üsse vuracağı hasar

        base.Start(); // Canı fullemek için ana sınıfın Start'ını çağırıyoruz
    }

    // === SOYUT METOTLARI DOLDURMA (Override) ===

    public override void HasarAl(float miktar)
    {
        currentHealth -= miktar;

        // Buraya ilerde "Havlama Sesi" veya "Kıvılcım Efekti" ekleyebiliriz

        if (currentHealth <= 0)
        {
            Ol();
        }
    }

    public override void Ol()
    {
        if (GameManager.Instance != null)
         {
             GameManager.Instance.ParaEkle(reward);
         }
        Debug.Log($"{enemyNameID} hurdaya döndü! Ödül: {reward}");
        Destroy(gameObject); // Sahneden sil
    }

    public override void UsseSaldir()
    {
        if (GameManager.Instance != null) 
         {
           GameManager.Instance.HasarAl(baseDamage);
         }
        Debug.Log($"{enemyNameID} ana sunucuyu ısırdı! Hasar: {baseDamage}");
        Destroy(gameObject);
    }
}