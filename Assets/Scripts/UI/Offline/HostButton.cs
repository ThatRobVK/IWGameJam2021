using Mirror;
using UnityEngine;

public class HostButton : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(true);
    }
    public void HandleButtonClick()
    {
        NetworkManager.singleton.maxConnections = 4;
        NetworkManager.singleton.StartHost();
    }
}
