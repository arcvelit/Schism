using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static string CROSSHAIR_CHAR = "•";
    public static string SELECT_CHAR ="▣";


    public UIDocument doc;
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
        crosshair = doc.rootVisualElement.Q("CrosshairText") as Label;
        batteryCounter = doc.rootVisualElement.Q("BatteryCounter") as Label;
        manuscriptsCounter = doc.rootVisualElement.Q("ManuscriptCounter") as Label;
        batteryImage = doc.rootVisualElement.Q("Battery") as VisualElement;
        staminaBar = doc.rootVisualElement.Q("StaminaBar") as ProgressBar;

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
        crosshair.text = SELECT_CHAR;

        switch (tag)
        {
            case "Battery":
            
            break;

            case "Artifact":

            break;
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
        crosshair.text = CROSSHAIR_CHAR;
    }

    public void UpdateBatteries(int batteries)
    {
        batteryCounter.text = batteries > 9 ? "x" + batteries.ToString() : "x0" + batteries.ToString();
    }

    public void UpdateManuscripts(int manuscripts)
    {
        manuscriptsCounter.text = manuscripts > 9 ? manuscripts.ToString() : "0" + manuscripts.ToString();
    }

    

}
