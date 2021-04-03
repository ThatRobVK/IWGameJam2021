using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class OfferingPanel : NetworkBehaviour
    {
        // Event definitions
        public event Action<int> OnOfferingConfirmed;


        // Editor fields
        public InputField OfferingInputField;
        public Text ResourceText;


        // Called when the Confirm button is clicked - runs on the client the event happens on
        public void HandleConfirmButtonClick()
        {
            // Parse the entered value, if valid int raise event to let watchers know of input
            int offeringValue = 0;
            if (int.TryParse(OfferingInputField.text, out offeringValue))
            {
                // Get the server to notify listeners
                CmdRaiseOfferingConfirmed(offeringValue);
            }
        }

        // Clears and hides the panel - called on all clients
        [ClientRpc]
        public void RpcHide()
        {
            gameObject.SetActive(false);
            OfferingInputField.text = string.Empty;
        }

        // Shows the panel - called on all clients
        [ClientRpc]
        public void RpcShow(ResourceType resourceType)
        {
            gameObject.SetActive(true);
            ResourceText.text = resourceType.ToString();
        }

        // Notify listeners that an offering has been sent - called on the server, can be called by any client
        [Command(requiresAuthority = false)]
        private void CmdRaiseOfferingConfirmed(int offeringValue, NetworkConnectionToClient sender = null)
        {
            OnOfferingConfirmed?.Invoke(offeringValue);
        }
    }
}