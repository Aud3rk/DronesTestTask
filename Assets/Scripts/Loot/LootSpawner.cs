using System;
using Extentions;
using UnityEngine;
using Object = UnityEngine.Object;

public class LootSpawner
{
    private Vector3 _lowPoint;
    private Vector3 _highPoint;
        
    public LootSpawner(Vector3 LowLeftPoint, Vector3 HighRightPosition)
    {
        _lowPoint = LowLeftPoint;
        _highPoint = HighRightPosition;
    }

    public void SpawnLoot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Loot");
            var go = Object.Instantiate(prefab, RandomExts.GeneratePoint(_lowPoint, _highPoint) ,Quaternion.identity);
            var lootComponent = go.GetComponent<Loot>();
            lootComponent.Init(5,10);
            lootComponent.gameObject.name = i.ToString();

        }
    }

        
}