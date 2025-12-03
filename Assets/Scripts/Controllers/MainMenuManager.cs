using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OyunuBaslat()
    {
        // Build Settings'deki 1 numaralı sahneyi aç
        SceneManager.LoadScene(1);
    }

    public void OyundanCik()
    {
        Debug.Log("Çıkış yapılıyor...");
        Application.Quit();
    }
}
