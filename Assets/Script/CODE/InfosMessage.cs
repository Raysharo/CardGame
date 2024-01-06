
using System;
using UnityEngine;
[System.Serializable]
public class InfosMessage
{
    public string  type;
    public int idPlayer;

    public InfosMessage(string type, int idPlayer)
    {
        this.type = type;
        this.idPlayer = idPlayer;
    }
}
