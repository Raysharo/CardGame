using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TableManager : MonoBehaviour
{
    private WebSocket ws;
    private Queue<Action> actionsToExecuteOnMainThread = new Queue<Action>();
    public PvPlayer pvPlayer; // Référence à votre classe PvPlayer

    // [SerializeField] // Cette ligne s'assure que l'array est visible dans l'Inspecteur Unity même s'il est privé
    //private Text[] healthDisplays; // Array pour les objets Text UI des PV des joueurs
    [SerializeField]
    private TextMeshProUGUI[] healthDisplays;

    [SerializeField]
    private TextMeshProUGUI[] piecesDisplays;

    public int currentPlayer = 1; // Commencez avec le joueur 1
    public int totalPlayers = 4; // Nombre total de joueurs

    public Vector3 scaleCardOnTable = new Vector3(1, 1, 0);
    public bool isMarket = false;

    void Start()
    {
        //string adresseNgrok = "8354-46-193-3-79";
        string adresseNgrok = NgrokManager.GetAdresseNgrok();
        // Initialiser la connexion WebSocket
        ws = new WebSocket(adresseNgrok + "0");



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
            InitializepiecesDisplays();
        }
        else
        {
            Debug.LogError("PvPlayer component not found!");
        }

        createMarket(currentPlayer);
    }

    public void supprimerMarket()
    {
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si l'ID de la carte correspond à celui que nous voulons détruire
            Debug.Log("supprimerMarket - ID de la carte: " + card.idPlayer);
            if (card.idPlayer == -1)
            {
                // Détruisez la carte
                UnityEngine.Object.Destroy(card.gameObject);
            }
        }
    }

    public int getCurrentPlayer()
    {
        Debug.Log("getCurrentPlayer - Current player: " + currentPlayer);
        return currentPlayer;
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


    public void InitializepiecesDisplays()
    {
        if (piecesDisplays != null && piecesDisplays.Length == 4)
        {
            // Mettre à jour le texte pour chaque affichage de PV
            for (int i = 0; i < piecesDisplays.Length; i++)
            {
                if (piecesDisplays[i] != null)
                {
                    piecesDisplays[i].text = "Pièces: " + pvPlayer.GetPiecesForPlayer(i + 1) * Constant.COINS_MULTIPLIER;


                    // Add a picture of a coin under the piecesDisplays object for each piece
                    for (int j = 0; j < pvPlayer.GetPiecesForPlayer(i + 1); j++)
                    {
                        // id of the array + 1 because the array start at 0
                        CreatePhysicalCoin(i , j );
                    }

                }
                else
                {
                    Debug.LogError("Pieces display Text object for player " + (i + 1) + " is not assigned in the inspector!");
                }
            }
        }
        else
        {
            Debug.LogError("PiecesDisplays array is not properly assigned in the inspector!");
        }
    }

    void CreatePhysicalCoin(int playerID, int pieceID)
    {
        Debug.Log("CreatePhysicalCoin - Création d'une pièce physique pour le joueur " + playerID + " et la pièce " + pieceID);
        GameObject coinObject = new GameObject("CoinPlayer" + playerID + "_PiecesNum_" + pieceID) ;
        // as child of the piecesDisplays object
        coinObject.transform.SetParent(piecesDisplays[playerID].transform, false);
        // position of the coin
        coinObject.transform.localPosition = new Vector3(-84 + 3 * pieceID, -25 , 0);
        // Add a SpriteRenderer component
        SpriteRenderer spriteRenderer = coinObject.AddComponent<SpriteRenderer>();
        // Set the sprite to be the coin sprite (Assets\Resources\Pictures\coin.jpg)
        spriteRenderer.sprite = Resources.Load<Sprite>("Pictures/coin");
        // Set the sorting order to be 1 so that the coin is always on top of the text
        spriteRenderer.sortingOrder = 1;
        // Set the scale of the coin
        const int COIN_SCALE = 7;
        coinObject.transform.localScale = new Vector3(COIN_SCALE, COIN_SCALE, COIN_SCALE);

    }


    public int GetPieces()
    {
        return pvPlayer.GetPiecesForPlayer(currentPlayer);
    }

    public void SetPieces(int piece)
    {
        pvPlayer.ReducePieces(currentPlayer, piece);
        UpdatePiecesDisplay(currentPlayer, pvPlayer.GetPiecesForPlayer(currentPlayer));
    }

    public void IncrementPieces(int piece)
    {
        pvPlayer.AddPieces(currentPlayer, piece);
        UpdatePiecesDisplay(currentPlayer, pvPlayer.GetPiecesForPlayer(currentPlayer));
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
        isMarket = false;
        if (playerId == 1)
        {
            rotationDegreesZ = 0.0f;
            CreateCard(new Vector3(-4.5f, -3.8f, 0), scaleCardOnTable, cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard, isMarket);
        }
        else if (playerId == 2)
        {
            rotationDegreesZ = -90.0f;
            CreateCard(new Vector3(-7.7f, 2.7f, 0), scaleCardOnTable, cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard, isMarket);
        }
        else if (playerId == 3)
        {
            rotationDegreesZ = 180.0f;
            CreateCard(new Vector3(4.5f, 3.8f, 0), scaleCardOnTable, cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard, isMarket);
        }
        else if (playerId == 4)
        {
            rotationDegreesZ = 90.0f;
            CreateCard(new Vector3(7.7f, -2.7f, 0), scaleCardOnTable, cardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconCard, isMarket);
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
        textObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Ajustez cette échelle selon vos besoins

    }

    Color CalculerCouleurOpposee(Color couleurOriginale)
    {
        // Calculer la couleur opposée pour chaque composante RVB
        Color couleurOpposee = new Color(1.0f - couleurOriginale.r, 1.0f - couleurOriginale.g, 1.0f - couleurOriginale.b);

        return couleurOpposee;
    }

    void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints, string text, float rotationDegreesZ, int playerId, string iconCard, bool isMarket)
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
                if (isMarket)
                {
                    int prix = UnityEngine.Random.Range(1, 5);
                    text = $"Prix: {prix * Constant.COINS_MULTIPLIER}\nAttaque: {attackPoints}\nDéfense: {defensePoints}";
                    cardComponent.prix = prix;
                    cardComponent.idPlayer = -1;
                }
                else
                {
                    cardComponent.prix = 0;
                    cardComponent.idPlayer = playerId;
                }
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
        if (newHealth <= 0)
        {
            healthDisplays[playerNumber - 1].text = "MORT";
            // TODO: execute new scene with winner
            SceneManager.LoadScene("GameOver");
        }
    }

    public void UpdatePiecesDisplay(int playerNumber, int newPieces)
    {
        Debug.Log("UpdatePiecesDisplay - Mise à jour de l'affichage des Pièces du joueur " + playerNumber + " à " + newPieces);

        // parse the old text to get the number of pieces
        string oldText = piecesDisplays[playerNumber - 1].text;
        string[] words = oldText.Split(' ');
        int oldPieces = Int32.Parse(words[1]) / Constant.COINS_MULTIPLIER;
        Debug.Log("UpdatePiecesDisplay - oldPieces: " + oldPieces);
        if(newPieces > oldPieces){
            for(int i = 0; i < newPieces - oldPieces; i++){
                CreatePhysicalCoin(playerNumber -1 , oldPieces + i + 1);
            }
        } else {
            // destroy a cylinder object under the piecesDisplays object for each piece removed
            for(int i = 0; i < oldPieces - newPieces; i++){
                GameObject coinObject = GameObject.Find("CoinPlayer" + (playerNumber-1) + "_PiecesNum_" + (newPieces + i));
                Destroy(coinObject);
            }
        }
        

        piecesDisplays[playerNumber - 1].text = "Pièces: " + newPieces * Constant.COINS_MULTIPLIER;



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
        //increment piece player
        IncrementPieces(1);
        currentPlayer++;
        if (currentPlayer > totalPlayers)
        {
            currentPlayer = 1; // Retour au premier joueur
                               //test
        }
        isMarket = true;
        supprimerMarket();
        // incremrnt piece

        StartCoroutine(AlphaColorZoneAnimation(currentPlayer));

        ShowPlayerTurn(currentPlayer);

        createMarket(currentPlayer);
    }

    void ShowPlayerTurn(int currentPlayer)
    {
        // Add a TextMeshPro text to the middle of the scene to indicate the current player turn
        // Bold, 43.5 font size, centered text, Middle Center alignment, white color, auto size
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
        textObject.transform.localPosition = new Vector3(0, 0, 0); // Centrez sur la carte
        // textObject.transform.localScale = new Vector3(1, 1, 1); //  // Adjust width (x) and height (y) as needed
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Tour du joueur " + currentPlayer;
        textComponent.fontSize = Constant.CHANGE_TURN_TEXT_FONT_SIZE;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.white;
        textComponent.autoSizeTextContainer = true;

        // make the text rotate to be visible by the current player
        if (currentPlayer == 1)
        {
            textObject.transform.Rotate(0, 0, 0);
        }
        else if (currentPlayer == 2)
        {
            textObject.transform.Rotate(0, 0, -90);
        }
        else if (currentPlayer == 3)
        {
            textObject.transform.Rotate(0, 0, 180);
        }
        else if (currentPlayer == 4)
        {
            textObject.transform.Rotate(0, 0, 90);
        }

        // Destroy the text after 5 seconds
        Destroy(textObject, Constant.DURATION_SHOW_CHANGE_TURN_TEXT);
    }

    // DO NOT WORK : make the border picture appear over the zone of the player
    void AddBorder(int playerID, Color borderColor, float borderWidth = 10f)
    {
        // Create an empty GameObject for the border
        GameObject borderObject = new GameObject("Border");
        RectTransform borderRectTransform = borderObject.AddComponent<RectTransform>();

        // Get the original image
        Image originalImage = GameObject.Find("Image Zone_player_" + playerID).GetComponent<Image>();

        // Set the parent of the border to be the same as the original image
        borderObject.transform.SetParent(originalImage.transform, false);

        // Adjust the size of the border
        borderRectTransform.sizeDelta = new Vector2(
            originalImage.rectTransform.sizeDelta.x + 2 * borderWidth,
            originalImage.rectTransform.sizeDelta.y + 2 * borderWidth
        );

        // Set the z position to be behind the original image
        borderRectTransform.localPosition = new Vector3(0, 0, -1);

        // Add an Image component to the border
        Image borderImage = borderObject.AddComponent<Image>();
        borderImage.color = borderColor;
    }

    void RemoveBorder(int playerID)
    {
        // Find the border GameObject
        GameObject borderObject = GameObject.Find("Image Zone_player_" + playerID + "/Border");

        // If the border GameObject exists, destroy it
        if (borderObject != null)
        {
            Destroy(borderObject);
        }
    }


    IEnumerator AlphaColorZoneAnimation(int playerNumber)
    {
        Debug.Log("AlphaColorZoneAnimation - Animation de la zone de couleur du joueur " + playerNumber);
        // Récupérer le composant de couleur de la zone de couleur du joueur
        Image image = GameObject.Find("Image Zone_player_" + playerNumber).GetComponent<Image>();
        float originalColorAlpha = image.color.a;

        StartCoroutine(IncreaseAlphaColorZoneAnimationCoroutine(image, 1));
        yield return new WaitForSeconds(1.0f); // Adjust the duration as needed
        StartCoroutine(DecreaseAlphaColorZoneAnimationCoroutine(image, originalColorAlpha, 1));
    }


    IEnumerator IncreaseAlphaColorZoneAnimationCoroutine(Image image, float targetAlpha, float duration = 0.5f)
    {
        Debug.Log("IncreaseAlphaColorZoneAnimationCoroutine - Animation de la zone de couleur du joueur " + image.name);
        // Récupérer la couleur actuelle de la zone de couleur
        Color originalColor = image.color;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Augmenter progressivement l'alpha de la couleur
            float newAlpha = Mathf.Lerp(originalColor.a, targetAlpha, elapsedTime / duration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that the final alpha value is exactly the target alpha
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }

    IEnumerator DecreaseAlphaColorZoneAnimationCoroutine(Image image, float targetAlpha, float duration = 0.5f)
    {
        Debug.Log("DecreaseAlphaColorZoneAnimationCoroutine - Animation de la zone de couleur du joueur " + image.name);
        // Récupérer la couleur actuelle de la zone de couleur
        Color originalColor = image.color;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Diminuer progressivement l'alpha de la couleur
            float newAlpha = Mathf.Lerp(originalColor.a, targetAlpha, elapsedTime / duration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that the final alpha value is exactly the target alpha
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }



    public void SendMessageToPlayer(string message)
    {
        if (ws != null && ws.ReadyState == WebSocketState.Open)
        {
            ws.Send(message);
        }
    }

    public void createMarket(int playerId)
    {
        //Créer le marché
        Debug.Log("createMarket - Création du marché");

        // GameObject market = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // market.transform.localScale = new Vector3(5, 5, 0.1f);
        string text = $"Marché";
        if (playerId == 1)
        {
            CreateforCardMarket(text, 0.0f, playerId);
        }
        else if (playerId == 2)
        {
            CreateforCardMarket(text, -90.0f, playerId);
        }
        else if (playerId == 3)
        {
            CreateforCardMarket(text, 180.0f, playerId);
        }
        else if (playerId == 4)
        {
            CreateforCardMarket(text, 90.0f, playerId);
        }

    }

    public void CreateforCardMarket(string text, float rotationDegreesZ, int playerId)
    {
        Type[] cardTypes = { typeof(GoldCard), typeof(BlueCard), typeof(GreenCard) };
        string[] iconNames = { "Icon1", "Icon2", "Icon3", "Icon4", "Icon5", "Icon6", "Icon7" };


        for (int i = 0; i < 4; i++)
        {
            Type randomCardType = cardTypes[UnityEngine.Random.Range(0, cardTypes.Length)];
            string iconSelected = iconNames[UnityEngine.Random.Range(0, iconNames.Length)];
            int attackPoints = UnityEngine.Random.Range(10, 25);
            int defensePoints = UnityEngine.Random.Range(1, 15);
            isMarket = true;

            if (playerId == 1)
            {
                CreateCard(new Vector3(-3 + 2 * i, 0, 0), scaleCardOnTable, randomCardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconSelected, isMarket);
            }
            else if (playerId == 2)
            {
                CreateCard(new Vector3(-2, -1 + 1.7f * i, 0), scaleCardOnTable, randomCardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconSelected, isMarket);
            }
            else if (playerId == 3)
            {
                CreateCard(new Vector3(3 - 2 * i, 0, 0), scaleCardOnTable, randomCardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconSelected, isMarket);
            }
            else if (playerId == 4)
            {
                CreateCard(new Vector3(2, 1 - 1.7f * i, 0), scaleCardOnTable, randomCardType, attackPoints, defensePoints, text, rotationDegreesZ, playerId, iconSelected, isMarket);
            }
        }

    }

    public void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints, string iconSelected, int prix)
    {
        GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Modifier la taille pour en faire une carte
        cardObject.transform.localScale = scale;
        cardObject.transform.position = position;
        // Ajouter le composant de carte du type spécifié
        //cardType.BaseType.GetMethod("InitializeCard").Invoke(cardObject.AddComponent(cardType), new object[] { attackPoints, defensePoints });
        Card cardComponent = (Card)cardObject.AddComponent(cardType);
        // Initialiser la carte (si vous avez une fonction pour cela dans votre classe Card)
        //cardComponent.InitializeCard();
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
        cardComponent.owner = null;
        cardComponent.idPlayer = -1;

    }



}
