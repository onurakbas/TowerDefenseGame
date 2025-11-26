using UnityEngine;
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

    protected virtual void Update()
    {
        // Güvenlik Kontrolü: GameManager veya Yol Noktaları yoksa hiçbir şey yapma
        if (GameManager.Instance == null || GameManager.Instance.yolNoktalari == null || GameManager.Instance.yolNoktalari.Count == 0)
            return;

        // Hedefe vardık mı? (Yolun sonuna geldik mi?)
        // Eğer mevcut indeks, liste sayısından küçükse yürümeye devam et
        if (currentWaypointIndex < GameManager.Instance.yolNoktalari.Count)
        {
            // 1. Hedefi Belirle
            Transform hedefNokta = GameManager.Instance.yolNoktalari[currentWaypointIndex];

            // 2. Oraya Doğru Yürü (Mevcut HareketEt fonksiyonunu kullanıyoruz)
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
            // Yol bitti! (Index sayıya eşit veya büyük)
            // Demek ki üsse ulaştık.
            UsseSaldir();
        }
    }

    // Property'ler (Erişimciler)
    public float CurrentHealth => currentHealth;
    public string NameID => enemyNameID;
    public float Armor => armor;
    public int Reward => reward;
    public int BaseDmg => baseDamage;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // HasarAl: Düşmanın canını azaltacak fonksiyon
    public abstract void HasarAl(float miktar);

    // Ol: Düşman öldüğünde çalışacak fonksiyon
    public abstract void Ol();

    // UsseSaldir: Düşman yolun sonuna gelince çalışacak
    public abstract void UsseSaldir();

    // HareketEt: Hedefe doğru yürüme işlemi
    public virtual void HareketEt(Transform hedefNokta)
    {
        if (hedefNokta != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, hedefNokta.position, speed * Time.deltaTime);
        }
    }
}