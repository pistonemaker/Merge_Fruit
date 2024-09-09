public class UpgradeBoost : BoostUI
{
    private void OnEnable()
    {
        Init(DataKey.UpgradeBoost);
        SetUpBooster();
        this.RegisterListener(EventID.On_Use_Upgrade_Boost, param => OnUseBooster((string) param));
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Use_Upgrade_Boost, param => OnUseBooster((string) param));
    }
}
