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
        [SerializeField]
        private Image playerImage;
        [SerializeField]
        private AudioSource buttonClickSound;

        private int imageIndex = 0;

        void Awake()
        {
            // Remove all players on load
            gameState.Players.Clear();
        }

        void Update()
        {
            playerImage.sprite = gameState.PlayerImages[imageIndex];
        }

        void Start()
        {
            CmdRegisterUser();
        }

        public void HandleImageButtonUpClick()
        {
            // Increase, loop to 0 if out of bounds
            imageIndex++;
            if (imageIndex > gameState.PlayerImages.Length - 1) imageIndex = 0;
        }

        public void HandleImageButtonDownClick()
        {
            // Decrease, loop to last image if out of bounds
            imageIndex--;
            if (imageIndex < 0) imageIndex = gameState.PlayerImages.Length - 1;
        }


        public void HandleNameButtonClick()
        {
            string username = nameInputField.text;
            CmdSetUsername(username, imageIndex);

            buttonClickSound.Play();

            // Flag player as ready - but not for server, they have a "Start game" button
            if (!isServer)
            {
                NetworkClient.connection.identity.GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
            }

            gameObject.SetActive(false);
        }

        [Command(requiresAuthority = false)]
        private void CmdSetUsername(string name, int image, NetworkConnectionToClient sender = null)
        {
            // Set the name on the object where the connection id matches
            gameState.Players.Where(x => x.ConnectionId == sender.connectionId).First().Name = name;
            gameState.Players.Where(x => x.ConnectionId == sender.connectionId).First().Image = image;
        }

        [Command(requiresAuthority = false)]
        private void CmdRegisterUser(NetworkConnectionToClient sender = null)
        {
            var newPlayer = new Player(sender, string.Format("Player {0}", gameState.Players.Count));

            gameState.Players.Add(newPlayer);

            RpcSetName(sender, newPlayer.Name);
        }

        [TargetRpc]
        private void RpcSetName(NetworkConnection target, string name)
        {
            nameInputField.text = name;
        }
    }
}