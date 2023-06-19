using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        startButton.onClick.AddListener(() => GameManager.Instance.ChangeScene("SampleScene"));
        quitButton.onClick.AddListener(GameManager.QuitGame);
    }
}
