using UnityEngine;

public class SniperCat : Tower
{
    // === EFEKT AYARLARI ===
    [Header("Görsel Efektler")]
    [SerializeField] private GameObject lazerEfektiPrefab; // Lazer görseli
    [SerializeField] private AudioClip atisSesi;           // "Pew" sesi

    // ▼▼▼ YENİ EKLENEN KISIM BURASI ▼▼▼
    [SerializeField] private Transform namluUcu;           // FirePoint buraya gelecek
    // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    protected override void Start()
    {
        // === İSTATİSTİKLER (Inspector'dan ezilebilir) ===
        towerNameID = "Sniper-Cat v1";
        
        // Eğer Inspector'da 0 girildiyse varsayılanları ata
        if (damage == 0) damage = 20f;
        if (range == 0) range = 4f;
        if (fireRate == 0) fireRate = 1f;
        if (cost == 0) cost = 50;
    }

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
            // Namlu ucunu veya merkezi belirle
            Vector3 cikisNoktasi = (namluUcu != null) ? namluUcu.position : transform.position;

            // Aradaki mesafeyi ölç (Lazerin boyu bu kadar olacak)
            float mesafe = Vector3.Distance(cikisNoktasi, hedef.transform.position);

            // Lazeri yarat
            GameObject efekt = Instantiate(lazerEfektiPrefab, cikisNoktasi, Quaternion.identity);

            // Yönü ayarla (Düşmana bak)
            Vector3 direction = hedef.transform.position - cikisNoktasi;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            efekt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // --- YENİ EKLENEN KISIM: DİNAMİK BOYUTLANDIRMA ---
            // Lazerin boyunu (X eksenini) mesafeye eşitle. 
            // Y (Kalınlık) sabit kalsın (örn: 0.15f).
            efekt.transform.localScale = new Vector3(mesafe, 0.15f, 1f);
            // ------------------------------------------------

            // Sadece görsel olduğu için çarpışma özelliğini kapatıyoruz (Garanti olsun)
            if(efekt.GetComponent<Collider2D>()) Destroy(efekt.GetComponent<Collider2D>());

            Destroy(efekt, 0.1f); // Çok kısa süre ekranda kalsın (Göz kırpma gibi)
        }

        // === 3. HASAR HESABI ===
        
        // KURAL: Zırhlı düşmana %50 az vur
        float uygulanacakHasar = damage;
        if (hedef.Armor > 0)
        {
            uygulanacakHasar = damage * 0.5f;
        }

        // Net hasar hesabı ve uygulama
        float netHasar = MathHelper.NetHasarHesapla(uygulanacakHasar, hedef.Armor);
        hedef.HasarAl(netHasar);

        GameManager.Instance.GunlukYaz($"Kule '{towerNameID}' -> '{hedef.NameID}' hedefine atış yaptı. Net Hasar: {netHasar}");
    }
}