using UnityEngine;
using System.Collections; // IEnumerator için gerekli
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    // === DEĞİŞKENLER ===
    [Header("Temel Özellikler")]
    [SerializeField] protected string enemyNameID;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float armor;
    [SerializeField] protected float speed;
    [SerializeField] protected int reward;
    [SerializeField] protected int baseDamage;

    protected float currentHealth;

    // EKSİK OLAN DEĞİŞKEN EKLENDİ:
    protected int currentWaypointIndex = 0;

    // === YAVAŞLATMA DEĞİŞKENLERİ ===
    private float orijinalHiz;
    private bool yavaslatildiMi = false;

    // Property'ler (Erişimciler)
    public float CurrentHealth => currentHealth;
    public string NameID => enemyNameID;
    public float Armor => armor;
    public int Reward => reward;
    public int BaseDmg => baseDamage;
    public int WaypointIndex => currentWaypointIndex;

    // === BAŞLANGIÇ AYARLARI (TEK FONKSİYONDA BİRLEŞTİRİLDİ) ===
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        orijinalHiz = speed; // Başlangıç hızını hafızaya al
    }

    // === OYUN DÖNGÜSÜ (HAREKET MANTIĞI) ===
    protected virtual void Update()
    {
        // Güvenlik Kontrolü: GameManager veya Yol Noktaları yoksa hiçbir şey yapma
        if (GameManager.Instance == null || GameManager.Instance.yolNoktalari == null || GameManager.Instance.yolNoktalari.Count == 0)
            return;

        // Hedefe vardık mı?
        if (currentWaypointIndex < GameManager.Instance.yolNoktalari.Count)
        {
            // 1. Hedefi Belirle
            Transform hedefNokta = GameManager.Instance.yolNoktalari[currentWaypointIndex];

            // 2. Oraya Doğru Yürü
            HareketEt(hedefNokta);

            // 3. Mesafe Kontrolü: Hedefe çok yaklaştık mı? (0.1 birim mesafe)
            if (Vector3.Distance(transform.position, hedefNokta.position) <= 0.1f)
            {
                // Bir sonraki noktaya geç
                currentWaypointIndex++;
            }
        }
        else
        {
            // Yol bitti, üsse ulaştık
            UsseSaldir();
        }
    }

    // === HAREKET FONKSİYONU ===
    public virtual void HareketEt(Transform hedefNokta)
    {
        if (hedefNokta != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, hedefNokta.position, speed * Time.deltaTime);
        }
    }

    // === YAVAŞLATMA SİSTEMİ ===
    // HackerCat bu fonksiyonu çağıracak
    public void HizDusur(float yavaslatmaOrani, float sure)
    {
        if (yavaslatildiMi) return;

        // Hızı düşür
        speed = orijinalHiz * (1f - yavaslatmaOrani);
        yavaslatildiMi = true;

        // Görsel efekt (HealthBarView üzerinden)
        GetComponentInChildren<HealthBarView>()?.SetSlowEffect(true);

        // Süre bitince hızı geri al
        StartCoroutine(HiziGeriAl(sure));
    }

    IEnumerator HiziGeriAl(float beklemeSuresi)
    {
        yield return new WaitForSeconds(beklemeSuresi);

        // Eski haline dön
        speed = orijinalHiz;
        yavaslatildiMi = false;

        // Görsel efekti kapat
        GetComponentInChildren<HealthBarView>()?.SetSlowEffect(false);
    }

    // === SOYUT METOTLAR (Alt sınıflar dolduracak) ===
    public abstract void HasarAl(float miktar);
    public abstract void Ol();
    public abstract void UsseSaldir();
}