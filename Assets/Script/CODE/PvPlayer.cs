using UnityEngine;
using UnityEngine.UI;

public class PvPlayer : MonoBehaviour
{
    public int lifePointsPlayer1 = 100;
    public int lifePointsPlayer2 = 100;
    public int lifePointsPlayer3 = 100;
    public int lifePointsPlayer4 = 100;

    public Image Player1CurrentHealth;
    public Image Player2CurrentHealth;
    public Image Player3CurrentHealth;
    public Image Player4CurrentHealth;

    void Start()
    {
        ReduceLifePoints(1, 30); // Réduit de 30 points la vie du joueur 1 pour tester
        ReduceLifePoints(2, 50); // Réduit de 50 points la vie du joueur 2 pour tester
        ReduceLifePoints(3, 80); // Réduit de 80 points la vie du joueur 3 pour tester
        ReduceLifePoints(4, 70); // Réduit de 70 points la vie du joueur 4 pour tester
        Debug.Log("Start: PV Joueur 1 après réduction = " + lifePointsPlayer1);
        Debug.Log("Start: PV Joueur 2 après réduction = " + lifePointsPlayer2);
    }

    public int GetLifePointsForPlayer(int playerNumber)
    {
        int lifePoints = 0;
        switch (playerNumber)
        {
            case 1: lifePoints = lifePointsPlayer1; break;
            case 2: lifePoints = lifePointsPlayer2; break;
            case 3: lifePoints = lifePointsPlayer3; break;
            case 4: lifePoints = lifePointsPlayer4; break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
        Debug.Log("GetLifePointsForPlayer: PV Joueur " + playerNumber + " = " + lifePoints);
        return lifePoints;
    }

    public void UpdateHealthBar(int playerNumber)
    {
        float lifePercent = GetLifePercentForPlayer(playerNumber);
        Debug.Log("UpdateHealthBar: Pourcentage de vie pour le joueur " + playerNumber + " = " + lifePercent);
        switch (playerNumber)
        {
            case 1: Player1CurrentHealth.fillAmount = lifePercent; break;
            case 2: Player2CurrentHealth.fillAmount = lifePercent; break;
            case 3: Player3CurrentHealth.fillAmount = lifePercent; break;
            case 4: Player4CurrentHealth.fillAmount = lifePercent; break;
            default: Debug.LogError("Invalid player number!"); break;
        }
        Canvas.ForceUpdateCanvases();
    }

    float GetLifePercentForPlayer(int playerNumber)
    {
        float lifePercent = 0f;
        switch (playerNumber)
        {
            case 1: lifePercent = lifePointsPlayer1 / 100f; break;
            case 2: lifePercent = lifePointsPlayer2 / 100f; break;
            case 3: lifePercent = lifePointsPlayer3 / 100f; break;
            case 4: lifePercent = lifePointsPlayer4 / 100f; break;
            default: Debug.LogError("Invalid player number!"); break;
        }
        return lifePercent;
    }

    public void ReduceLifePoints(int playerId, int damage)
    {
        switch (playerId)
        {
            case 1: lifePointsPlayer1 -= damage; break;
            case 2: lifePointsPlayer2 -= damage; break;
            case 3: lifePointsPlayer3 -= damage; break;
            case 4: lifePointsPlayer4 -= damage; break;
            default: Debug.LogError("Invalid player number!"); break;
        }
        Debug.Log("ReduceLifePoints: PV Joueur " + playerId + " après dégâts = " + GetLifePointsForPlayer(playerId));
        UpdateHealthBar(playerId);
    }
}
