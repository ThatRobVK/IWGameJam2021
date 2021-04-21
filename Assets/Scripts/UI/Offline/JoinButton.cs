using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class JoinButton : MonoBehaviour
{
    [SerializeField]
    private InputField hostText;

    public void HandleButtonClick()
    {
        NetworkManager.singleton.networkAddress = hostText.text;
        NetworkManager.singleton.StartClient();
    }
}
