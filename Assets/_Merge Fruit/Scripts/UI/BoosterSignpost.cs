using TMPro;

public class BoosterSignpost : Singleton<BoosterSignpost>
{
    public TextMeshPro text;

    private void OnEnable()
    {
        text = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
    }

    public void ShowSignpost(string signpost)
    {
        text.text = signpost;
        gameObject.SetActive(true);
    }
}
