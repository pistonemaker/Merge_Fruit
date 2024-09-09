using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    [SerializeField] protected Image amount;
    [SerializeField] protected TextMeshProUGUI amountText;
    [SerializeField] protected Image addButton;
    protected string dataKey;

    private void Awake()
    {
        GameManager.Instance.CheckIfPlayerPlayFirstTime();
    }

    protected void Init(string key)
    {
        dataKey = key;
        amount = transform.GetChild(1).gameObject.GetComponent<Image>();
        amountText = amount.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        addButton = transform.GetChild(2).GetComponent<Image>();
    }

    protected void SetUpBooster()
    {
        int boosterCount = PlayerPrefs.GetInt(dataKey);
        
        if (boosterCount > 0)
        {
            amount.gameObject.SetActive(true);
            amountText.text = boosterCount.ToString();
            addButton.gameObject.SetActive(false);
        }
        else
        {
            amount.gameObject.SetActive(false);
            amountText.text = "0";
            addButton.gameObject.SetActive(true);
        }
    }

    protected void OnUseBooster(string key)
    {
        if (key != dataKey)
        {
            return;
        }
        
        int boosterCount = PlayerPrefs.GetInt(dataKey);
        boosterCount--;
        if (boosterCount < 0)
        {
            boosterCount = 0;
        }
        
        PlayerPrefs.SetInt(dataKey, boosterCount);
        amountText.text = boosterCount.ToString();
        SetUpBooster();
    }
}
