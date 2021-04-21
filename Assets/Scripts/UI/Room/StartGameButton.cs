using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace FDaaGF.UI.Room
{
    public class StartGameButton : NetworkBehaviour
    {
        [SerializeField]
        private GameState gameState;

        // Update is called once per frame
        void Update()
        {
            if (isServer)
            {
                if (gameState.Players.Count > 1 && gameState.Players.Where(x => !x.IsReady).Count() == 1)
                {
                    GetComponent<CanvasGroup>().alpha = 1;
                }
            }
        }

        public void HandleButtonClick()
        {
            NetworkClient.connection.identity.GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
        }
    }
}