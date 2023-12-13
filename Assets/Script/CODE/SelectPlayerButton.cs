// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SelectPlayerButton : MonoBehaviour
// {public int playerId; // Assignez cette valeur dans l'inspecteur à 1 pour Player 1 et à 2 pour Player 2

//     public void OnClick()
//     {
//         try
//         {
//             Debug.Log("Button clicked, player ID: " + playerId);
//             GameManager.PlayerId = playerId; // Mettre à jour l'ID du joueur

//             // Tentez de changer de scène
//             SceneManager.LoadScene("SampleScene");
            
//         }
//         catch (System.Exception e)
//         {
//             // Log l'erreur et ne change pas de scène si une exception est levée
//             Debug.LogError("Erreur lors du changement de scène: " + e.Message);
//         }
//     }
// }