using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void OnPlayerButtonClicked(int playerID)
    {
        GameManager.Instance.LoadPlayerScene(playerID);
    }
}