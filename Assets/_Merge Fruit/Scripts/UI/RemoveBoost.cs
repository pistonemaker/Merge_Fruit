public class RemoveBoost : BoostUI
{
    private void OnEnable()
    {
        Init(DataKey.RemoveBoost);
        SetUpBooster();
        this.RegisterListener(EventID.On_Use_Remove_Boost, param => OnUseBooster((string) param));
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Use_Remove_Boost, param => OnUseBooster((string) param));
    }
}
