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
        // Working around a weird bug where on different size windows the panels disappear
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -87.5f);

        if (isServer)
        {
            // On the server, set the syncvars
            currentTurn = gameState.Turn;
            currentResource = (int)gameState.ResourceRequirements[currentTurn - 1];
            nextResource = (int)gameState.ResourceRequirements[currentTurn];

            Debug.LogFormat("Server START set vars turn {0}, current {1}, next {2}", currentTurn, currentResource, nextResource);
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

            Debug.LogFormat("Server UPDATE set vars turn {0}, current {1}, next {2}", currentTurn, currentResource, nextResource);
        }

        // Update the images
        thisTurnText.text = currentTurn.ToString();
        thisTurnImage.texture = resourceTextures[currentResource];
        nextTurnImage.texture = resourceTextures[nextResource];
    }
}
