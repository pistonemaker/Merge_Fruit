using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public GameData data;
    public MergeFx mergeFx;
    public FailNotice failNotice;
    public Boom boom;
    public BoomExplosion boomExplosion;
    private int nextID;

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        AudioManager.Instance.PlayMusic("Game_Play");
        SetID();
    }

    public void CheckIfPlayerPlayFirstTime()
    {
        if (!DataKey.IsPlayerPlayFirstTime())
        {
            return;
        }
        
        PlayerPrefs.SetInt(DataKey.IsPlayingFirstTime, 1);
        PlayerPrefs.SetInt(DataKey.RemoveBoost, 1);
        PlayerPrefs.SetInt(DataKey.BoomBoost, 1);
        PlayerPrefs.SetInt(DataKey.UpgradeBoost, 1);
        PlayerPrefs.SetInt(DataKey.ShakeBoost, 1);
        PlayerPrefs.SetInt(DataKey.Use_Music, 1);
        PlayerPrefs.SetInt(DataKey.Use_Vibrate, 1);
        PlayerPrefs.SetInt(DataKey.Use_SFX, 1);
    }

    private void SetID()
    {
        for (int i = 0; i < data.fruitDatas.Count; i++)
        {
            data.fruitDatas[i].fruitPrefab.id = i;
        }
    }

    public Fruit GetFirstFruit()
    {
        int id = Random.Range(0, 4);
        nextID = Random.Range(0, 4);
        this.PostEvent(EventID.On_Show_Next_Fruit, nextID);
        return data.fruitDatas[id].fruitPrefab;
    }
    
    public Fruit GetRandomFruit()
    {
        int id = nextID;
        nextID = Random.Range(0, 4);
        this.PostEvent(EventID.On_Show_Next_Fruit, nextID);
        return data.fruitDatas[id].fruitPrefab;
    }
}
