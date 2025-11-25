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