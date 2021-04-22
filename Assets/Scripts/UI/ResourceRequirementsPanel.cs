using FDaaGF;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ResourceRequirementsPanel : NetworkBehaviour
{
    [SerializeField]
    private Text thisTurnText;
    [SerializeField]
    private RawImage thisTurnImage;
    [SerializeField]
    private RawImage nextTurnImage;
    [SerializeField]
    private Texture2D[] resourceTextures;
    [SerializeField]
    private GameState gameState;

    [SyncVar]
    private int currentTurn = -1;
    [SyncVar]
    private int currentResource = -1;
    [SyncVar]
    private int nextResource = -1;


    void Start()
    {
        if (isServer)
        {
            // On the server, set the syncvars
            currentTurn = gameState.Turn;
            currentResource = (int)gameState.ResourceRequirements[currentTurn - 1];
            nextResource = (int)gameState.ResourceRequirements[currentTurn];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer && gameState.Turn != currentTurn)
        {
            // On the server, set the syncvars
            currentTurn = gameState.Turn;
            currentResource = ((currentTurn - 1 < gameState.ResourceRequirements.Count)) ? (int)gameState.ResourceRequirements[currentTurn - 1] : 0;
            nextResource = ((currentTurn < gameState.ResourceRequirements.Count)) ? (int)gameState.ResourceRequirements[currentTurn] : 0;
        }

        // Update the images
        thisTurnText.text = currentTurn.ToString();
        thisTurnImage.texture = resourceTextures[currentResource];
        nextTurnImage.texture = resourceTextures[nextResource];

        // Reset position - it changes on clients for some weird reason
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-166.7998f, -100f);
    }
}
