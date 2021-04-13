using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI.Room
{
    public class PlayerNamePanel : NetworkBehaviour
    {
        // Editor fields
        [SerializeField]
        private GameState gameState;
        [SerializeField]
        private InputField nameInputField;

        void Awake()
        {
            // Remove all players on load
            gameState.Players.Clear();
        }

        void Start()
        {
            if (!isClient) return;

            CmdRegisterUser();
        }

        public void HandleNameButtonClick()
        {
            string username = nameInputField.text;
            CmdSetUsername(username);
        }

        [Command(requiresAuthority = false)]
        private void CmdSetUsername(string name, NetworkConnectionToClient sender = null)
        {
            // Set the name on the object where the connection id matches
            gameState.Players.Where(x => x.ConnectionId == sender.connectionId).First().Name = name;
        }

        [Command(requiresAuthority = false)]
        private void CmdRegisterUser(NetworkConnectionToClient sender = null)
        {
            var newPlayer = new Player(sender, string.Format("Player {0}", sender.connectionId));

            gameState.Players.Add(newPlayer);
        }
    }
}