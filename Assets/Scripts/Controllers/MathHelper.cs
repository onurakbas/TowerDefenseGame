using UnityEngine;

public static class MathHelper
{
    /// <summary>
    /// Net Hasar Hesapla: Kule hasarı ve zırh değerine göre son hasarı verir.
    /// Formül: Net_Hasar = Kule_Hasarı * (1 - (Zırh / (Zırh + 100)))
    /// </summary>
    public static float NetHasarHesapla(float hamHasar, float hedefZirh)
    {
        // Formülün aynısı
        float hasarAzaltmaCarpani = hedefZirh / (hedefZirh + 100.0f);

        float netHasar = hamHasar * (1 - hasarAzaltmaCarpani);

        return netHasar;
    }
}