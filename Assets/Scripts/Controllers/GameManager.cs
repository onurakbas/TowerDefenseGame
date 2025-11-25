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
}