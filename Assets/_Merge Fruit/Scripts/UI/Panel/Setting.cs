using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : BasePanel
{
    public Button soundButton;
    public Button vibrateButton;
    public Button musicButton;
    public Button restartButton;
    public Button continueButton;

    public Image soundImage;
    public Image vibrateImage;
    public Image musicImage;
    public Sprite sound;
    public Sprite nonSound;
    public Sprite vibrate;
    public Sprite nonVibrate;
    public Sprite music;
    public Sprite nonMusic;
    
    private void OnEnable()
    {
        OpenPanel();
        ShowBox();
        SetListener();
    }

    private void ShowBox()
    {
        UIManager.Instance.blockClickMask.gameObject.SetActive(true);
        LoadBoxData();
    }

    private void LoadBoxData()
    {
        if (DataKey.IsUseVibrate())
        {
            vibrateImage.sprite = vibrate;
        }
        else
        {
            vibrateImage.sprite = nonVibrate;
        }
        
        if (DataKey.IsUseMusic())
        {
            musicImage.sprite = music;
            AudioManager.Instance.ToggleMusic(false);
        }
        else
        {
            musicImage.sprite = nonMusic;
            AudioManager.Instance.ToggleMusic(true);
        }
        
        if (DataKey.IsUseSound())
        {
            soundImage.sprite = sound;
            AudioManager.Instance.ToggleSFX(false);
        }
        else
        {
            soundImage.sprite = nonSound;
            AudioManager.Instance.ToggleSFX(true);
        }
    }

    private void SetListener()
    {
        vibrateButton.onClick.AddListener(() =>
        {
            if (vibrateImage.sprite == vibrate)
            {
                vibrateImage.sprite = nonVibrate;
                PlayerPrefs.SetInt(DataKey.Use_Vibrate, 0);
            }
            else if (vibrateImage.sprite == nonVibrate)
            {
                vibrateImage.sprite = vibrate;
                
                PlayerPrefs.SetInt(DataKey.Use_Vibrate, 1);
            }
        });
        
        musicButton.onClick.AddListener(() =>
        {
            if (musicImage.sprite == music)
            {
                musicImage.sprite = nonMusic;
                PlayerPrefs.SetInt(DataKey.Use_Music, 0);
                AudioManager.Instance.ToggleMusic(true);
                AudioManager.Instance.ToggleSFX(true);
            }
            else if (musicImage.sprite == nonMusic)
            {
                musicImage.sprite = music;
                PlayerPrefs.SetInt(DataKey.Use_Music, 1);
                AudioManager.Instance.ToggleMusic(false);
                AudioManager.Instance.ToggleSFX(false);
            }
        });
        
        soundButton.onClick.AddListener(() =>
        {
            if (soundImage.sprite == sound)
            {
                soundImage.sprite = nonSound;
                PlayerPrefs.SetInt(DataKey.Use_SFX, 0);
                AudioManager.Instance.ToggleMusic(true);
                AudioManager.Instance.ToggleSFX(true);
            }
            else if (soundImage.sprite == nonSound)
            {
                soundImage.sprite = sound;
                PlayerPrefs.SetInt(DataKey.Use_SFX, 1);
                AudioManager.Instance.ToggleMusic(false);
                AudioManager.Instance.ToggleSFX(false);
            }
        });
        
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("Scenes/Game Play");
        });
        
        continueButton.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        soundButton.onClick.RemoveAllListeners();
        vibrateButton.onClick.RemoveAllListeners();
        musicButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
    }
}
