using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static string CROSSHAIR_CHAR = "•";

    public static bool messaging;
    public static bool lookingat;

    public static Collectible lookatObject;


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

    public void ShowInteraction(string tag)
    {
        // Add interact with (e)
        interactionlabel.text = "(E) Pick up";
        lookingat = true;

        switch (tag)
        {
            case "Battery":
            
            break;

            case "Artifact":

            break;
        }
    }

    public void SetLookatCollectible(Collectible collectible) 
    {
        lookatObject = collectible;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!messaging) 
            {
                messaging = true;
                StartCoroutine(GameMessage("Hello, World!\nI hate working with Unity..."));
            }
        }

        if (lookingat && Input.GetKeyDown(KeyCode.E)) 
        {
            lookatObject.Collect();
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
        lookingat = false;
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



IEnumerator GameMessage(string message) 
{
    yield return StartCoroutine(IncreaseAlpha(0.7f, 1.0f));
    yield return StartCoroutine(WriteMessage(message));
    yield return new WaitForSeconds(1.0f);

    gameMessages.text = "";

    yield return StartCoroutine(IncreaseAlpha(0.0f, 1.0f));

    messaging = false;

}

IEnumerator IncreaseAlpha(float targetAlpha, float duration)
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


IEnumerator WriteMessage(string message)
    {
        gameMessages.text = "";
        int messageLength = message.Length;

        for (int i = 0; i < messageLength; i++)
        {
            gameMessages.text += message[i];

            yield return new WaitForSeconds(0.1f);
        }
    }

    

}
