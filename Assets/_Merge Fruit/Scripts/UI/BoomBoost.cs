public class BoomBoost : BoostUI
{
    private void OnEnable()
    {
        Init(DataKey.BoomBoost);
        SetUpBooster();
        this.RegisterListener(EventID.On_Use_Boom_Boost, param => OnUseBooster((string) param));
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Use_Boom_Boost, param => OnUseBooster((string) param));
    }
}
