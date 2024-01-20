using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;
using TMPro;


using WebSocketSharp;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;
//using UnityEditor.PackageManager.Requests;

public class Player
{

    public int id;
    public List<Card> cards = new List<Card>(); // Liste pour suivre les cartes du joueur
    public int lifePoints;

    public List<Card> Deck = new List<Card>();

    public WebSocket PlayerSocket;

    private TextMeshProUGUI deckCountDisplay;

    public Player(int id, string adresseNgrok, TextMeshProUGUI deckCountDisplay)
    {
        this.deckCountDisplay = deckCountDisplay;
        this.id = id;
        // PlayerSocket = new WebSocket("wss://" + adresseNgrok + "/" + id);
        PlayerSocket = new WebSocket(adresseNgrok + id);

        PlayerSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
            try
            {
                MessageManager.Instance.EnqueueMessage(e.Data, this);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Erreur : " + ex.Message);
            }
        };
        PlayerSocket.Connect();

        CreateCardPlayer(id);
        UpdateDeckCountDisplay(); // Afficher le nombre de cartes dans le deck
    }


    public void CreateCardPlayer(int id)
    {
        Type[] cardTypes = { typeof(GoldCard), typeof(BlueCard), typeof(GreenCard) };
        string[] iconNames = { "Icon1", "Icon2", "Icon3", "Icon4", "Icon5", "Icon6", "Icon7" };
        if (id == 1)
        {
            CreateforCard(cardTypes, iconNames);
        }
        else if (id == 2)
        {
            CreateforCard(cardTypes, iconNames);
        }
        else if (id == 3)
        {
            CreateforCard(cardTypes, iconNames);
        }
        else if (id == 4)
        {
            CreateforCard(cardTypes, iconNames);
        }
    }

    public void UpdateDeckCountDisplay()
    {
        if (deckCountDisplay != null)
        {
            // previous value
            int previousValue;
            try {
                previousValue = int.Parse(deckCountDisplay.text.Split(':')[1]);
            } catch (Exception e) {
                Debug.Log("Deck count display text is not in the expected format: " + deckCountDisplay.text);
                previousValue = 0;
            }

            // remove DeckCard as child of Deck object
            if(previousValue > Deck.Count)
            {
                for (int i = 0; i < previousValue - Deck.Count; i++)
                {
                    GameObject deckCard = GameObject.Find("DeckCard_" + (previousValue - i));
                    GameObject.Destroy(deckCard);
                }
            }
            // add DeckCard as child of Deck object
            else if(previousValue < Deck.Count)
            {
                // load Assets/Resources/Card_Shirts_Lite/PNG/Card_shirt_01.png
                Sprite sprite = Resources.Load<Sprite>("Card_Shirts_Lite/PNG/Card_shirt_01");
                for (int i = 0; i < Deck.Count - previousValue; i++)
                {
                    GameObject deckCard = new GameObject("DeckCard_" + (previousValue + i + 1));
                    deckCard.transform.SetParent(GameObject.Find("Deck").transform, false);
                    deckCard.transform.localPosition = new Vector3( Constant.DIST_BETWEEN_DECK_CARDS * (previousValue + i), -274, 0);
                    deckCard.transform.localScale = new Vector3(Constant.SCALE_DECK_CARDS, Constant.SCALE_DECK_CARDS, 0.1f);
                    SpriteRenderer spriteRenderer = deckCard.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.sortingOrder = 1;
                }
            }
           

            deckCountDisplay.text = "Deck: " + Deck.Count.ToString();
        }
    }


    public void CreateforCard(Type[] cardTypes, string[] iconNames)
    {
        for (int i = 0; i < 4; i++)
        {
            Type randomCardType = cardTypes[UnityEngine.Random.Range(0, cardTypes.Length)];
            string iconSelected = iconNames[UnityEngine.Random.Range(0, iconNames.Length)];
            int attackPoints = UnityEngine.Random.Range(10, 25);
            int defensePoints = UnityEngine.Random.Range(1, 15);
            CreateCard(new Vector3(-2 + 2 * i, 0, 0), Constant.SCALE_CARD_ON_TEL, randomCardType, attackPoints, defensePoints, iconSelected, false);
        }
    }


    // create function create card from deck 
    public void CreateCardFromDeck()
    {
        //Random to deck
        for (int i = 0; i < 4; i++)
        {
            Card card = Deck[UnityEngine.Random.Range(0, Deck.Count)];
            CreateCard(new Vector3(-2 + 2 * i, 0, 0), Constant.SCALE_CARD_ON_TEL, card.GetType(), card.attackPoints, card.defensePoints, card.iconCard, false);
            Deck.Remove(card);

            UpdateDeckCountDisplay();
        }
    }


    public void HandleCarteDetruiteMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        Debug.Log("Message reçu du serveur : " + message);
        InfosMessage carteDetruite = JsonUtility.FromJson<InfosMessage>(message);
        Debug.Log("Action reçue : " + carteDetruite.type);
        // Vérifier l'id du joueur
        if (carteDetruite.idPlayer == 1)
        {
            GameManager.Instance.DecrementZoneIncarte1();
        }
        else if (carteDetruite.idPlayer == 2)
        {
            GameManager.Instance.DecrementZoneIncarte2();
        }
        else if (carteDetruite.idPlayer == 3)
        {
            GameManager.Instance.DecrementZoneIncarte3();
        }
        else if (carteDetruite.idPlayer == 4)
        {
            GameManager.Instance.DecrementZoneIncarte4();
        }
    }



    public void HandleRequestCardsMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        Debug.Log("Message reçu du serveur : " + message);
        RequestInfo requestInfo = JsonUtility.FromJson<RequestInfo>(message);
        Debug.Log("Action reçue : " + requestInfo.action);

        // Vérifier le type d'action
        if (requestInfo.action == "requestCards")
        {
            // Si le joueur ciblé est ce joueur
            if (requestInfo.targetPlayerId == this.id)
            {
                Debug.Log("Cartes demandées par le joueur " + requestInfo.requestingPlayerId);
                // Envoyer les informations sur les cartes
                SendCardInfo(requestInfo.requestingPlayerId, requestInfo.targetPlayerId);
            }
        }
    }

    public void HandleGiveCardsMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        ListCardMessage cardMessage = JsonUtility.FromJson<ListCardMessage>(message);
        int x = -2;
        foreach (var card in cardMessage.cards)
        {
            Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints + "  " + card.idPlayer + "  " + card.cardType + "  " + card.iconCard);
            Type cardType = Type.GetType(card.cardType);
            CreateCard(new Vector3(x, 4, 0), new Vector3(1, 1.5f, 0.1f), cardType, card.attackPoints, card.defensePoints, card.iconCard, true);
            x += 2;
        }
    }

    public void HandleCartePourLeMarcheMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        Debug.Log("Message reçu du serveur : " + message);
        CardMarket cardMarket = JsonUtility.FromJson<CardMarket>(message);

        Debug.Log("Carte trouvée : " + cardMarket.attackPoints + "  " + cardMarket.defensePoints + "  " + cardMarket.idPlayer + "  " + cardMarket.typeCard + "  " + cardMarket.iconCard);

        CreateCardforDeck(cardMarket.id, cardMarket.idPlayer, cardMarket.attackPoints, cardMarket.defensePoints, cardMarket.iconCard, cardMarket.type, cardMarket.typeCard);

    }

    public void CreateCardforDeck(int id, int idPlayer, int attackPoints, int defensePoints, string iconCard, string type, string typeCard)
    {
        // Créer l'objet de la carte
        GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Card cardComponent = (Card)cardObject.AddComponent(Type.GetType(typeCard));
        cardComponent.InitializeCard(attackPoints, defensePoints);
        cardComponent.iconCard = iconCard;
        cardComponent.owner = this;
        cardComponent.idPlayer = idPlayer;
        cardComponent.id = id;
        Deck.Add(cardComponent);
        // destroy card from scene after create card for deck
        DestroyCard(id);
        UpdateDeckCountDisplay();
    }

    private void SendCardInfo(int requestingPlayerId, int targetPlayerId)
    {
        Debug.Log("Nombre de cartes trouvées : " + cards.Count);

        Debug.Log("Cartes demandées par le joueur requestingPlayerId " + requestingPlayerId);
        Debug.Log("Cartes envoyées au joueur targetPlayerId " + targetPlayerId);

        List<CardData> cardDataList = new List<CardData>();
        foreach (var card in cards)
        {
            Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints);
            CardData cardData = new CardData(card);
            cardDataList.Add(cardData);
        }

        // how to send liste cards  to player 1 use web socket to send liste cards  to player 1

        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            // Créer un objet CardMessage
            //ListCardMessage cardMessage = new ListCardMessage("giveCards", cards);
            ListCardMessage cardMessage = new ListCardMessage("giveCards", cardDataList, requestingPlayerId, targetPlayerId);
            string message = JsonUtility.ToJson(cardMessage);
            PlayerSocket.Send(message);
        }
    }

    public void RequestCards(int requestingPlayerId, int targetPlayerId)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            Debug.Log("Requesting cards from player " + targetPlayerId + " to player " + requestingPlayerId);
            // Créer un objet RequestInfo
            RequestInfo requestInfo = new RequestInfo("requestCards", requestingPlayerId, targetPlayerId);
            string message = JsonUtility.ToJson(requestInfo);
            PlayerSocket.Send(message);
        }
    }

    public void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints, string iconSelected, bool isGetCard)
    {
        // Assuming icons are named "Icon1", "Icon2", ..., "Icon7" in the Resources/Sprites folder

        // Créer l'objet de la carte
        GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Modifier la taille pour en faire une carte
        cardObject.transform.localScale = scale;
        cardObject.transform.position = position;
                // Ajouter le composant de carte du type spécifié
        //cardType.BaseType.GetMethod("InitializeCard").Invoke(cardObject.AddComponent(cardType), new object[] { attackPoints, defensePoints });
        Card cardComponent = (Card)cardObject.AddComponent(cardType);
        // Initialiser la carte (si vous avez une fonction pour cela dans votre classe Card)
        cardComponent.InitializeCard(attackPoints, defensePoints);
        // Assigner le joueur comme propriétaire de la carte
        string text = $"Attaque: {attackPoints}\nDéfense: {defensePoints}";
        AddTextToCardUI(cardObject, text, new Vector3(0, -0.35f, 0));

        GameObject icon = new GameObject("Icon");
        icon.transform.SetParent(cardObject.transform, false);
        icon.transform.localPosition = new Vector3(0, 0.17f, -1); // Centrez sur la carte
        icon.transform.localScale = new Vector3(0.8f, 0.5f, 1f); //  // Adjust width (x) and height (y) as needed
        SpriteRenderer spriteRenderer = icon.AddComponent<SpriteRenderer>();
        cardComponent.iconCard = iconSelected;
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + iconSelected);
        spriteRenderer.sortingOrder = 1;

        if (isGetCard == false)
        {
            // string iconSelected = iconNames[UnityEngine.Random.Range(0, iconNames.Length)];
            cardComponent.owner = this;
            cardComponent.idPlayer = this.id;
            cards.Add(cardComponent); // Ajoutez la carte à la liste du joueur
        }
    }


    // Fonction pour trouver l'ensemble des cartes presnt sur la scene
    public void FindAllCards()
    {
        // Trouvez toutes les cartes dans la scène
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si la carte appartient à ce joueur
            if (card.owner == this)
            {
                Debug.Log("carte appartient à ce joueur " + card.idPlayer);
                // Ajoutez la carte à la liste du joueur*
                Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints);
                //cards.Add(card);
            }
        }
    }

    public int CompteCarte()
    {
        //Debug.Log("Nombre de cartes trouvées : " + cards.Count);
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        int nbCarte = 0;
        foreach (var card in allCards)
        {
            nbCarte++;
        }
        Debug.Log("Nombre de cartes trouvées : " + nbCarte);
        return nbCarte - 1;
    }

    public void SupprimerCartefromlistCards(int idCard)
    {
        //cards.RemoveAt(idCard);
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si l'ID de la carte correspond à celui que nous voulons détruire
            if (card.id == idCard)
            {
                cards.Remove(card);
                //Deck.Add(card);
                //UpdateDeckCountDisplay();
            }
        }
    }

    public void SendMessageToTAble(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }


    public void SendMessageToPlayer(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }

    public void HandleCarteCoequipierMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        Debug.Log("Message reçu du serveur : " + message);
        CardDataCoequipier cardDataCoequipier = JsonUtility.FromJson<CardDataCoequipier>(message);
        Debug.Log("Carte trouvée : " + cardDataCoequipier.attackPoints + "  " + cardDataCoequipier.defensePoints + "  " + cardDataCoequipier.idPlayerCoequipier + "  " + cardDataCoequipier.cardType + "  " + cardDataCoequipier.iconCard);

        // Rappel format name card object "GoldCard" or "GreenCard" + the id of the card
        
        // Get the card object
        GameObject cardObject = GameObject.Find(cardDataCoequipier.cardType + cardDataCoequipier.idCard);
        if(cardObject == null)
        {
            Debug.Log("Card not found: " + cardDataCoequipier.cardType + cardDataCoequipier.idCard);
            return;
        }

        // TEMPORARY : add a square over the card
        GameObject square = GameObject.CreatePrimitive(PrimitiveType.Cube);
        square.transform.SetParent(cardObject.transform, false);
        square.transform.localPosition = new Vector3(0, 0, -3); // Centrez sur la carte
        square.transform.localScale = new Vector3(1.5f, 2.5f, 0.1f); //  // Adjust width (x) and height (y) as needed
        SpriteRenderer spriteRenderer = square.AddComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        spriteRenderer.sortingOrder = 1;

        // Delay the destruction of the square
        UnityEngine.Object.Destroy(square, 8);


        //CreateCardforDeck(cardDataCoequipier.idCard, cardDataCoequipier.idPlayerCoequipier, cardDataCoequipier.attackPoints, cardDataCoequipier.defensePoints, cardDataCoequipier.iconCard, cardDataCoequipier.type, cardDataCoequipier.cardType);

    } 



    public void AddTextToCardUI(GameObject cardObject, string text, Vector3 localPosition)
    {
        // Add a textMeshPro to the card autosized to fit the card
        GameObject textObject = new GameObject(cardObject.name + "_textObject");
        textObject.transform.SetParent(cardObject.transform, false);
        // make z position = -2 to make sure it is over the card
        textObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.y-2); // Centrez sur la carte
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.fontSize = 1.5f;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.black;

    }

    public void DestroyCard(int cardId)
    {
        // Trouvez toutes les cartes dans la scène
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si l'ID de la carte correspond à celui que nous voulons détruire
            if (card.id == cardId)
            {
                // Détruisez la carte
                UnityEngine.Object.Destroy(card.gameObject);
                break; // Sortez de la boucle si la carte a été trouvée et détruite
            }
        }
    }
}