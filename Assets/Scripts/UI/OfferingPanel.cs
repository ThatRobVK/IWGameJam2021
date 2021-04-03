using System;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class OfferingPanel : MonoBehaviour
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
                OnOfferingConfirmed?.Invoke(offeringValue);
            }
        }

        // Clears and hides the panel
        public void Hide()
        {
            gameObject.SetActive(false);
            OfferingInputField.text = string.Empty;
        }

        // Shows the panel
        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}