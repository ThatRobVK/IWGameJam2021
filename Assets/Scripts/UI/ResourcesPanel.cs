using UnityEngine;
using UnityEngine.UI;

namespace FDaaGF.UI
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField]
        private Text[] resourceAmountTexts;

        public void UpdateUI(int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                resourceAmountTexts[i].text = values[i].ToString();
            }
        }
    }
}