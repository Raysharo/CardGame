using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvPlayer : MonoBehaviour
{
    public int lifePointsPlyer1 = 100;
    public int lifePointsPlyer2 = 100;
    public int lifePointsPlyer3 = 100;
    public int lifePointsPlyer4 = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetLifePointsForPlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1: return lifePointsPlyer1;
            case 2: return lifePointsPlyer2;
            case 3: return lifePointsPlyer3;
            case 4: return lifePointsPlyer4;
            default:
                Debug.LogError("Invalid player number!");
                return 0;
        }
    }
}
