using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int PlayerID { get; set; }

    public int ZoneIncarte1 = 0;
    public int ZoneIncarte2 = 0;
    public int ZoneIncarte3 = 0;
    public int ZoneIncarte4 = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadPlayerScene(int playerID)
    {
        PlayerID = playerID;
        SceneManager.LoadScene("PlayerScene");
    }

    public void IncrementZoneIncarte1()
    {
        ZoneIncarte1++;
    }

    public void IncrementZoneIncarte2()
    {
        ZoneIncarte2++;
    }

    public void IncrementZoneIncarte3()
    {
        ZoneIncarte3++;
    }

    public void IncrementZoneIncarte4()
    {
        ZoneIncarte4++;
    }



    public void DecrementZoneIncarte1()
    {
        ZoneIncarte1--;
    }

    public void DecrementZoneIncarte2()
    {
        ZoneIncarte2--;
    }

    public void DecrementZoneIncarte3()
    {
        ZoneIncarte3--;
    }

    public void DecrementZoneIncarte4()
    {
        ZoneIncarte4--;
    }

    public int GetZoneIncarte1()
    {
        return ZoneIncarte1;
    }

    public int GetZoneIncarte2()
    {
        return ZoneIncarte2;
    }

    public int GetZoneIncarte3()
    {
        return ZoneIncarte3;
    }

    public int GetZoneIncarte4()
    {
        return ZoneIncarte4;
    }


}
