using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class OfferingPanel : NetworkBehaviour
    {
        // Event definitions
        public event Action<NetworkConnectionToClient, int, bool> OnOfferingConfirmed;


        // Editor fields
        [SerializeField]
        private GameObject InputPanel;
        [SerializeField]
        private GameObject WaitPanel;
        [SerializeField]
        private InputField offeringInputField;
        [SerializeField]
        private Toggle sacrificeToggle;
        [SerializeField]
        private Text resourceText;
        [SerializeField]
        private Image resourceImage;
        [SerializeField]
        private Sprite[] resourceSprites;
        [SerializeField]
        private Text totalOfferingText;
        [SerializeField]
        private string noSacrificeText = "Can't sacrifice, not enough workers!";
        [SerializeField]
        private string canSacrificeText = "Sacrifice a poor, innocent worker";
        [SerializeField]
        private AudioSource sacrificeSound;

        private int maxOffer = -1;
        private CanvasGroup canvasGroup;


        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            ShowHideCanvasGroup(false);
        }

        void Update()
        {
            var sacrificeAmount = (sacrificeToggle.isOn) ? 5 : 0;

            int offeringValue = (!string.IsNullOrEmpty(offeringInputField.text)) ? int.Parse(offeringInputField.text) : 0;
            if (offeringValue >= 0 && offeringValue <= maxOffer)
            {
                // Get the server to notify listeners
                totalOfferingText.text = (offeringValue + sacrificeAmount).ToString();
            }
        }


        // Called when the Confirm button is clicked - runs on the client the event happens on
        public void HandleConfirmButtonClick()
        {
            // Parse the entered value, if valid int raise event to let watchers know of input

            int offeringValue = (!string.IsNullOrEmpty(offeringInputField.text)) ? int.Parse(offeringInputField.text) : 0;
            if (offeringValue >= 0 && offeringValue <= maxOffer)
            {
                if (sacrificeToggle.isOn)
                {
                    // Play sacrifice sound effect
                    sacrificeSound?.Play();
                }

                // Get the server to notify listeners
                CmdRaiseOfferingConfirmed(offeringValue, sacrificeToggle.isOn);
            }
            else
            {
                Debug.Log("Invalid offer");
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

            ShowHideCanvasGroup(false);

            offeringInputField.text = string.Empty;
        }

        // Shows the panel - called on all clients
        [TargetRpc]
        public void RpcShow(NetworkConnection target, ResourceType resourceType, int maxOffer, bool canSacrifice)
        {
            ShowHideCanvasGroup(true);

            InputPanel.SetActive(true);
            WaitPanel.SetActive(false);
            resourceText.text = resourceType.ToString();
            resourceImage.sprite = resourceSprites[(int)resourceType];

            // Set sacrifice toggle
            sacrificeToggle.isOn = false;
            sacrificeToggle.interactable = canSacrifice;
            sacrificeToggle.GetComponentInChildren<Text>().text = (canSacrifice) ? canSacrificeText : noSacrificeText;

            this.maxOffer = maxOffer;
        }

        // Notify listeners that an offering has been sent - called on the server, can be called by any client
        [Command(requiresAuthority = false)]
        private void CmdRaiseOfferingConfirmed(int offeringValue, bool sacrifice, NetworkConnectionToClient sender = null)
        {
            OnOfferingConfirmed?.Invoke(sender, offeringValue, sacrifice);
        }

        private void ShowHideCanvasGroup(bool show)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (show) ? 1 : 0;
                canvasGroup.blocksRaycasts = show;
                canvasGroup.interactable = show;
            }
        }
    }
}