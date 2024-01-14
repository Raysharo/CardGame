[System.Serializable] // Assurez-vous d'ajouter ceci pour permettre la sérialisation
public class CardMarket {
    public int id;
    public int idPlayer;
    public int attackPoints;
    public int defensePoints;
    public string iconCard;
     public string type;
    public string typeCard;

    public CardMarket(int id, int idPlayer, int attackPoints, int defensePoints, string iconCard, string type, string typeCard) {
        this.id = id;
        this.idPlayer = idPlayer;
        this.attackPoints = attackPoints;
        this.defensePoints = defensePoints;
        this.iconCard = iconCard;
        this.typeCard = typeCard;
        this.type = type;
    }
}