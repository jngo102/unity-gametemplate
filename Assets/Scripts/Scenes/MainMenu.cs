using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private RectTransform homePage;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;

    private Stack<RectTransform> menuStack = new();

    private void Start() {
        startButton.onClick.AddListener(() => GameManager.Instance.ChangeScene("SampleScene"));
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other) {
            quitButton.gameObject.SetActive(false);
        } else {
            quitButton.onClick.AddListener(GameManager.QuitGame);
        }

        // Add home page to stack by default
        menuStack.Push(homePage);
    }

    private void OnEnable() {
        InputManager.Instance.Cancel.performed += _ => Back();
    }

    /// <summary>
    /// Show a menu, hiding the current one.
    /// </summary>
    /// <param name="menu">The menu to show.</param>
    public void ShowMenu(RectTransform menu) {
        var topMenu = menuStack.Peek();
        backButton.gameObject.SetActive(topMenu == homePage || menu != homePage);
        topMenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        menuStack.Push(menu);
    }

    /// <summary>
    /// Go to the previous menu screen.
    /// </summary>
    public void Back() {
        if (menuStack.Count > 1) {
            menuStack.Pop().gameObject.SetActive(false);
            menuStack.Peek().gameObject.SetActive(true);
        }
    }

}
