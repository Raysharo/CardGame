using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button seeCardPlayButton; // Assurez-vous d'assigner ceci dans l'inspecteur Unity ou via le code.

    public bool isGetCard = false;
    private bool isButtonCooldown = false;

    public int idPlayer = -1;

    void Start()
    {
        // Vérifiez si le bouton est assigné
        if (seeCardPlayButton != null)
        {
            // Abonnez-vous à l'événement onClick du bouton
            seeCardPlayButton.onClick.AddListener(OnSeeCardPlayClicked);
        }
    }

    // Fonction de rappel appelée lorsque le bouton est cliqué

    public void OnSeeCardPlayClicked()
    {
        if (isButtonCooldown) return; // Ignore les clics supplémentaires
        isButtonCooldown = true; // Commence le cooldown
        Debug.Log("Bouton 'see card play' cliqué!");
        Debug.Log("isGetCard eb de if  avant : " + isGetCard);
        // Ici, mettez la logique à exécuter lors du clic sur le bouton.
        // Par exemple, changer les cartes affichées à l'écran.
        FindAllCards();
        StartCoroutine(ResetButtonCooldown());
    }

    private IEnumerator ResetButtonCooldown()
    {
        yield return new WaitForSeconds(0.5f); // Attendez 0.5 seconde
        isButtonCooldown = false; // Réactive le bouton
    }
    public void FindAllCards()
    {
        // Trouvez toutes les cartes dans la scène
        Card[] allCards = FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si la carte appartient au joueur actuel
            // Logique pour traiter les cartes du joueur actuel
            Debug.Log("Carte trouvée : att : " + card.attackPoints + " def :  " + card.defensePoints + " idPlayer : " + card.idPlayer);
            // Par exemple, vous pourriez ajouter une logique pour afficher ou mettre à jour l'UI ici
            if (isGetCard == false)
            {
                idPlayer = card.owner.id;
            }

            if ((isGetCard == true) && (card.idPlayer != idPlayer))
            {
                Debug.Log("Carte trouvée : ID : " + card.idPlayer);
                card.gameObject.SetActive(false);
            }
        }

        if (isGetCard == false)
        {
            if (idPlayer == 1)
            {
                // how to get card of player 3 use web socket to get card of player 3
                Debug.Log("Player 1");
                allCards[0].owner.RequestCards(1, 3);
                isGetCard = true;
            }
            else if (idPlayer == 2)
            {
                // how to get card of player 4 use web socket to get card of player 4
                Debug.Log("Player 2");
                allCards[0].owner.RequestCards(2, 4);
                isGetCard = true;
            }
            else if (idPlayer == 3)
            {
                // how to get card of player 1 use web socket to get card of player 1
                Debug.Log("Player 3");
                allCards[0].owner.RequestCards(3, 1);
                isGetCard = true;
            }
            else if (idPlayer == 4)
            {
                // how to get card of player 2 use web socket to get card of player 2
                Debug.Log("Player 4");
                allCards[0].owner.RequestCards(4, 2);
                isGetCard = true;
            }
        }
        else
        {
            isGetCard = false;
        }

        // Debug.Log("isGetCard eb de if  avant : " + isGetCard);

        // Debug.Log("isGetCard eb de if apres  : " + isGetCard);
    }
}
