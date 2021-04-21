using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class QuitButton : NetworkBehaviour
{
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
