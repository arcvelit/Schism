using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public static string CROSSHAIR_CHAR = "•";

    public static bool messaging;
    public static bool lookingatitem;
    public static bool lookingathousedoor;
    public static bool lookingataltar;

    public static Collectible lookatObject;
    public static HouseDoor lookatHouseDoor;
    public static Altar lookatAltar;

    public GameObject player;

    public UIDocument messages;
    private Label gameMessages;
    private VisualElement gameMessageContainer;
    public UIDocument doc;
    private Label interactionlabel;
    private Label crosshair;
    private Label batteryCounter;
    private Label manuscriptsCounter;
    private VisualElement batteryImage;

    private ProgressBar staminaBar;

    public Texture2D battery0;
    public Texture2D battery1;
    public Texture2D battery2;
    public Texture2D battery3;

    void Start()
    {

    }

    void Awake()
    {
        gameMessageContainer = messages.rootVisualElement.Q("Container") as VisualElement;   
        gameMessages =  messages.rootVisualElement.Q("MessageBox") as Label;
        gameMessages.text = "";

        Color newColor = new Color(0f, 0f, 0f, 0.0f);
        gameMessageContainer.style.backgroundColor = new StyleColor(newColor);


        interactionlabel = doc.rootVisualElement.Q("InteractLabel") as Label;
        crosshair = doc.rootVisualElement.Q("CrosshairText") as Label;
        batteryCounter = doc.rootVisualElement.Q("BatteryCounter") as Label;
        manuscriptsCounter = doc.rootVisualElement.Q("ManuscriptCounter") as Label;
        batteryImage = doc.rootVisualElement.Q("Battery") as VisualElement;
        staminaBar = doc.rootVisualElement.Q("StaminaBar") as ProgressBar;

        interactionlabel.text = "";
        staminaBar.value = 100;
        crosshair.text = CROSSHAIR_CHAR;
        batteryCounter.text = "x00";
        manuscriptsCounter.text = "00";

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowCollectibleInteraction()
    {
        interactionlabel.text = "(E) Pick up";
        lookingatitem = true;
    }

    public void ShowHouseDoorInteraction()
    {
        interactionlabel.text = $"(E) {(lookatHouseDoor.opened ? "Close" : "Open")} door";
        lookingathousedoor = true;
    }

    public void ShowAltarInteraction(int id)
    {
        interactionlabel.text = $"(E) Place manuscript {id}";
        lookingataltar = true;
    }

    public void SetLookatHouseDoor(HouseDoor door) 
    {
        lookatHouseDoor = door;
    }

    public void SetLookatCollectible(Collectible collectible) 
    {
        lookatObject = collectible;
    }

    public void SetLookatAltar(Altar altar)
    {
        lookatAltar = altar;
    }

    void Update() 
    {
        if (lookingatitem && Input.GetKeyDown(KeyCode.E)) 
        {
            lookatObject.Collect();
        }
        
        if (lookingathousedoor && Input.GetKeyDown(KeyCode.E)) 
        {
            lookatHouseDoor.Interact();
        }

        if (lookingataltar && Input.GetKeyDown(KeyCode.E)) 
        {
            lookatAltar.Interact();
        }
    }

    public void UpdateTorchLife(int percentage)
    {
        if (percentage > 66)
        {
            batteryImage.style.backgroundImage = new StyleBackground(battery3);
        }
        else if (percentage > 33)
        {
            batteryImage.style.backgroundImage = new StyleBackground(battery2);
        }
        else if (percentage > 0)
        {            
            batteryImage.style.backgroundImage = new StyleBackground(battery1);
        }
        else if (percentage <= 0)
        {
            batteryImage.style.backgroundImage = new StyleBackground(battery0);
        }
    }

    public void UpdateStaminaBar(int percentage)
    {
        staminaBar.value = percentage;
    }

    public void RemoveInteraction()
    {
        // Remove interact with (e)
        lookingatitem = false;
        lookingathousedoor = false;
        lookingataltar = false;
        interactionlabel.text = "";
    }

    public void UpdateBatteries(int batteries)
    {
        batteryCounter.text = batteries > 9 ? "x" + batteries.ToString() : "x0" + batteries.ToString();
    }

    public void UpdateManuscripts(int manuscripts)
    {
        manuscriptsCounter.text = manuscripts > 9 ? manuscripts.ToString() : "0" + manuscripts.ToString();
    }


public void ResetMessage()
{
    gameMessages.text = "";
}

public IEnumerator GameMessage(string message, float alpha, Movement player) 
{
    if (messaging) yield break;

    messaging = true;

    yield return StartCoroutine(IncreaseAlpha(alpha, 1.0f));
    yield return StartCoroutine(WriteMessage(message));
    yield return new WaitForSeconds(1.0f);

    if (player != null) player.BackToSpawn();

    ResetMessage();

    yield return StartCoroutine(IncreaseAlpha(0.0f, 1.0f));

    messaging = false;

}

public IEnumerator IncreaseAlpha(float targetAlpha, float duration)
    {
        Color initialColor = gameMessageContainer.resolvedStyle.backgroundColor;
        float startAlpha = initialColor.a;

        targetAlpha = Mathf.Clamp01(targetAlpha);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            gameMessageContainer.style.backgroundColor = new StyleColor(new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        gameMessageContainer.style.backgroundColor = new StyleColor(new Color(initialColor.r, initialColor.g, initialColor.b, targetAlpha));
    }


public IEnumerator WriteMessage(string message)
    {
        PlayerSounds.Instance.PlayTyping();
        gameMessages.text = "";
        int messageLength = message.Length;

        for (int i = 0; i < messageLength; i++)
        {
            gameMessages.text += message[i];

            yield return new WaitForSeconds(0.1f);
        }

        PlayerSounds.Instance.StopTyping();
    }

    

}
