using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data / Game Data")]
public class GameData : ScriptableObject
{
    public List<FruitData> fruitDatas;
}

[Serializable]
public class FruitData
{
    public Fruit fruitPrefab;
    public int scrore;
}
