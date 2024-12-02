using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public UIDocument scrolls;
    public Button exitButton;
    public Label scrollText;
    public VisualElement scrollContainer;

    public UIDocument messages;
    private Label gameMessages;
    private VisualElement gameMessageContainer;
    public UIDocument doc;
    private Label interactionlabel;
    private Label crosshair;
    private Label batteryCounter;
    private Label manuscriptsCounter;
    private VisualElement batteryImage;

    public UIDocument pauseMenu;
    private Button resumeButton;
    private Button returnButton;

    public UIDocument map;
    
    private ProgressBar staminaBar;

    public Texture2D battery0;
    public Texture2D battery1;
    public Texture2D battery2;
    public Texture2D battery3;

    public static bool inScrollView;
    public static bool isPaused;
    public static bool isMapOpened;

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
        
        map.rootVisualElement.style.display = DisplayStyle.None;
        
        pauseMenu.rootVisualElement.style.display = DisplayStyle.None;
        resumeButton = pauseMenu.rootVisualElement.Q("ResumeButton") as Button;
        resumeButton.RegisterCallback<ClickEvent>(e =>
        {
            ResumeGame();
        });
        returnButton = pauseMenu.rootVisualElement.Q("MainMenuButton") as Button;
        returnButton.RegisterCallback<ClickEvent>(e =>
        {
            PerformGameExit();
        });

        scrolls.rootVisualElement.style.display = DisplayStyle.None;
        scrollText = scrolls.rootVisualElement.Q("ScrollText") as Label;
        exitButton = scrolls.rootVisualElement.Q("ExitButton") as Button;
        exitButton.RegisterCallback<ClickEvent>(evt =>
        {
            PerformScrollViewExit(); 
        });
        scrollContainer = scrolls.rootVisualElement.Q("ScrollContainer") as VisualElement;

        interactionlabel = doc.rootVisualElement.Q("InteractLabel") as Label;
        crosshair = doc.rootVisualElement.Q("CrosshairText") as Label;
        batteryCounter = doc.rootVisualElement.Q("BatteryCounter") as Label;
        manuscriptsCounter = doc.rootVisualElement.Q("ManuscriptCounter") as Label;
        batteryImage = doc.rootVisualElement.Q("Battery") as VisualElement;
        staminaBar = doc.rootVisualElement.Q("StaminaBar") as ProgressBar;

        interactionlabel.text = "";
        staminaBar.value = 100;
        crosshair.text = CROSSHAIR_CHAR;
        batteryCounter.text = "x01";
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
        if (ProgressGlobal.Instance.FoundManuscriptId(id))
        {
            interactionlabel.text = $"(E) Place manuscript";
        }
        else 
        {
            interactionlabel.text = $"Missing manuscript {id}";
        }

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
        if (isPaused) return;
        if ((lookingatitem && Input.GetKeyDown(KeyCode.E)) && !isMapOpened) 
        {
            lookatObject.Collect();
        }
        
        if ((lookingathousedoor && Input.GetKeyDown(KeyCode.E)) && !isMapOpened) 
        {
            lookatHouseDoor.Interact();
        }

        if ((lookingataltar && Input.GetKeyDown(KeyCode.E)) && !isMapOpened) 
        {
            lookatAltar.Interact();
        }

        if (Input.GetKeyDown(KeyCode.M) && !inScrollView)
        {
            ToggleMap();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
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

    public void ToggleMap()
    {
        PlayerSounds.Instance.PlayBookTake();
        if (isMapOpened)
        {
            //Map quit
            map.rootVisualElement.style.display = DisplayStyle.None;
            Time.timeScale = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Map open
            map.rootVisualElement.style.display = DisplayStyle.Flex;
            Time.timeScale = 0;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        isMapOpened = !isMapOpened;
    }

    public void PerformScrollViewExit()
    {
        Time.timeScale = 1;
        inScrollView = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        doc.rootVisualElement.style.display = DisplayStyle.Flex;
        scrolls.rootVisualElement.style.display = DisplayStyle.None;

        scrollText.text = "";

        PlayerSounds.Instance.PlayBookClose();

    }

    public void PerformGameExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    public void ResumeGame()
    {
        if (!inScrollView && !isMapOpened)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        pauseMenu.rootVisualElement.style.display = DisplayStyle.None;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        isPaused = true;
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void PerformScrollViewEnter(int id)
    {
        Time.timeScale = 0;
        inScrollView = true;

        UnityEngine.Cursor.lockState = CursorLockMode.None;

        doc.rootVisualElement.style.display = DisplayStyle.None;
        scrolls.rootVisualElement.style.display = DisplayStyle.Flex;        

        scrollText.text = ProgressGlobal.Instance.GetScrollContent(id);
    }

    public void Blackout()
    {
        StartCoroutine(IncreaseAlpha(1.0f, 0.2f));
        doc.rootVisualElement.style.display = DisplayStyle.None;
    }

    public bool blockInputs => inScrollView || isPaused || isMapOpened;



}
