using UnityEngine;
using System.IO; // Dosya işlemleri için kütüphane
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Her yerden erişim için (Singleton)

    [Header("Oyun Ayarları")]
    [SerializeField] private int baslangicCani = 100;
    [SerializeField] private int baslangicParasi = 200;

    // Oyundaki anlık durumlar
    public int MevcutCan { get; private set; }
    public int MevcutPara { get; private set; }

    // Yol Noktaları (Düşmanlar buraya bakıp yürüyecek)
    // Arkadaşın (Onur) sahnedeki noktaları buraya sürükleyecek.
    public List<Transform> yolNoktalari;

    // Log Dosyası Yolu
    private string logDosyaYolu;

    private void Awake()
    {
        // Singleton Ayarı
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişse de silinme
        }
        else
        {
            Destroy(gameObject);
        }

        // Log dosyasını bilgisayarın güvenli bir klasörüne ayarla
        // Örn: C:/Users/Kullanici/AppData/LocalLow/Sirket/Oyun/savunma_gunlugu.txt
        logDosyaYolu = Path.Combine(Application.persistentDataPath, "savunma_gunlugu.txt");

        // Yeni oyun başlayınca eski logu temizle ve başlığı at
        File.WriteAllText(logDosyaYolu, "=== SİMÜLASYON GÜNLÜĞÜ ===\n");
        GunlukYaz($"Simülasyon Başladı. Başlangıç Can: {baslangicCani}, Para: {baslangicParasi}");
    }

    private void Start()
    {
        MevcutCan = baslangicCani;
        MevcutPara = baslangicParasi;

        // UI güncellemesi için View katmanına haber ver (Şimdilik sadece logluyoruz)
        Debug.Log($"Oyun Başladı! Can: {MevcutCan}, Para: {MevcutPara}");
    }

    // === EKONOMİ YÖNETİMİ ===

    public void ParaEkle(int miktar)
    {
        MevcutPara += miktar;
        // İleride CurrencyView.Guncelle(MevcutPara) çağıracağız
    }

    public bool ParaHarcama(int miktar)
    {
        if (MevcutPara >= miktar)
        {
            MevcutPara -= miktar;
            GunlukYaz($"Harcama yapıldı: {miktar}. Kalan Para: {MevcutPara}");
            return true; // Satın alma başarılı
        }

        Debug.Log("Yetersiz Bakiye!");
        return false; // Para yetmedi
    }

    // === SAĞLIK YÖNETİMİ ===

    public void HasarAl(int hasarMiktari)
    {
        MevcutCan -= hasarMiktari;
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
            // Time.timeScale = 0; // Oyunu durdur
        }
    }

    // === LOGLAMA SİSTEMİ (Önemli İster) ===

    public void GunlukYaz(string mesaj)
    {
        // Mesaja zaman damgası ekle: [14:30:05] Mesaj
        string zamanliMesaj = $"[{System.DateTime.Now.ToString("HH:mm:ss")}] {mesaj}\n";

        // Dosyanın sonuna ekle (Append)
        File.AppendAllText(logDosyaYolu, zamanliMesaj);

        // Konsolda da görelim
        // Debug.Log("<color=green>LOG:</color> " + mesaj);
    }

    // === DALGA YAPISI (Inspector'da ayarlanabilsin diye Serializable yapıyoruz) ===
    [System.Serializable]
    public struct DalgaBilgisi
    {
        public string dalgaAdi;         // Örn: "Dalga 1 - Giriş"
        public Enemy dusmanTuru;        // Hangi düşman çıkacak? (Prefab)
        public int adet;                // Kaç tane?
        public float cikisAraligi;      // Kaç saniyede bir çıksın?
    }

    [Header("Dalga Ayarları")]
    public Transform baslangicNoktasi;  // Düşmanların doğacağı yer (Start Point)
    public List<DalgaBilgisi> dalgalar; // Dalga listesi (Editörden doldurulacak)

    // Dalga Kontrolü
    private int mevcutDalgaIndex = 0;

    // Start fonksiyonunu güncelle: Oyun başlayınca dalgayı başlat!
    private void Start()
    {
        MevcutCan = baslangicCani;
        MevcutPara = baslangicParasi;

        Debug.Log($"Oyun Başladı! Can: {MevcutCan}, Para: {MevcutPara}");

        // İlk dalgayı başlat (Biraz gecikmeli başlasın ki hazırlanalım)
        StartCoroutine(DalgaBaslat());
    }

    // === DALGA OLUŞTURMA MANTIĞI (Coroutine) ===
    System.Collections.IEnumerator DalgaBaslat()
    {
        // Tüm dalgalar bitene kadar dön
        while (mevcutDalgaIndex < dalgalar.Count)
        {
            DalgaBilgisi suankiDalga = dalgalar[mevcutDalgaIndex];
            GunlukYaz($"--- {suankiDalga.dalgaAdi} Başladı! ---");

            // O dalgadaki düşman sayısı kadar dön
            for (int i = 0; i < suankiDalga.adet; i++)
            {
                DusmanYarat(suankiDalga.dusmanTuru);

                // Bir sonraki düşman için bekle (Örn: 1 saniye)
                yield return new WaitForSeconds(suankiDalga.cikisAraligi);
            }

            // Dalga bitti, bir sonrakine geçmeden önce biraz bekle (Örn: 5 saniye dinlenme)
            GunlukYaz($"{suankiDalga.dalgaAdi} tamamlandı. Sonraki dalga bekleniyor...");
            yield return new WaitForSeconds(5f);

            mevcutDalgaIndex++; // Sonraki dalgaya geç
        }

        // Döngü bittiyse tüm dalgalar bitmiştir
        OyunBitti(true); // KAZANDINIZ!
    }

    void DusmanYarat(Enemy prefab)
    {
        if (prefab == null || baslangicNoktasi == null) return;

        // Düşmanı sahnede oluştur (Instantiate)
        Enemy yeniDusman = Instantiate(prefab, baslangicNoktasi.position, Quaternion.identity);

        GunlukYaz($"Düşman sahnede: {yeniDusman.NameID} (Can: {yeniDusman.CurrentHealth})");
    }
}