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
        LoadBoxData();
    }

    private void LoadBoxData()
    {
        if (DataKey.IsUseVibrate())
        {
            vibrateButton.image.sprite = vibrate;
        }
        else
        {
            vibrateButton.image.sprite = nonVibrate;
        }
        
        if (DataKey.IsUseMusic())
        {
            musicButton.image.sprite = music;
            AudioManager.Instance.ToggleMusic(false);
        }
        else
        {
            musicButton.image.sprite = nonMusic;
            AudioManager.Instance.ToggleMusic(true);
        }
        
        if (DataKey.IsUseSound())
        {
            soundButton.image.sprite = sound;
            AudioManager.Instance.ToggleSFX(false);
        }
        else
        {
            soundButton.image.sprite = nonSound;
            AudioManager.Instance.ToggleSFX(true);
        }
    }

    private void SetListener()
    {
        vibrateButton.onClick.AddListener(() =>
        {
            if (vibrateButton.image.sprite == vibrate)
            {
                vibrateButton.image.sprite = nonVibrate;
                PlayerPrefs.SetInt(DataKey.Use_Vibrate, 0);
            }
            else if (vibrateButton.image.sprite == nonVibrate)
            {
                vibrateButton.image.sprite = vibrate;
                
                PlayerPrefs.SetInt(DataKey.Use_Vibrate, 1);
            }
        });
        
        musicButton.onClick.AddListener(() =>
        {
            if (musicButton.image.sprite == music)
            {
                musicButton.image.sprite = nonMusic;
                PlayerPrefs.SetInt(DataKey.Use_Music, 0);
                AudioManager.Instance.ToggleMusic(true);
                AudioManager.Instance.ToggleSFX(true);
            }
            else if (musicButton.image.sprite == nonMusic)
            {
                musicButton.image.sprite = music;
                PlayerPrefs.SetInt(DataKey.Use_Music, 1);
                AudioManager.Instance.ToggleMusic(false);
                AudioManager.Instance.ToggleSFX(false);
            }
        });
        
        soundButton.onClick.AddListener(() =>
        {
            if (soundButton.image.sprite == sound)
            {
                soundButton.image.sprite = nonSound;
                PlayerPrefs.SetInt(DataKey.Use_SFX, 0);
                AudioManager.Instance.ToggleMusic(true);
                AudioManager.Instance.ToggleSFX(true);
            }
            else if (soundButton.image.sprite == nonSound)
            {
                soundButton.image.sprite = sound;
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
