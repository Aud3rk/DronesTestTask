using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int LootCount;

    public void Init(int minLootValue, int maxLootValue)
    {
        var random = new System.Random();
        LootCount = random.Next(minLootValue, maxLootValue);
    }

    public int Take()
    {
        gameObject.SetActive(false);
        return LootCount;
    }
}