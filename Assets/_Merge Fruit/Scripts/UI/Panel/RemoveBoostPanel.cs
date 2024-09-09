using UnityEngine;
using UnityEngine.UI;

public class RemoveBoostPanel : BasePanel
{
    public Button closeButton;
    public Button buyButton;
    public Button watchAdsButton;
    public Ticket ticket;

    private void OnEnable()
    {
        OpenPanel();
        closeButton.onClick.AddListener(ClosePanel);
        buyButton.onClick.AddListener(BuyBooster);
        watchAdsButton.onClick.AddListener(ShowAds);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
        watchAdsButton.onClick.RemoveAllListeners();
    }

    private void BuyBooster()
    {
        int ticketCount = PlayerPrefs.GetInt(DataKey.Ticket);
        if (ticketCount >= 1)
        {
            ticketCount--;
            PlayerPrefs.SetInt(DataKey.Ticket, ticketCount);
            ticket.ticketText.text = ticketCount.ToString();
            UIManager.Instance.PostEventDelay(EventID.On_Use_Remove_Boost_By_Ticket);
            ClosePanel();
        }
    }
    
    

    private void ShowAds()
    {
        
    }
}
