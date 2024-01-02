[System.Serializable] // Assurez-vous d'ajouter ceci pour permettre la sérialisation
public class CardMessage {
    public string type = "move card to player";
    public int cardId; 
    public string cardType;
    public int attackPoints;
    public int defensePoints;
    public int playerId;
    public string iconCard;

    public CardMessage(int id, string type, int attack, int defense, string iconCard) {
        this.cardId = id;
        this.cardType = type;
        this.attackPoints = attack;
        this.defensePoints = defense;
        this.playerId = GameManager.Instance.PlayerID;
        this.iconCard = iconCard;
    }
}