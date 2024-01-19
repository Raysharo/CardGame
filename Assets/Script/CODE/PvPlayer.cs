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


    public int PiecesPlayer1 = 1;
    public int PiecesPlayer2 = 1;
    public int PiecesPlayer3 = 1;
    public int PiecesPlayer4 = 1;

    // audio
    public AudioSource audioSource;
    public AudioClip damageAudioClip;

    void Start()
    {
        // ReduceLifePoints(1, 30); // Réduit de 30 points la vie du joueur 1 pour tester
        // ReduceLifePoints(2, 50); // Réduit de 50 points la vie du joueur 2 pour tester
        // ReduceLifePoints(3, 80); // Réduit de 80 points la vie du joueur 3 pour tester
        // ReduceLifePoints(4, 70); // Réduit de 70 points la vie du joueur 4 pour tester
        // Debug.Log("Start: PV Joueur 1 après réduction = " + lifePointsPlayer1);
        // Debug.Log("Start: PV Joueur 2 après réduction = " + lifePointsPlayer2);

        // audio add
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (damageAudioClip == null)
        {
            // Assets\Resources\Sound\minecraft_hit_soundmp3converter.mp3
            damageAudioClip = Resources.Load<AudioClip>("Sound/minecraft_hit_soundmp3converter");
        }
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

    public int GetPiecesForPlayer(int playerNumber)
    {
        int Pieces = 0;
        switch (playerNumber)
        {
            case 1: Pieces = PiecesPlayer1; break;
            case 2: Pieces = PiecesPlayer2; break;
            case 3: Pieces = PiecesPlayer3; break;
            case 4: Pieces = PiecesPlayer4; break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
        Debug.Log("GetPiecesForPlayer: Pieces Joueur " + playerNumber + " = " + Pieces);
        return Pieces;
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
        // Play damage sound
        audioSource.PlayOneShot(damageAudioClip);
        UpdateHealthBar(playerId);
    }

    public void ReducePieces(int playerId,  int damage)
    {
        switch (playerId)
        {
            case 1: PiecesPlayer1 -= damage; break;
            case 2: PiecesPlayer2 -= damage; break;
            case 3: PiecesPlayer3 -= damage; break;
            case 4: PiecesPlayer4 -= damage; break;
            default: Debug.LogError("Invalid player number!"); break;
        }
    }

    public void AddPieces(int playerId, int damage)
    {
        switch (playerId)
        {
            case 1: PiecesPlayer1 += damage; break;
            case 2: PiecesPlayer2 += damage; break;
            case 3: PiecesPlayer3 += damage; break;
            case 4: PiecesPlayer4 += damage; break;
            default: Debug.LogError("Invalid player number!"); break;
        }
    }
}
