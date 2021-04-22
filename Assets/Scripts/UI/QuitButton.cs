using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class QuitButton : NetworkBehaviour
{
    void Update()
    {
        // Reset position - it changes on clients for some weird reason
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-42.3999f, 25.09998f);
    }

    public void HandleButtonClick()
    {
        if (isServer)
        {
            // Close server if running
            NetworkServer.Shutdown();
        }
        if (NetworkClient.isConnected)
        {
            // Disconnect if client
            NetworkClient.Disconnect();
        }

        Application.Quit();
    }
}
