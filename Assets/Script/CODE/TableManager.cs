
using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TableManager : MonoBehaviour
{
    private WebSocket ws;
    private Queue<Action> actionsToExecuteOnMainThread = new Queue<Action>();
    public PvPlayer pvPlayer; // Référence à votre classe PvPlayer

    // [SerializeField] // Cette ligne s'assure que l'array est visible dans l'Inspecteur Unity même s'il est privé
    //private Text[] healthDisplays; // Array pour les objets Text UI des PV des joueurs
    [SerializeField]
    private TextMeshProUGUI[] healthDisplays;

    public int currentPlayer = 1; // Commencez avec le joueur 1
    public int totalPlayers = 4; // Nombre total de joueurs


    void Start()
    {
        //string adresseNgrok = "8354-46-193-3-79";
        string adresseNgrok = NgrokManager.GetAdresseNgrok();
        // Initialiser la connexion WebSocket
        ws = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/0");

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message reçu: " + e.Data);
            HandleCardMessage(e.Data);
        };

        ws.Connect();
        // pvPlayer = new PvPlayer();
        // InitializeHealthDisplays();
        //pvPlayer = GetComponent<PvPlayer>(); // This gets the PvPlayer component on the same GameObject.

        // If PvPlayer is on a different GameObject, you'd use something like:
        pvPlayer = GameObject.Find("PvPlayer").GetComponent<PvPlayer>();

        // Make sure pvPlayer is not null before calling InitializeHealthDisplays
        if (pvPlayer != null)
        {
            InitializeHealthDisplays();
        }
        else
        {
            Debug.LogError("PvPlayer component not found!");
        }

    }

    void InitializeHealthDisplays()
    {
        if (healthDisplays != null && healthDisplays.Length == 4)
        {
            // Mettre à jour le texte pour chaque affichage de PV
            for (int i = 0; i < healthDisplays.Length; i++)
            {
                if (healthDisplays[i] != null)
                {
                    healthDisplays[i].text = "PV: " + pvPlayer.GetLifePointsForPlayer(i + 1);
                }
                else
                {
                    Debug.LogError("Health display Text object for player " + (i + 1) + " is not assigned in the inspector!");
                }
            }
        }
        else
        {
            Debug.LogError("HealthDisplays array is not properly assigned in the inspector!");
        }
    }

    // Ajoutez une méthode pour mettre à jour l'affichage des PV quand ils changent


    void Update()
    {
        while (actionsToExecuteOnMainThread.Count > 0)
        {
            actionsToExecuteOnMainThread.Dequeue().Invoke();
        }
    }

    void HandleCardMessage(string data)
    {
        Debug.Log("HandleCardMessage - Données brutes reçues: " + data);

        // Tentez de parser le message JSON reçu
        CardMessage message;
        try
        {
            message = JsonUtility.FromJson<CardMessage>(data);
            Debug.Log($"Message parsé - Type: {message.type}, CardId: {message.cardId}, cardType: {message.cardType}, attackPoints: {message.attackPoints}, defensePoints: {message.defensePoints}, playerId: {message.playerId}, iconCard: {message.iconCard}");
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors du parsing JSON: " + e.Message);
            return;
        }

        if (message.type == "move card to player")
        {
            Debug.Log($"Traitement du message - Déplacement de la carte ID {message.cardId} vers la table.");
            // Créer ou mettre à jour la carte sur la table
            Type cardType = Type.GetType(message.cardType); // Convertissez le nom du type en Type
            int attackPoints = message.attackPoints;
            int defensePoints = message.defensePoints;
            string iconCard = message.iconCard;
            int playerId = message.playerId;
            if (cardType != null)
            {
                CreateOrUpdateCardOnTable(message.cardId, cardType, attackPoints, defensePoints, playerId, iconCard);
            }
            else
            {
                Debug.LogError("Type de carte non pris en charge: " + message.cardType);
            }
        }
    }



    // Assurez-vous de fermer la connexion WebSocket lors de la destruction de l'objet
    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }

    void CreateOrUpdateCardOnTable(int cardId, Type cardType, int attackPoints, int defensePoints, int playerId, string iconCard)
    {
        Debug.Log("CreateOrUpdateCardOnTable - Création de la carte ID " + cardId);
        Debug.Log("CreateOrUpdateCardOnTable - attackPoints " + attackPoints);
        Debug.Log("CreateOrUpdateCardOnTable - defensePoints " + defensePoints);
        Debug.Log("CreateOrUpdateCardOnTable - playerId " + playerId);
        string text = $"Attaque: {attackPoints}\nDéfense: {defensePoints}";
        float rotationDegreesZ = 0.0f;
        if (playerId == 1)
        {
            rotationDegreesZ = 0.0f;
            CreateCard(new Vector3(0, -4, 0), new Vector3(1, 1.5f, 0.1f), cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard);
        }
        else if (playerId == 2)
        {
            rotationDegreesZ = -90.0f;
            CreateCard(new Vector3(-9.5f, 0, 0), new Vector3(1, 1.5f, 0.1f), cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard);
        }
        else if (playerId == 3)
        {
            rotationDegreesZ = 180.0f;
            CreateCard(new Vector3(0, 4, 0), new Vector3(1, 1.5f, 0.1f), cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard);
        }
        else if (playerId == 4)
        {
            rotationDegreesZ = 90.0f;
            CreateCard(new Vector3(9.5f, 0, 0), new Vector3(1, 1.5f, 0.1f), cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard);
        }
    }

    void AddTextToCardUI(GameObject cardObject, string text, Vector3 localPosition)
    {
        // Créez un nouveau GameObject pour le Canvas s'il n'existe pas déjà sur la carte
        Canvas canvas = cardObject.GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("CardCanvas");
            canvasObject.transform.SetParent(cardObject.transform, false);
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.scaleFactor = 0.1f; // Ajustez selon la taille de la carte
            canvasScaler.dynamicPixelsPerUnit = 10f;
            canvasObject.AddComponent<GraphicRaycaster>();
            // Ajustez la taille et la position du Canvas pour qu'il corresponde à la carte
            RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
            canvasRectTransform.sizeDelta = new Vector2(100, 100); // Ajustez cette taille selon vos besoins
            canvasRectTransform.localPosition = localPosition; // Centrez sur la carte
        }

        // Créer l'objet de texte en tant qu'enfant de cardObject
        GameObject textObject = new GameObject("CardTextUI");
        textObject.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, -1); // Centrez sur la carte

        // Ajouter et configurer le composant de texte
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = 3;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = Color.black;

        // Ajuster la taille de l'échelle pour rendre le texte proportionnel à la carte
        textObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Ajustez cette échelle selon vos besoins

    }

    Color CalculerCouleurOpposee(Color couleurOriginale)
    {
        // Calculer la couleur opposée pour chaque composante RVB
        Color couleurOpposee = new Color(1.0f - couleurOriginale.r, 1.0f - couleurOriginale.g, 1.0f - couleurOriginale.b);

        return couleurOpposee;
    }

    void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints, string text, float rotationDegreesZ, int playerId, string iconCard)
    {
        actionsToExecuteOnMainThread.Enqueue(() =>
        {
            try
            {
                Debug.Log("CreateCard - Tentative de création de la carte");
                // Créer l'objet de la carte
                GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // Modifier la taille pour en faire une carte
                cardObject.transform.localScale = scale;
                cardObject.transform.position = position;
                // rotation de la carte
                cardObject.transform.Rotate(0, 0, rotationDegreesZ);
                // Ajouter le composant de carte du type spécifié
                Card cardComponent = (Card)cardObject.AddComponent(cardType);
                cardComponent.attackPoints = attackPoints;
                cardComponent.defensePoints = defensePoints;
                cardComponent.idPlayer = playerId;
                // Initialiser la carte
                cardComponent.InitializeCard(attackPoints, defensePoints);
                AddTextToCardUI(cardObject, text, new Vector3(0, -0.35f, 0));
                GameObject icon = new GameObject("Icon");
                icon.transform.SetParent(cardObject.transform, false);
                icon.transform.localPosition = new Vector3(0, 0.17f, -1); // Centrez sur la carte
                icon.transform.localScale = new Vector3(0.8f, 0.5f, 1f); //  // Adjust width (x) and height (y) as needed
                SpriteRenderer spriteRenderer = icon.AddComponent<SpriteRenderer>();
                cardComponent.iconCard = iconCard;
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + iconCard);
                spriteRenderer.sortingOrder = 1;
                Debug.Log("CreateCard - Carte créée avec succès");
            }
            catch (Exception e)
            {
                Debug.LogError("CreateCard - Erreur lors de la création de la carte: " + e.Message);
            }
        });
    }


    public void AttackPlayer(int attackerId, int defenderId, int attackPoints)
    {
        // Diminuer les points de vie du défenseur
        pvPlayer.ReduceLifePoints(defenderId, attackPoints);
        // Mettre à jour l'affichage des points de vie du défenseur
        UpdateHealthDisplay(defenderId, pvPlayer.GetLifePointsForPlayer(defenderId));
    }
    public void UpdateHealthDisplay(int playerNumber, int newHealth)
    {
        Debug.Log("UpdateHealthDisplay - Mise à jour de l'affichage des PV du joueur " + playerNumber + " à " + newHealth);
        healthDisplays[playerNumber - 1].text = "PV: " + newHealth;
    }

    public void UpdatePositionCards(int playerId)
    {
        Debug.Log("UpdatePositionCards - Mise à jour de la position des cartes du joueur " + playerId);
        Card[] allCards = FindObjectsOfType<Card>();
        foreach (Card card in allCards)
        {
            if (card.idPlayer == playerId && playerId == 1)
            {
                // a jour avec cette possition new Vector3(0, -3, 0)
                card.transform.position = new Vector3(0, -4, 0);
            }
            else if (card.idPlayer == playerId && playerId == 2)
            {
                // a jour avec cette possition new Vector3(-6, 0, 0)
                card.transform.position = new Vector3(-9.5f, 0, 0);
            }
            else if (card.idPlayer == playerId && playerId == 3)
            {
                // a jour avec cette possition new Vector3(0, 3, 0)
                card.transform.position = new Vector3(0, 4, 0);
            }
            else if (card.idPlayer == playerId && playerId == 4)
            {
                // a jour avec cette possition new Vector3(6, 0, 0)
                card.transform.position = new Vector3(9.5f, 0, 0);
            }
        }
    }

    public void NextPlayerTurn()
    {
        currentPlayer++;
        if (currentPlayer > totalPlayers)
        {
            currentPlayer = 1; // Retour au premier joueur
        }
    }




}
