using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Text readyText;

    public string Name;
    public Sprite Image;

    private string NotReadyText = "(not ready)";
    private string ReadyText = "(ready)";

    public void SetPlayerDetails(string playerName, Sprite image, bool isReady)
    {
        Name = playerName;
        Image = image;

        playerNameText.text = Name;
        playerImage.sprite = Image;
        readyText.text = (isReady) ? ReadyText : NotReadyText;
    }
}
