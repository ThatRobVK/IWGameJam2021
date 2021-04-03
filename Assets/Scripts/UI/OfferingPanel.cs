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


        // Called when the Confirm button is clicked
        public void HandleConfirmButtonClick()
        {
            // Parse the entered value, if valid int raise event to let watchers know of input
            int offeringValue = 0;
            if (int.TryParse(OfferingInputField.text, out offeringValue))
            {
                CmdRaiseOfferingConfirmed(offeringValue);
            }
        }

        // Clears and hides the panel
        [ClientRpc]
        public void RpcHide()
        {
            gameObject.SetActive(false);
            OfferingInputField.text = string.Empty;
        }

        // Shows the panel
        [ClientRpc]
        public void RpcShow()
        {
            gameObject.SetActive(true);
        }

        [Command(requiresAuthority = false)]
        private void CmdRaiseOfferingConfirmed(int offeringValue, NetworkConnectionToClient sender = null)
        {
            Debug.LogFormat("Sender: {0} - Value {1}", sender.identity.netId, offeringValue);
            OnOfferingConfirmed?.Invoke(offeringValue);
        }
    }
}