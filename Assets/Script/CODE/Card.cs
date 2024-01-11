using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

// using Program;


public abstract class Card : MonoBehaviour
{
    // ATTRIBUTS
    public int id;
    public Vector3 offset;

    // OBJECT
    public Renderer rend;
    // public BoxCollider2D collid;
    public BoxCollider collid;

    public Program Program;
    public bool onInteraction = false;

    // hitbox + grande que la carte
    private float boxScaleMargeX = 1;
    private float boxScaleFactorY = 1;
    public Color color;
    Light pointLight;
    private TableManager tableManager;


    private float clickDuration = 0.3f; // Seuil de durée pour un clic bref en secondes
    private float mouseDownTime;

    public Player owner;
    public int attackPoints;
    public int defensePoints;
    public int idPlayer;
    public string iconCard;
    // c'est pour  market
    public int prix;
    private int currentZonePlayerId = -1; // -1  la carte n'est dans aucune zone
    private static Dictionary<Rect, Card> cartesEnModeDefenseParZone = new Dictionary<Rect, Card>();






    private Vector2 topLeftPlayer1 = new Vector2(-5.0f, -5.0f);
    private Vector2 topRightPlayer1 = new Vector2(5.0f, -5.0f);
    private Vector2 bottomLeftPlayer1 = new Vector2(-5.0f, -10.0f);
    private Vector2 bottomRightPlayer1 = new Vector2(5.0f, -6.27f);




    private Vector2 topLeftPlayer2 = new Vector2(-10.0f, -5.0f);
    private Vector2 topRightPlayer2 = new Vector2(-4.3f, 2.68f);
    private Vector2 bottomLeftPlayer2 = new Vector2(-10.0f, -10.0f);
    private Vector2 bottomRightPlayer2 = new Vector2(-4.3f, -6.27f);




    private Vector2 topLeftPlayer3 = new Vector2(-5.0f, 0.0f);
    private Vector2 topRightPlayer3 = new Vector2(5.0f, 0.0f);
    private Vector2 bottomLeftPlayer3 = new Vector2(-5.0f, -5.0f);
    private Vector2 bottomRightPlayer3 = new Vector2(5.0f, -5.0f);


    private Vector2 topLeftPlayer4 = new Vector2(5.50f, -5.0f);
    private Vector2 topRightPlayer4 = new Vector2(11.0f, 2.68f);
    private Vector2 bottomLeftPlayer4 = new Vector2(5.50f, -10.0f);
    private Vector2 bottomRightPlayer4 = new Vector2(11.0f, -6.27f);




    private Rect playerZone1;
    private Rect playerZone2;
    private Rect playerZone3;
    private Rect playerZone4;

    private bool initializedNombreDeCartesZone = false;


    void Start()
    {
        Program = GameObject.Find("Program").GetComponent<Program>();

    }


    public void InitializeCard(int attackPoints, int defensePoints)
    {
        // Récupère la référence à l'objet auquel ce script est attaché
        this.id = UnityEngine.Random.Range(0, 1000000);
        // Utilise gameObject.AddComponent pour ajouter des composants au GameObject
        this.attackPoints = attackPoints;
        this.defensePoints = defensePoints;

        this.rend = gameObject.GetComponent<Renderer>();

        // Stocke une référence au BoxCollider
        this.collid = gameObject.GetComponent<BoxCollider>();

        // this.collid.size = new Vector2(transform.localScale.x + boxScaleMargeX, transform.localScale.y + boxScaleFactorY);

    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (!onInteraction)
        {
            if (currentSceneName == "TableScene")
            {
                CheckAndAdjustPositionTable();
            }
            else
            {
                CheckAndAdjustPositionPlayer();
            }
        }
    }

