[System.Serializable] // Assurez-vous d'ajouter ceci pour permettre la sérialisation
public class CardMarket {
    public string type;
    public Card card; 
    public int playerId;



    public CardMarket(string type, Card card, int playerId) {
        this.type = type;
        this.card = card;
        this.playerId = playerId;
    }
}