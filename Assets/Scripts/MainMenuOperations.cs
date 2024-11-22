using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuOperations : MonoBehaviour
{
    private UIDocument _document;

    private Button _startButton;

    public string _sceneToLoad;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnStartClick);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartClick);
    }

    private void OnStartClick(ClickEvent ce)
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
