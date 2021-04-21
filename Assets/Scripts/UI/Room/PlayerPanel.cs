using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    private Image playerImage;

    public string Name;
    public Sprite Image;

    public void SetPlayerDetails(string playerName, Sprite image)
    {
        Name = playerName;
        Image = image;

        playerNameText.text = Name;
        playerImage.sprite = Image;
    }
}
