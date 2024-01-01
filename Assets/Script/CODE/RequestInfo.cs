[System.Serializable]
public class RequestInfo
{
    public string action;
    public int requestingPlayerId;
    public int targetPlayerId;

    public RequestInfo(string action, int requestingPlayerId, int targetPlayerId)
    {
        this.action = action;
        this.requestingPlayerId = requestingPlayerId;
        this.targetPlayerId = targetPlayerId;
    }
}