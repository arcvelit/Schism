using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PreambleHandler : MonoBehaviour
{
    private UIDocument _document;

    private Button _nextButton;
    
    private Button _playButton;
    
    private VisualElement _textContainer;
    
    private VisualElement _scrollContainer;
    
    private VisualElement _nextButtonContainer;
    
    private VisualElement _playButtonContainer;
    
    public string _sceneToLoad;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _nextButton = _document.rootVisualElement.Q("NextButton") as Button;
        _playButton = _document.rootVisualElement.Q("PlayButton") as Button;
        _textContainer = _document.rootVisualElement.Q("TextContainer") as VisualElement;
        _scrollContainer = _document.rootVisualElement.Q("ScrollContainer") as VisualElement;
        _nextButtonContainer = _document.rootVisualElement.Q("NextButtonContainer") as VisualElement;
        _playButtonContainer = _document.rootVisualElement.Q("PlayButtonContainer") as VisualElement;
        _nextButton.RegisterCallback<ClickEvent>(OnNextClick);
        _playButton.RegisterCallback<ClickEvent>(OnPlayClick);
    }
    
    private void OnNextClick(ClickEvent ce)
    {
        StartCoroutine(SwitchPanels());
    }

    private void OnPlayClick(ClickEvent ce)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    private IEnumerator SwitchPanels()
    {
        _scrollContainer.style.display = DisplayStyle.None;
        _nextButtonContainer.style.display = DisplayStyle.None;
        _playButtonContainer.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(0.1f);
        _textContainer.style.display = DisplayStyle.Flex;
    }
    
    private void OnDisable()
    {
        _nextButton.UnregisterCallback<ClickEvent>(OnNextClick);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
