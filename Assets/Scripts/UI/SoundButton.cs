using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField]
    private Sprite soundOnIcon;
    [SerializeField]
    private Sprite soundOffIcon;
    [SerializeField]
    private Image buttonImage;

    private AudioSource[] audioSources;
    private bool audioEnabled = true;

    void Awake()
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset position - it changes on clients for some weird reason
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-98.69995f, 25.09998f);

        buttonImage.sprite = (audioEnabled) ? soundOnIcon : soundOffIcon;
    }

    public void HandleButtonClick()
    {
        audioEnabled = !audioEnabled;

        foreach (var audioSource in audioSources)
        {
            audioSource.enabled = audioEnabled;
        }
    }
}
