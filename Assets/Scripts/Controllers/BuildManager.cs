using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance; // Her yerden ulaşmak için

    private void Awake()
    {
        Instance = this;
    }

    // Seçili olan kuleyi hafızada tutuyoruz
    private Tower secilenKulePrefab;

    public Tower GetSecilenKule()
    {
        return secilenKulePrefab;
    }

    // Butonlara tıklayınca bu fonksiyon çalışacak
    public void KuleSec(Tower kule)
    {
        secilenKulePrefab = kule;
        Debug.Log("Kule Seçildi: " + kule.NameID);
    }
}