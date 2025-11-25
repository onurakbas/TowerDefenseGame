using UnityEngine;

// Enemy sınıfından MİRAS alıyoruz
public class RoboPug : Enemy
{
    // Başlangıç ayarlarını burada yapıyoruz
    protected override void Start()
    {
        // === KONSEPT AYARLARI (Standart) ===
        enemyNameID = "Robo-Pug v1";
        maxHealth = 50f; [cite_start]// Standart Can [cite: 51]
        armor = 0f; [cite_start]// Zırhsız [cite: 51]
        speed = 5f; [cite_start]// Normal Hız [cite: 51]
        reward = 10; [cite_start]// Ödül: 10 Para [cite: 52]
        baseDamage = 5; [cite_start]// Üsse vuracağı hasar [cite: 52]

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
        // GameManager.ParaEkle(reward) komutu buraya gelecek (4. Gün)
        Debug.Log($"{enemyNameID} hurdaya döndü! Ödül: {reward}");
        Destroy(gameObject); // Sahneden sil
    }

    public override void UsseSaldir()
    {
        // GameManager.CanAzalt(baseDamage) komutu buraya gelecek
        Debug.Log($"{enemyNameID} ana sunucuyu ısırdı! Hasar: {baseDamage}");
        Destroy(gameObject);
    }
}