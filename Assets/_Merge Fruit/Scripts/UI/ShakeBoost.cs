public class ShakeBoost : BoostUI
{
    private void OnEnable()
    {
        Init(DataKey.ShakeBoost);
        SetUpBooster();
        this.RegisterListener(EventID.On_Use_Shake_Boost, param => OnUseBooster((string) param));
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Use_Shake_Boost, param => OnUseBooster((string) param));
    }
}
