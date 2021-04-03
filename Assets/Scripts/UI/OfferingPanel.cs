using System;
using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class OfferingPanel : MonoBehaviour
    {
        public event Action<int> OnOfferingConfirmed;

        public InputField OfferingInputField;

        public void HandleConfirmButtonClick()
        {
            int offeringValue = 0;

            if (int.TryParse(OfferingInputField.text, out offeringValue))
            {
                // If valid int entered, raise event
                OnOfferingConfirmed?.Invoke(offeringValue);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OfferingInputField.text = string.Empty;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}