    void CheckAndAdjustPositionTable()
    {
        if (this.idPlayer == 1 || this.idPlayer == 3)
        {
            // Check if the card is in collision with another card
            Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
            // Debug.Log("hitColliders" + hitColliders.Length);

            // Sort the colliders by their horizontal positions
            System.Array.Sort(hitColliders, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                {
                    Debug.Log(gameObject.name + " collides with " + hitCollider.gameObject.name);
                    // Determine the direction based on relative horizontal positions
                    float direction = (hitCollider.transform.position.x < transform.position.x) ? 1f : -1f;

                    // Slowly move the card to the right or left based on the direction
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + (0.1f * direction), transform.position.y, transform.position.z), 0.1f);
                }
            }
        }
        else if (this.idPlayer == 2 || this.idPlayer == 4)
        {
            // Check if the card is in collision with another card
            Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
            // Debug.Log("hitColliders" + hitColliders.Length);

            // Sort the colliders by their vertical positions
            System.Array.Sort(hitColliders, (a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                {
                    Debug.Log(gameObject.name + " collides with " + hitCollider.gameObject.name);
                    // Determine the direction based on relative vertical positions
                    float direction = (hitCollider.transform.position.y < transform.position.y) ? 1f : -1f;

                    // Slowly move the card to the top or bottom based on the direction
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + (0.1f * direction), transform.position.z), 0.1f);
                }
            }

        }
    }

    void CheckAndAdjustPositionPlayer()
    {
        // Check if the card is in collision with another card
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        // Debug.Log("hitColliders" + hitColliders.Length);

        // Sort the colliders by their horizontal positions
        System.Array.Sort(hitColliders, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                // Determine the direction based on relative horizontal positions
                float direction = (hitCollider.transform.position.x < transform.position.x) ? 1f : -1f;

                // Slowly move the card to the right or left based on the direction
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + (0.1f * direction), transform.position.y, transform.position.z), 0.1f);
            }
        }
    }


    void TaskOnClik()
    {
        Debug.Log("TaskOnClik");
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "TableScene")
        {
            Rect zone = ObtenirZonePourLeJoueur(this.idPlayer);
            Debug.Log("TableManager found: " + tableManager.gameObject.name);
            if (PeutOrienterEnDefense(zone))
            {
                if (this.idPlayer == 1)
                {

                    Debug.Log("this.transform.rotation.z" + this.transform.rotation.z);
                    if (this.transform.rotation.z == 0)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, -90);
                        cartesEnModeDefenseParZone[zone] = this;
                    }
                    else
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 0);
                        cartesEnModeDefenseParZone.Remove(zone);
                    }
                }
                else if (this.idPlayer == 2)
                {
                    Debug.Log("this.transform.rotation.z" + this.transform.rotation.z);
                    if (this.transform.rotation.z == 0)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, -90);
                        cartesEnModeDefenseParZone[zone] = this;
                    }
                    else
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 0);
                        cartesEnModeDefenseParZone.Remove(zone);
                    }
                }
                else if (this.idPlayer == 3)
                {
                    Debug.Log("this.transform.rotation.z" + this.transform.rotation.z);
                    float currentZAngle = this.transform.rotation.eulerAngles.z;
                    float tolerance = 1.0f;
                    if (Mathf.Abs(currentZAngle - 180) < tolerance)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 90);
                        cartesEnModeDefenseParZone[zone] = this;
                    }
                    else
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, -180);
                        cartesEnModeDefenseParZone.Remove(zone);
                    }
                }
                else if (this.idPlayer == 4)
                {
                    Debug.Log("this.transform.rotation.z" + this.transform.rotation.z);
                    if (this.transform.rotation.z == 0)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 90);
                        cartesEnModeDefenseParZone[zone] = this;
                    }
                    else
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 0);
                        cartesEnModeDefenseParZone.Remove(zone);
                    }
                }
            }
            else
            {
                Debug.Log("Carte ne peut pas etre orienter en mode defense");
            }
        }
        else
        {
            if (this.idPlayer == 1)
            {
                if (GameManager.Instance.GetZoneIncarte1() < 2)
                {
                    owner.SupprimerCartefromlistCards(this.id);
                    Debug.Log("Carte peut etre poser");
                    GameManager.Instance.IncrementZoneIncarte1();
                    CardMessage messageObject = new CardMessage(this.id, this.GetType().Name, attackPoints, defensePoints, this.iconCard);
                    string message = JsonUtility.ToJson(messageObject);
                    owner.SendMessageToTAble(message);
                    owner.DestroyCard(this.id);
                    if (owner.CompteCarte() == 0)
                    {
                        owner.CreateCardPlayer(this.idPlayer);
                    }
                }
                else
                {
                    Debug.Log("Carte ne peut pas etre poser");
                }
            }
            else if (this.idPlayer == 2)
            {
                if (GameManager.Instance.GetZoneIncarte2() < 2)
                {
                    owner.SupprimerCartefromlistCards(this.id);
                    Debug.Log("Carte peut etre poser");
                    GameManager.Instance.IncrementZoneIncarte2();
                    CardMessage messageObject = new CardMessage(this.id, this.GetType().Name, attackPoints, defensePoints, this.iconCard);
                    string message = JsonUtility.ToJson(messageObject);
                    owner.SendMessageToTAble(message);
                    owner.DestroyCard(this.id);
                    // nombreDeCartesParZone[Zone1Id]++;
                    if (owner.CompteCarte() == 0)
                    {
                        owner.CreateCardPlayer(this.idPlayer);
                    }

                }
                else
                {
                    Debug.Log("Carte ne peut pas etre poser");
                }
            }
            else if (this.idPlayer == 3)
            {
                if (GameManager.Instance.GetZoneIncarte3() < 2)
                {
                    owner.SupprimerCartefromlistCards(this.id);
                    Debug.Log("Carte peut etre poser");
                    GameManager.Instance.IncrementZoneIncarte3();
                    CardMessage messageObject = new CardMessage(this.id, this.GetType().Name, attackPoints, defensePoints, this.iconCard);
                    string message = JsonUtility.ToJson(messageObject);
                    owner.SendMessageToTAble(message);
                    owner.DestroyCard(this.id);
                    // nombreDeCartesParZone[Zone3Id]++;
                    if (owner.CompteCarte() == 0)
                    {
                        owner.CreateCardPlayer(this.idPlayer);
                    }


                }
                else
                {
                    Debug.Log("Carte ne peut pas etre poser");
                }
            }
            else if (this.idPlayer == 4)
            {
                if (GameManager.Instance.GetZoneIncarte4() < 2)
                {
                    owner.SupprimerCartefromlistCards(this.id);
                    Debug.Log("Carte peut etre poser");
                    GameManager.Instance.IncrementZoneIncarte4();
                    CardMessage messageObject = new CardMessage(this.id, this.GetType().Name, attackPoints, defensePoints, this.iconCard);
                    string message = JsonUtility.ToJson(messageObject);
                    owner.SendMessageToTAble(message);
                    owner.DestroyCard(this.id);
                    // nombreDeCartesParZone[Zone4Id]++;
                    if (owner.CompteCarte() == 0)
                    {
                        owner.CreateCardPlayer(this.idPlayer);
                    }

                }
                else
                {
                    Debug.Log("Carte ne peut pas etre poser");
                }
            }

        }

    }



    bool PeutOrienterEnDefense(Rect zone)
    {
        // Vérifiez si une carte dans cette zone est déjà en mode défense
        return !cartesEnModeDefenseParZone.ContainsKey(zone) || cartesEnModeDefenseParZone[zone] == this;
    }

    Rect ObtenirZonePourLeJoueur(int idJoueur)
    {
        // Logique pour déterminer la zone basée sur l'idJoueur
        // Par exemple :
        switch (idJoueur)
        {
            case 1:
                return playerZone1;
            case 2:
                return playerZone2;
            case 3:
                return playerZone3;
            case 4:
                return playerZone4;
        }
        return default(Rect);
    }

    void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");

        onInteraction = true;
        mouseDownTime = Time.time;

        // Change color to red when clicked
        rend.material.color = Color.red;
        // Calculez l'offset entre la position initiale et la position actuelle du clic
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }



    void OnDrawGizmost1()
    {
        Vector3 bottomLeft = new Vector3(playerZone1.x, playerZone1.y, 0);
        Vector3 topLeft = new Vector3(playerZone1.x, playerZone1.y + playerZone1.height, 0);
        Vector3 topRight = new Vector3(playerZone1.x + playerZone1.width, playerZone1.y + playerZone1.height, 0);
        Vector3 bottomRight = new Vector3(playerZone1.x + playerZone1.width, playerZone1.y, 0);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.Log("playerZone1" + playerZone1);
    }

    void OnDrawGizmost2()
    {
        Vector3 bottomLeft = new Vector3(playerZone2.x, playerZone2.y, 0);
        Vector3 topLeft = new Vector3(playerZone2.x, playerZone2.y + playerZone2.height, 0);
        Vector3 topRight = new Vector3(playerZone2.x + playerZone2.width, playerZone2.y + playerZone2.height, 0);
        Vector3 bottomRight = new Vector3(playerZone2.x + playerZone2.width, playerZone2.y, 0);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.Log("playerZone2" + playerZone2);
    }

    void OnDrawGizmost3()
    {
        Vector3 bottomLeft = new Vector3(playerZone3.x, playerZone3.y, 0);
        Vector3 topLeft = new Vector3(playerZone3.x, playerZone3.y + playerZone3.height, 0);
        Vector3 topRight = new Vector3(playerZone3.x + playerZone3.width, playerZone3.y + playerZone3.height, 0);
        Vector3 bottomRight = new Vector3(playerZone3.x + playerZone3.width, playerZone3.y, 0);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.Log("playerZone3" + playerZone3);
    }

    void OnDrawGizmost4()
    {
        Vector3 bottomLeft = new Vector3(playerZone4.x, playerZone4.y, 0);
        Vector3 topLeft = new Vector3(playerZone4.x, playerZone4.y + playerZone4.height, 0);
        Vector3 topRight = new Vector3(playerZone4.x + playerZone4.width, playerZone4.y + playerZone4.height, 0);
        Vector3 bottomRight = new Vector3(playerZone4.x + playerZone4.width, playerZone4.y, 0);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.Log("playerZone4" + playerZone4);
    }





    void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
        onInteraction = false;
        rend.material.color = this.color;
        float clickDuration = Time.time - mouseDownTime;

        if (SceneManager.GetActiveScene().name == "TableScene")
        {
            tableManager = GameObject.Find("GameObject").GetComponent<TableManager>();
            if (tableManager == null)
            {
                Debug.LogError("TableManager not found in the scene!");
            }
            else if (this.idPlayer == -1)
            {
                Debug.Log("Carte Pour le marché");
                tableManager.getCurrentPlayer();
                int destinationMessage = tableManager.getCurrentPlayer();
                this.idPlayer = destinationMessage;
                this.prix = 0;
                string type = "cartePourLeMarche";
                CardMarket messageObject = new CardMarket(type, this, this.idPlayer);
                UnityEngine.Object.Destroy(this.gameObject);
                string message = JsonUtility.ToJson(messageObject);
                tableManager.SendMessageToPlayer(message);
            }
            else
            {

                playerZone1 = new Rect(-5.0f, -5.0f, 10.0f, 5.0f);
                playerZone2 = new Rect(-10.5f, -5.0f, 5.0f, 10.0f);
                playerZone3 = new Rect(-5.0f, 0.0f, 10.0f, 5.0f);
                playerZone4 = new Rect(5.5f, -5.0f, 5.0f, 10.0f);

                OnDrawGizmost1();
                OnDrawGizmost2();
                OnDrawGizmost3();
                OnDrawGizmost4();

                CheckZonesForCards();
                Vector2 cardPosition2D = new Vector2(this.transform.position.x, this.transform.position.y);
                int attackPoints = 0;
                if (playerZone1.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 1 && this.idPlayer != 3)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        //attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone1, this.idPlayer);
                        attackPoints = PointDeffenceCardsInZonetest(playerZone1, this.idPlayer, this.attackPoints);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 1, attackPoints);
                        UnityEngine.Object.Destroy(this.gameObject);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone2.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 2 && this.idPlayer != 4)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        //attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone2, this.idPlayer);
                        attackPoints = PointDeffenceCardsInZonetest(playerZone2, this.idPlayer, this.attackPoints);

                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 2, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        UnityEngine.Object.Destroy(this.gameObject);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone3.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 1 && this.idPlayer != 3)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        //attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone3, this.idPlayer);
                        attackPoints = PointDeffenceCardsInZonetest(playerZone3, this.idPlayer, this.attackPoints);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 3, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        UnityEngine.Object.Destroy(this.gameObject);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone4.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 2 && this.idPlayer != 4)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        //attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone4, this.idPlayer);
                        attackPoints = PointDeffenceCardsInZonetest(playerZone4, this.idPlayer, this.attackPoints);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 4, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        UnityEngine.Object.Destroy(this.gameObject);
                        tableManager.NextPlayerTurn();
                    }
                }
                else
                {
                    Debug.Log("Carte levée dans aucune zone");
                }

                //tableManager.UpdatePositionCards(this.idPlayer);
            }
        }

        if (clickDuration < this.clickDuration)
        {
            TaskOnClik();
        }
    }

    public void SetCurrentZone(int playerId)
    {
        this.currentZonePlayerId = playerId;
    }

    void OnMouseDrag()
    {
        tableManager = GameObject.Find("GameObject").GetComponent<TableManager>();

        if (!tableManager)
        {
            Debug.LogError("TableManager component not found on the GameObject!");
        }
        else
        {
            if (tableManager.currentPlayer == this.idPlayer)
            {
                // Move the card while preserving the offset
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
                transform.position = objPosition;
            }

        }

        // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        // Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
        // transform.position = objPosition;

    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(0.3f);
        // Move slowly the card to y=0
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.1f);
    }


    public int CountCardsInZone(Rect zone)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int count = 0;
        int pDeffence = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos))
            {
                count++;
                pDeffence += card.defensePoints;
            }
        }

        return count; // Retourne le nombre de cartes dans la zone
    }

    // count card in zone with an angle different to angle of initial card
    public int CountCardsInZonePositiondeffence(Rect zone, float angle)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int count = 0;
        int pDeffence = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos) && card.transform.rotation.z != angle)
            {
                count++;
                pDeffence += card.defensePoints;
            }
        }

        return count; // Retourne le nombre de cartes dans la zone
    }

    // Vous pouvez appeler cette fonction pour chaque zone pour obtenir le compte
    public void CheckZonesForCards()
    {
        int cardsInZone1 = CountCardsInZone(playerZone1);
        int cardsInZone2 = CountCardsInZone(playerZone2);
        int cardsInZone3 = CountCardsInZone(playerZone3);
        int cardsInZone4 = CountCardsInZone(playerZone4);

        // Logique ou actions basées sur le nombre de cartes dans chaque zone
        Debug.Log("Zone 1 a " + cardsInZone1 + " cartes.");
        Debug.Log("Zone 2 a " + cardsInZone2 + " cartes.");
        Debug.Log("Zone 3 a " + cardsInZone3 + " cartes.");
        Debug.Log("Zone 4 a " + cardsInZone4 + " cartes.");
    }

    public int PointDeffenceCardsInZone(Rect zone, int idPlayer)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int pDeffence = 0;
        int pAttack = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos) && card.idPlayer != idPlayer)
            {
                pDeffence += card.defensePoints;
            }
            if (card.idPlayer == idPlayer)
            {
                pAttack += card.attackPoints;
            }
        }

        // suppriemr toutes les cartes dans la zone sauf la carte qui attaque si elle est plus forte que la carte deffence
        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (pAttack > pDeffence && zone.Contains(cardPos) && card.idPlayer != idPlayer)
            {
                Debug.Log("pAttack" + pAttack);
                Debug.Log("pDeffence" + pDeffence);
                Debug.Log("card dertroyed voir le paln ");
                // how to destroy card;
                UnityEngine.Object.Destroy(card.gameObject);
            }
        }

        return pDeffence; // Retourne le nombre de cartes dans la zone
    }

    public int PointDeffenceCardsInZonetest(Rect zone, int idPlayer, int attackPoints)
    {

        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        //int pDeffence = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            // Get the current Z angle and normalize it to the range [0, 360)
            float currentZAngle = card.transform.eulerAngles.z % 360;

            if (zone.Contains(cardPos) && card.idPlayer != idPlayer)
            {
                if (zone == playerZone1 && !Mathf.Approximately(currentZAngle, 0))
                {
                    //pDeffence += card.defensePoints;
                    if (attackPoints >= card.defensePoints)
                    {
                        //GameManager.Instance.DecrementZoneIncarte1();
                        UnityEngine.Object.Destroy(card.gameObject);
                        attackPoints -= card.defensePoints;

                        //SendMessagetoplayer 1  use websocket
                        InfosMessage messageObject = new InfosMessage("carteDetruite", 1);
                        string message = JsonUtility.ToJson(messageObject);
                        tableManager.SendMessageToPlayer(message);
                        // nombreDeCartesParZone[Zone1Id]--;
                    }
                    else
                    {
                        card.defensePoints -= attackPoints;
                        return 0;
                    }
                }
                else if (zone == playerZone2 && Mathf.Approximately(currentZAngle, 0))
                {
                    //pDeffence += card.defensePoints;
                    if (attackPoints >= card.defensePoints)
                    {

                        UnityEngine.Object.Destroy(card.gameObject);
                        attackPoints -= card.defensePoints;
                        //GameManager.Instance.DecrementZoneIncarte2();
                        InfosMessage messageObject = new InfosMessage("carteDetruite", 2);
                        string message = JsonUtility.ToJson(messageObject);
                        tableManager.SendMessageToPlayer(message);

                        // nombreDeCartesParZone[Zone2Id]--;
                    }
                    else
                    {
                        card.defensePoints -= attackPoints;
                        return 0;
                    }
                }
                else if (zone == playerZone3 && Mathf.Approximately(currentZAngle, 90))
                {
                    //pDeffence += card.defensePoints;
                    if (attackPoints >= card.defensePoints)
                    {
                        //GameManager.Instance.DecrementZoneIncarte3();
                        UnityEngine.Object.Destroy(card.gameObject);
                        attackPoints -= card.defensePoints;
                        // nombreDeCartesParZone[Zone3Id]--;
                        InfosMessage messageObject = new InfosMessage("carteDetruite", 3);
                        string message = JsonUtility.ToJson(messageObject);
                        card.owner.SendMessageToPlayer(message);

                    }
                    else
                    {
                        card.defensePoints -= attackPoints;
                        return 0;
                    }
                }
                else if (zone == playerZone4 && Mathf.Approximately(currentZAngle, 0))
                {
                    //pDeffence += card.defensePoints;
                    if (attackPoints >= card.defensePoints)
                    {
                        // GameManager.Instance.DecrementZoneIncarte4();
                        UnityEngine.Object.Destroy(card.gameObject);
                        attackPoints -= card.defensePoints;
                        // nombreDeCartesParZone[Zone4Id]--;   
                        InfosMessage messageObject = new InfosMessage("carteDetruite", 4);
                        string message = JsonUtility.ToJson(messageObject);
                        card.owner.SendMessageToPlayer(message);
                    }
                    else
                    {
                        card.defensePoints -= attackPoints;
                        return 0;
                    }

                }
            }
        }

        return attackPoints; // Retourne le nombre de cartes dans la zone
    }

    public int PlayerIDCarteInZone(Rect zone)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int idPlayer = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos))
            {
                idPlayer = card.idPlayer;
            }
        }

        return idPlayer; // Retourne le nombre de cartes dans la zone
    }

}