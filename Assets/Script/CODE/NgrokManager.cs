using UnityEngine;

public class NgrokManager : MonoBehaviour
{
    // private static string adresseNgrok = "192.168.1.4:3000";
    private static string adresseNgrok = "wss://192.168.1.4:3000/";


    public static string GetAdresseNgrok()
    {
        return adresseNgrok;
    }
} 

