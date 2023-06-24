using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for the game's main menu screen.
/// </summary>
public class MainMenu : MonoBehaviour, IDataPersistence {
    /// <summary>
    /// The main home page of the main menu.
    /// </summary>
    [SerializeField] private RectTransform homePage;

    /// <summary>
    /// The button that starts the game.
    /// </summary>
    [SerializeField] private Button startButton;

    /// <summary>
    /// The button that quits from the game.
    /// </summary>
    [SerializeField] private Button quitButton;

    /// <summary>
    /// The button that takes the player back to the previous page.
    /// </summary>
    [SerializeField] private Button backButton;

    /// <summary>
    /// A stack that keeps track of the pages that the player has visited.
    /// </summary>
    private readonly Stack<RectTransform> menuStack = new();

    /// <summary>
    /// The last scene that the player saved at.
    /// </summary>
    private string lastSaveScene;

    /// <inheritdoc />
    private void Start() {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other) {
            quitButton.gameObject.SetActive(false);
        }

        // Initially add home page to stack
        menuStack.Push(homePage);
    }

    /// <inheritdoc />
    private void OnEnable() {
        InputManager.Instance.Cancel.performed += _ => Back();
    }

    /// <summary>
    /// Open a menu, hiding the current one.
    /// </summary>
    /// <param name="menu">The menu to show.</param>
    public void OpenMenu(RectTransform menu) {
        var topMenu = menuStack.Peek();
        if (menu != homePage) {
            backButton.gameObject.SetActive(true);
        }
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
            var topMenu = menuStack.Peek();
            topMenu.gameObject.SetActive(true);
            if (topMenu == homePage) {
                backButton.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Quit the game from the main menu.
    /// </summary>
    public void QuitGame() {
        GameManager.QuitGame();
    }

    /// <summary>
    /// Load the last scene that the player saved in.
    /// </summary>
    public void LoadLastSaveSpot() {
        GameManager.Instance.LoadSaveSpot(lastSaveScene);
    }

    /// <inheritdoc />
    public void LoadData(SaveData saveData) {
        lastSaveScene = saveData.SaveScene;
    }

    /// <inheritdoc />
    public void SaveData(SaveData saveData) {

    }
}
