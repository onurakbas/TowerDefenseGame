using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Oyun Ayarları")]
    [SerializeField] private int baslangicCani = 100;
    [SerializeField] private int baslangicParasi = 200;
    [Header("Bekleme Süresi")]
    public float baslamaGecikmesi = 10f; // Oyun başlayınca kaç saniye beklesin?
    
    // === UI Bağlantısı ===
    [Header("UI Bağlantıları")]
    [SerializeField] private CurrencyView currencyView; // Arkadaşın (Onur) buraya UI scriptini sürükleyecek

    // Oyundaki anlık durumlar
    public int MevcutCan { get; private set; }
    public int MevcutPara { get; private set; }

    // Yol Noktaları
    public List<Transform> yolNoktalari;

    // Log Dosyası Yolu
    private string logDosyaYolu;

    // === DALGA YAPISI ===
    [System.Serializable]
    public struct DalgaBilgisi
    {
        public string dalgaAdi;
        public Enemy dusmanTuru;
        public int adet;
        public float cikisAraligi;
    }

    [Header("Dalga Ayarları")]
    public Transform baslangicNoktasi;
    public List<DalgaBilgisi> dalgalar;
    private int mevcutDalgaIndex = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        logDosyaYolu = Path.Combine(Application.persistentDataPath, "savunma_gunlugu.txt");
        File.WriteAllText(logDosyaYolu, "=== SİMÜLASYON GÜNLÜĞÜ ===\n");
        GunlukYaz($"Simülasyon Başladı. Başlangıç Can: {baslangicCani}, Para: {baslangicParasi}");
    }

    private void Start()
    {
        MevcutCan = baslangicCani;
        MevcutPara = baslangicParasi;

        // === Başlangıçta UI'ı Güncelle ===
        if (currencyView != null)
        {
            currencyView.UpdateCurrencyUI(MevcutPara);
            currencyView.UpdateHealthUI(MevcutCan);
        }

        Debug.Log($"Oyun Başladı! Can: {MevcutCan}, Para: {MevcutPara}");
        StartCoroutine(DalgaBaslat());
    }

    // === EKONOMİ VE İNŞAAT YÖNETİMİ ===

    public void ParaEkle(int miktar)
    {
        MevcutPara += miktar;
        // UI Güncelle
        if (currencyView != null) currencyView.UpdateCurrencyUI(MevcutPara);
    }

    public bool ParaHarcama(int miktar)
    {
        if (MevcutPara >= miktar)
        {
            MevcutPara -= miktar;
            // UI Güncelle
            if (currencyView != null) currencyView.UpdateCurrencyUI(MevcutPara);

            GunlukYaz($"Harcama yapıldı: {miktar}. Kalan Para: {MevcutPara}");
            return true; // Satın alma başarılı
        }

        Debug.Log("Yetersiz Bakiye!");
        return false; // Para yetmedi
    }

    // === KULE İNŞA ETME FONKSİYONU ===
    public void KuleInsaEt(Tower kulePrefab, Vector3 konum)
    {
        // 1. Para Yetiyor mu Kontrolü
        if (ParaHarcama(kulePrefab.Cost))
        {
            // 2. Kuleyi Yarat
            Tower yeniKule = Instantiate(kulePrefab, konum, Quaternion.identity);

            // 3. Logla (Proje İsteri)
            GunlukYaz($"Kullanıcı, {konum} konumuna '{yeniKule.NameID}' inşa etti. Kalan Para: {MevcutPara}.");
        }
        else
        {
            Debug.Log("Kule inşa edilemedi: Para Yetersiz.");
        }
    }

    // === SAĞLIK YÖNETİMİ (GÜNCELLENDİ) ===

    public void HasarAl(int hasarMiktari)
    {
        MevcutCan -= hasarMiktari;

        // UI Güncelle
        if (currencyView != null) currencyView.UpdateHealthUI(MevcutCan);

        GunlukYaz($"Üs hasar aldı! (-{hasarMiktari}). Kalan Can: {MevcutCan}");

        if (MevcutCan <= 0)
        {
            OyunBitti(false);
        }
    }

    private void OyunBitti(bool kazandi)
    {
        if (kazandi)
        {
            GunlukYaz("SON: Tüm dalgalar temizlendi. OYUN KAZANILDI!");
            Debug.Log("KAZANDINIZ!");
        }
        else
        {
            GunlukYaz("SON: Üs düştü. KAYBETTİNİZ.");
            Debug.Log("KAYBETTİNİZ!");
        }
    }

    // === LOGLAMA SİSTEMİ ===
    public void GunlukYaz(string mesaj)
    {
        string zamanliMesaj = $"[{System.DateTime.Now.ToString("HH:mm:ss")}] {mesaj}\n";
        File.AppendAllText(logDosyaYolu, zamanliMesaj);
    }

    // === DALGA OLUŞTURMA MANTIĞI ===
    System.Collections.IEnumerator DalgaBaslat()
    {
        GunlukYaz($"Oyunun başlamasına {baslamaGecikmesi} saniye var. Hazırlan!");
        yield return new WaitForSeconds(baslamaGecikmesi);
        while (mevcutDalgaIndex < dalgalar.Count)
        {
            DalgaBilgisi suankiDalga = dalgalar[mevcutDalgaIndex];
            GunlukYaz($"--- {suankiDalga.dalgaAdi} Başladı! ---");

            for (int i = 0; i < suankiDalga.adet; i++)
            {
                if (suankiDalga.dusmanTuru != null)
                    DusmanYarat(suankiDalga.dusmanTuru);

                yield return new WaitForSeconds(suankiDalga.cikisAraligi);
            }

            GunlukYaz($"{suankiDalga.dalgaAdi} tamamlandı. Sonraki dalga bekleniyor...");
            yield return new WaitForSeconds(5f);

            mevcutDalgaIndex++;
        }
        OyunBitti(true);
    }

    void DusmanYarat(Enemy prefab)
    {
        if (prefab == null || baslangicNoktasi == null) return;
        Enemy yeniDusman = Instantiate(prefab, baslangicNoktasi.position, Quaternion.identity);
        GunlukYaz($"Düşman sahnede: {yeniDusman.NameID} (Can: {yeniDusman.CurrentHealth})");
    }
    private void OnDrawGizmos()
    {
        // Eğer yol noktaları yoksa çizme
        if (yolNoktalari == null || yolNoktalari.Count < 2) return;

        Gizmos.color = Color.green; // Çizgi rengi yeşil olsun

        for (int i = 0; i < yolNoktalari.Count - 1; i++)
        {
            if (yolNoktalari[i] != null && yolNoktalari[i+1] != null)
            {
                // Noktalar arasına çizgi çek
                Gizmos.DrawLine(yolNoktalari[i].position, yolNoktalari[i+1].position);
                // Noktanın yerini belli et
                Gizmos.DrawSphere(yolNoktalari[i].position, 0.2f);
            }
        }
    }
    
}