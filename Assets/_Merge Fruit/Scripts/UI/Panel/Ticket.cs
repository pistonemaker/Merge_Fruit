using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ticket : MonoBehaviour
{
    public TextMeshProUGUI ticketText;
    public Button addButton;

    private void OnEnable()
    {
        ticketText.text = PlayerPrefs.GetInt(DataKey.Ticket).ToString();
        addButton.onClick.AddListener(AddTicket);
    }

    private void OnDisable()
    {
        addButton.onClick.RemoveAllListeners();
    }

    private void AddTicket()
    {
        int ticket = PlayerPrefs.GetInt(DataKey.Ticket);
        ticket++;
        ticketText.text = ticket.ToString();
        PlayerPrefs.SetInt("Ticket", ticket);
    }
}
