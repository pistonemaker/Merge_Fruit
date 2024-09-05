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
    public List<Sprite> fruitSprites;
    public Button removeBoostButton;
    public Button boomBoostButton;
    public Button upgradeBoostButton;
    public Button shakeBoostButton;
    public Camera captureCam;
    public EndPanel endPanel;

    private void OnEnable()
    {
        canvas = GetComponent<Canvas>();
        blockClick.gameObject.SetActive(false);
        this.RegisterListener(EventID.On_Show_Next_Fruit, param => ShowNextFruit((int)param));
        removeBoostButton.onClick.AddListener(RemoveBoost);
        boomBoostButton.onClick.AddListener(BoomBoost);
        upgradeBoostButton.onClick.AddListener(UpgradeBoost);
        shakeBoostButton.onClick.AddListener(ShakeBoost);
        EventDispatcher.Instance.RegisterListener(EventID.On_Player_Dead, ShowEndPanel);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Show_Next_Fruit, param => ShowNextFruit((int)param));
        removeBoostButton.onClick.RemoveAllListeners();
        boomBoostButton.onClick.RemoveAllListeners();
        upgradeBoostButton.onClick.RemoveAllListeners();
        shakeBoostButton.onClick.RemoveAllListeners();
        EventDispatcher.Instance.RemoveListener(EventID.On_Player_Dead, ShowEndPanel);
    }

    private void ShowNextFruit(int id)
    {
        nextFruit.transform.localScale = Vector3.zero;
        nextFruit.sprite = fruitSprites[id];
        nextFruit.transform.DOScale(Vector3.one, 0.2f);
    }

    private void RemoveBoost()
    {
        blockClick.gameObject.SetActive(true);
        FruitBox.Instance.RemoveSmallestFruit();
    }

    private void BoomBoost()
    {
        blockClick.gameObject.SetActive(true);
        FruitBox.Instance.isRemovingFruit = true;
    }

    private void UpgradeBoost()
    {
        blockClick.gameObject.SetActive(true);
        FruitBox.Instance.isUpgradingFruit = true;
    }

    private void ShakeBoost()
    {
        SetCanvasSortingLayer("Shake");
        blockClick.gameObject.SetActive(true);
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
        int cropWidth = Screen.width * 2; // Kích thước crop tuỳ chỉnh
        int cropHeight = Screen.height;

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
            new Rect(0, 0, croppedScreenshot.width, croppedScreenshot.height),
            new Vector2(0.5f, 0.5f));

        SetCanvasSortingLayer("UI");
        endPanel.endImage.sprite = screenshotSprite;
        endPanel.gameObject.SetActive(true);    
        captureCam.gameObject.SetActive(false);
    }

    private void ShowEndPanel(object param)
    {
        StartCoroutine(ShowImg());
    }
}