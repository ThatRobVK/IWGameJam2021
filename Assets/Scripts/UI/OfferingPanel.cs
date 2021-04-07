using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class OfferingPanel : NetworkBehaviour
    {
        // Event definitions
        public event Action<NetworkConnectionToClient, int> OnOfferingConfirmed;


        // Editor fields
        [SerializeField]
        private GameObject InputPanel;
        [SerializeField]
        private GameObject WaitPanel;
        [SerializeField]
        private InputField offeringInputField;
        [SerializeField]
        private Text resourceText;

        private int maxOffer = -1;


        // Called when the Confirm button is clicked - runs on the client the event happens on
        public void HandleConfirmButtonClick()
        {
            // Parse the entered value, if valid int raise event to let watchers know of input
            int offeringValue = 0;
            if (int.TryParse(offeringInputField.text, out offeringValue))
            {
                if (offeringValue >= 0 && offeringValue <= maxOffer)
                {
                    // Get the server to notify listeners
                    CmdRaiseOfferingConfirmed(offeringValue);
                }
                else
                {
                    Debug.Log("Invalid offer");
                }
            }
        }

        // Clears and hides the panel - called on all clients
        [TargetRpc]
        public void RpcHide(NetworkConnection target)
        {
            InputPanel.SetActive(false);
            WaitPanel.SetActive(true);
            offeringInputField.text = string.Empty;
        }

        [ClientRpc]
        public void RpcHideAll()
        {
            InputPanel.SetActive(true);
            WaitPanel.SetActive(false);
            gameObject.SetActive(false);
            offeringInputField.text = string.Empty;
        }

        // Shows the panel - called on all clients
        [TargetRpc]
        public void RpcShow(NetworkConnection target, ResourceType resourceType, int maxOffer)
        {
            gameObject.SetActive(true);
            InputPanel.SetActive(true);
            WaitPanel.SetActive(false);
            resourceText.text = resourceType.ToString();
            this.maxOffer = maxOffer;
        }

        // Notify listeners that an offering has been sent - called on the server, can be called by any client
        [Command(requiresAuthority = false)]
        private void CmdRaiseOfferingConfirmed(int offeringValue, NetworkConnectionToClient sender = null)
        {
            OnOfferingConfirmed?.Invoke(sender, offeringValue);
        }
    }
}