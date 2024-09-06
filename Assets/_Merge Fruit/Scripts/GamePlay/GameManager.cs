using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public GameData data;
    public MergeFx mergeFx;
    public FailNotice failNotice;
    private int nextID;

    private void OnEnable()
    {
        SetID();
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
