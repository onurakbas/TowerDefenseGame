using UnityEngine;

public class SniperCat : Tower
{
    // === EFEKT AYARLARI ===
    [Header("Görsel Efektler")]
    [SerializeField] private GameObject lazerEfektiPrefab; // Lazer ışını görseli
    [SerializeField] private AudioClip atisSesi;           // "Pew" sesi

    protected override void Start()
    {
        // === İSTATİSTİKLER ===
        towerNameID = "Sniper-Cat v1";
        damage = 10f;        // Taban Hasar
        range = 4f;          // Uzun Menzil
        fireRate = 1f;       // Saniyede 1 Atış
        cost = 50;           // Maliyet
    }

    // Polimorfizm: AtesEt fonksiyonunu kendine özel yazıyoruz
    public override void AtesEt()
    {
        if (hedef == null) return;

        // === 1. SES EFEKTİ ===
        if (atisSesi != null)
        {
            AudioSource.PlayClipAtPoint(atisSesi, transform.position);
        }

        // === 2. GÖRSEL EFEKT (Lazer) ===
        if (lazerEfektiPrefab != null)
        {
            // Efekti kulenin merkezinde yarat
            GameObject efekt = Instantiate(lazerEfektiPrefab, transform.position, Quaternion.identity);

            // Efekti hedefe doğru döndür (2D Rotasyon Hesabı)
            Vector3 direction = hedef.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            efekt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Not: Lazer prefab'ının kendi kendine yok olması için (Destroy Timer) scripti olmalı 
            // veya Particle System ise "Stop Action: Destroy" seçili olmalı.
            Destroy(efekt, 0.2f); // Garanti olsun diye 0.2 sn sonra siliyoruz
        }

        // 1. KURAL: Zırhlı düşmana (MechaBulldog) %50 daha az vurur
        float uygulanacakHasar = damage;

        if (hedef.Armor > 0) // Eğer düşmanın zırhı varsa
        {
            uygulanacakHasar = damage * 0.5f; // Hasarı yarıya indir
            // Debug.Log("Zırhlı hedef tespit edildi! Hasar düşürüldü.");
        }

        // 2. KURAL: Matematik Motorunu çağırıp Net Hasarı hesapla
        float netHasar = MathHelper.NetHasarHesapla(uygulanacakHasar, hedef.Armor);

        // 3. Hedefe hasar ver
        hedef.HasarAl(netHasar);

        GameManager.Instance.GunlukYaz($"Kule '{towerNameID}' -> '{hedef.NameID}' hedefine atış yaptı. Net Hasar: {netHasar}");
        // İleride buraya Lazer Sesi/Efekti eklenecek
    }
}