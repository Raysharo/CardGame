using UnityEngine;

public class NgrokManager : MonoBehaviour
{
    
    // AQUARIUM
    private static string adresseNgrok = "wss://192.168.1.4:3000/";
    // ME
    // private static string adresseNgrok = "wss://192.168.231.178:3000/";

    public static string GetAdresseNgrok()
    {
        return adresseNgrok;
    }
} 

