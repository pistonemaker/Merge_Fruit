using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image endImage;
    public Button nextButton;

    private void OnEnable()
    {
        nextButton.onClick.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveAllListeners();
    }

    private void LoadGame()
    {
        SceneManager.LoadSceneAsync("Scenes/Game Play");
    }
}
