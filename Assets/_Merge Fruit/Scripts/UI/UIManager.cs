using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Canvas canvas;
    public Image nextFruit;
    public Image blockClick;
    public Image blockClickMask;
    public List<Sprite> fruitSprites;

    public Button removeBoostButton;
    public Button boomBoostButton;
    public Button upgradeBoostButton;
    public Button shakeBoostButton;
    public Button settingButton;

    public Camera captureCam;
    public EndPanel endPanel;
    public Setting setting;
    public RemoveBoostPanel removeBoostPanel;
    public BoomBoostPanel boomBoostPanel;
    public UpgradeBoostPanel upgradeBoostPanel;
    public ShakeBoostPanel shakeBoostPanel;
    public GameObject UITop;

    private void OnEnable()
    {
        canvas = GetComponent<Canvas>();
        UITop = transform.Find("UI Top").gameObject;
        blockClick.gameObject.SetActive(false);
        blockClickMask.gameObject.SetActive(false);
        this.RegisterListener(EventID.On_Show_Next_Fruit, param => ShowNextFruit((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Player_Dead, ShowEndPanel);
        RegisterBoosterListener();

        removeBoostButton.onClick.AddListener(RemoveBoost);
        boomBoostButton.onClick.AddListener(BoomBoost);
        upgradeBoostButton.onClick.AddListener(UpgradeBoost);
        shakeBoostButton.onClick.AddListener(ShakeBoost);

        settingButton.onClick.AddListener(() =>
        {
            blockClick.gameObject.SetActive(true);
            setting.gameObject.SetActive(true);
        });
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Show_Next_Fruit, param => ShowNextFruit((int)param));
        EventDispatcher.Instance.RemoveListener(EventID.On_Player_Dead, ShowEndPanel);
        RemoveBoosterListener();
        removeBoostButton.onClick.RemoveAllListeners();
        boomBoostButton.onClick.RemoveAllListeners();
        upgradeBoostButton.onClick.RemoveAllListeners();
        shakeBoostButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
    }

    private void RegisterBoosterListener()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Use_Remove_Boost_By_Ticket, HandleRemoveBoost);
        EventDispatcher.Instance.RegisterListener(EventID.On_Use_Boom_Boost_By_Ticket, HandleBoomBoost);
        EventDispatcher.Instance.RegisterListener(EventID.On_Use_Upgrade_Boost_By_Ticket, HandleUpgradeBoost);
        EventDispatcher.Instance.RegisterListener(EventID.On_Use_Shake_Boost_By_Ticket, HanldeShakeBoost);
    }

    private void RemoveBoosterListener()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Use_Remove_Boost_By_Ticket, HandleRemoveBoost);
        EventDispatcher.Instance.RemoveListener(EventID.On_Use_Boom_Boost_By_Ticket, HandleBoomBoost);
        EventDispatcher.Instance.RemoveListener(EventID.On_Use_Upgrade_Boost_By_Ticket, HandleUpgradeBoost);
        EventDispatcher.Instance.RemoveListener(EventID.On_Use_Shake_Boost_By_Ticket, HanldeShakeBoost);
    }

    private void ShowNextFruit(int id)
    {
        nextFruit.transform.localScale = Vector3.zero;
        nextFruit.sprite = fruitSprites[id];
        nextFruit.transform.DOScale(Vector3.one, 0.2f);
    }

    private void RemoveBoost()
    {
        var canUse = FruitBox.Instance.HasSuitableFruit();

        if (!canUse)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            return;
        }

        var boostNumber = PlayerPrefs.GetInt(DataKey.RemoveBoost);
        Debug.Log(boostNumber);

        if (boostNumber == 0)
        {
            removeBoostPanel.gameObject.SetActive(true);
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Remove_Boost_By_Ticket);
    }

    private void HandleRemoveBoost(object param)
    {
        blockClick.gameObject.SetActive(true);

        if (FruitBox.Instance.GetFruitNumber() == 0)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            blockClick.gameObject.SetActive(false);
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Remove_Boost, DataKey.RemoveBoost);
        FruitBox.Instance.RemoveSmallestFruit();
    }

    private void BoomBoost()
    {
        if (FruitBox.Instance.GetFruitNumber() == 0)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            return;
        }

        var boostNumber = PlayerPrefs.GetInt(DataKey.BoomBoost);
        Debug.Log(boostNumber);
        if (boostNumber == 0)
        {
            boomBoostPanel.gameObject.SetActive(true);
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Boom_Boost_By_Ticket);
    }

    private void HandleBoomBoost(object param)
    {
        blockClick.gameObject.SetActive(true);
        FruitBox.Instance.isRemovingFruit = true;

        if (FruitBox.Instance.GetFruitNumber() == 0)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            FruitBox.Instance.isRemovingFruit = false;
            StartCoroutine(DeactiveBlockClick());
            return;
        }

        TreeSlider.Instance.lines.gameObject.SetActive(false);
        TreeSlider.Instance.gameObject.SetActive(false);
        UITop.SetActive(false);
        BoosterSignpost.Instance.ShowSignpost("Tap to remove a Fruit from the box");
        EventDispatcher.Instance.PostEvent(EventID.On_Use_Boom_Boost, DataKey.BoomBoost);
        FruitBox.Instance.ShowFruitsTarget();
    }

    private void UpgradeBoost()
    {
        if (FruitBox.Instance.GetFruitNumber() == 0)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            return;
        }

        var boostNumber = PlayerPrefs.GetInt(DataKey.UpgradeBoost);

        if (boostNumber == 0)
        {
            upgradeBoostPanel.gameObject.SetActive(true);
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Upgrade_Boost_By_Ticket);
    }

    private void HandleUpgradeBoost(object param)
    {
        blockClick.gameObject.SetActive(true);
        FruitBox.Instance.isUpgradingFruit = true;

        if (FruitBox.Instance.GetFruitNumber() == 0)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            FruitBox.Instance.isUpgradingFruit = false;
            StartCoroutine(DeactiveBlockClick());
            return;
        }

        TreeSlider.Instance.lines.gameObject.SetActive(false);
        TreeSlider.Instance.gameObject.SetActive(false);
        UITop.SetActive(false);
        BoosterSignpost.Instance.ShowSignpost("Tap to upgrade a Fruit in the box");
        EventDispatcher.Instance.PostEvent(EventID.On_Use_Upgrade_Boost, DataKey.UpgradeBoost);
        FruitBox.Instance.ShowFruitsTarget();
    }

    private void ShakeBoost()
    {
        if (FruitBox.Instance.GetFruitNumber() < 2)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            return;
        }

        var boostNumber = PlayerPrefs.GetInt(DataKey.ShakeBoost);

        if (boostNumber == 0)
        {
            shakeBoostPanel.gameObject.SetActive(true);
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Shake_Boost_By_Ticket);
    }

    private void HanldeShakeBoost(object param)
    {
        blockClick.gameObject.SetActive(true);

        if (FruitBox.Instance.GetFruitNumber() < 2)
        {
            var failNotice = PoolingManager.Spawn(GameManager.Instance.failNotice, transform.transform.position, Quaternion.identity);
            failNotice.ShowNotice("There is no suitable Object in the Box");
            FruitBox.Instance.isUpgradingFruit = false;
            blockClick.gameObject.SetActive(false);
            StartCoroutine(DeactiveBlockClick());
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.On_Use_Shake_Boost, DataKey.ShakeBoost);
        SetCanvasSortingLayer("Shake");
        StartCoroutine(FruitBox.Instance.ShakeBox());
    }

    public void SetCanvasSortingLayer(string layerName)
    {
        canvas.sortingLayerName = layerName;
    }

    public IEnumerator DeactiveBlockClick()
    {
        yield return new WaitForSeconds(0.1f);
        blockClick.gameObject.SetActive(false);
    }

    public Texture2D CaptureImageWithCamera()
    {
        captureCam.gameObject.SetActive(true);
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        int renderTextureWidth = screenWidth * 2; // Tăng độ phân giải gấp đôi cho nét 
        int renderTextureHeight = screenHeight * 2;

        RenderTexture rt = new RenderTexture(renderTextureWidth, renderTextureHeight, 24);
        rt.antiAliasing = 8;
        captureCam.targetTexture = rt;
        captureCam.Render();

        // Lấy ảnh từ RenderTexture với kích thước đầy đủ
        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(renderTextureWidth, renderTextureHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTextureWidth, renderTextureHeight), 0, 0);
        screenshot.Apply();

        captureCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        return screenshot;
    }

    private IEnumerator ShowImg()
    {
        yield return new WaitForSeconds(0.5f);
        int cropWidth = Screen.width * 2 - 100; // Kích thước crop tuỳ chỉnh
        int cropHeight = Screen.height - 50;

        int renderTextureWidth = Screen.width * 2; // Kích thước của RenderTexture
        int renderTextureHeight = Screen.height * 2;

        // Tính toán kích thước vùng crop
        int xOffset = (renderTextureWidth - cropWidth) / 2;
        int yOffset = (renderTextureHeight - cropHeight) / 2;

        Texture2D fullScreenshot = CaptureImageWithCamera();

        // Đảm bảo kích thước vùng crop hợp lệ
        if (xOffset < 0 || yOffset < 0 || xOffset + cropWidth > fullScreenshot.width || yOffset + cropHeight > fullScreenshot.height)
        {
            Debug.LogError("Crop region exceeds the bounds of the captured screenshot.");
            yield return null;
        }

        // Crop phần giữa của ảnh
        Texture2D croppedScreenshot = new Texture2D(cropWidth, cropHeight);
        croppedScreenshot.filterMode = FilterMode.Bilinear; // Tăng độ nét
        Color[] pixels = fullScreenshot.GetPixels(xOffset, yOffset, cropWidth, cropHeight);
        croppedScreenshot.SetPixels(pixels);
        croppedScreenshot.Apply();

        // Tạo Sprite từ ảnh đã crop và hiển thị trên EndPanel
        Sprite screenshotSprite = Sprite.Create(croppedScreenshot,
            new Rect(0, 50, croppedScreenshot.width, croppedScreenshot.height - 50),
            new Vector2(0.5f, 0.5f));

        SetCanvasSortingLayer("UI");
        endPanel.endImage.sprite = screenshotSprite;
        endPanel.gameObject.SetActive(true);
        endPanel.scoreText.text = "Score: " + ScoreManager.Instance.curScore;
        captureCam.gameObject.SetActive(false);
    }

    private void ShowEndPanel(object param)
    {
        StartCoroutine(CaptureFullScreenAndShowOnEndPanel());
    }

    public void PostEventDelay(EventID eventID)
    {
        StartCoroutine(PostEventDelayRoutine(eventID));
    }

    private IEnumerator PostEventDelayRoutine(EventID eventID)
    {
        yield return new WaitForSeconds(0.5f);
        EventDispatcher.Instance.PostEvent(eventID);
    }
    
    public void CaptureFullScreenAndShowOnEndPanel1()
    {
        captureCam.gameObject.SetActive(true);
    
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        if (screenWidth > screenHeight)
        {
            (screenWidth, screenHeight) = (screenHeight, screenWidth);
        }   
        Debug.Log(screenWidth + "\t" + screenHeight);

        RenderTexture rt = new RenderTexture(screenWidth, screenHeight, 24);
        rt.antiAliasing = 8;
        captureCam.targetTexture = rt;
        captureCam.Render();

        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        Debug.Log(screenshot.width + "\t" + screenshot.height);
        screenshot.Apply();

        captureCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // // Cắt ảnh (600x750) tại vị trí (0, 50) của ảnh vừa chụp
        // Texture2D croppedScreenshot = new Texture2D((int)(screenshot.width * 600f / 750f), 
        //     (int)(screenshot.height * 750f / 1334f));
        //
        // croppedScreenshot.SetPixels(screenshot.GetPixels(0, (int)(screenshot.height * 50f / 1334f), 
        //     (int)(screenshot.width * 600f / 750f), (int)(screenshot.height * 750f / 1334f))); 
        //
        // croppedScreenshot.Apply();

        Sprite screenshotSprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), 
            new Vector2(0.5f, 0.5f));

        // Gán Sprite cho endImage và chỉnh lại kích thước về 450x500
        endPanel.endImage.sprite = screenshotSprite;
        endPanel.endImage.rectTransform.sizeDelta = new Vector2(450, 500); // Chỉnh lại width và height của endImage

        // Hiển thị endPanel
        SetCanvasSortingLayer("UI");
        captureCam.gameObject.SetActive(false);
        endPanel.gameObject.SetActive(true);
        endPanel.scoreText.text = "Score: " + ScoreManager.Instance.curScore;
    }

    public IEnumerator CaptureFullScreenAndShowOnEndPanel()
    {
        yield return new WaitForEndOfFrame();
        captureCam.gameObject.SetActive(true);
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        //Debug.Log("Width: " + screenWidth + "   Height:" + screenHeight);
        int renderTextureWidth = screenWidth;
        int renderTextureHeight = screenHeight;

        RenderTexture rt = new RenderTexture(renderTextureWidth, renderTextureHeight, 24);
        rt.antiAliasing = 8;
        captureCam.targetTexture = rt;
        captureCam.Render();

        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(renderTextureWidth, renderTextureHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTextureWidth, renderTextureHeight), 0, 0);
        screenshot.Apply();

        captureCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Sprite screenshotSprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height),
            new Vector2(0.5f, 0.5f));

        SetCanvasSortingLayer("UI");
        captureCam.gameObject.SetActive(false);
        endPanel.endImage.sprite = screenshotSprite;
        endPanel.endImage.SetNativeSize();
        ScaleImageToResolution(endPanel.endImage);
        endPanel.gameObject.SetActive(true);
        endPanel.scoreText.text = "Score: " + ScoreManager.Instance.curScore;
    }

    public void ScaleImageToResolution(Image originalImage, int targetWidth = 750, int targetHeight = 1334)
    {
        int originalWidth = Screen.currentResolution.width;
        int originalHeight = Screen.currentResolution.height;
        if (originalWidth > originalHeight)
        { 
            (originalWidth, originalHeight) = (originalHeight, originalWidth);
        }

        Debug.Log(originalWidth + "\t" + originalHeight);

        float scaleX = 0.65f * targetWidth / originalWidth;
        float scaleY = 0.65f * targetHeight / originalHeight;

        Vector2 newScale = new Vector2(scaleX, scaleY);

        if (newScale.x * originalWidth < 450)
        {
            var scaleAdd = (450 - newScale.x * originalWidth) / originalWidth;
            newScale = new Vector2(newScale.x + scaleAdd, newScale.y);
            Debug.Log(1);
        }
        else if (newScale.x * originalWidth > 450)
        {
            var scaleAdd = (-450 + newScale.x * originalWidth) / originalWidth;
            
            if ((float)originalHeight / originalWidth < 2f)
            {
                Debug.Log(2);
                newScale = new Vector2(newScale.x - scaleAdd, newScale.y);
            }
            else
            {
                Debug.Log(3);
                newScale = new Vector2(newScale.x - scaleAdd / 2f, newScale.y);
            }
        }
        
        if (newScale.y * originalHeight < 500)
        {
            var scaleAdd = (500 - newScale.y * originalHeight) / originalHeight;
            newScale = new Vector2(newScale.x, newScale.y + scaleAdd);
            Debug.Log(4);
        }
        else if (newScale.y * originalHeight > 500)
        {
            var scaleAdd = (-500 + newScale.y * originalHeight) / originalHeight;
            if ((float)originalHeight / originalWidth < 2f)
            {
                Debug.Log(5);
                newScale = new Vector2(newScale.x, newScale.y - scaleAdd);
            }
            else
            {
                Debug.Log(6);
                newScale = new Vector2(newScale.x, newScale.y);
            }
        }

        if ((float)originalHeight / originalWidth < 2f)
        {
            Debug.Log("x");
            newScale = new Vector2(newScale.x * 1.25f, newScale.y * 1.5f);
        }
        
        originalImage.rectTransform.localScale = new Vector3(newScale.x, newScale.y, 1);
    }
}