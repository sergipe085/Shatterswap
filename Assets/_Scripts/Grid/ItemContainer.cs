using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Singleton<ItemContainer>
{
    [SerializeField] private List<Item> itemList = new();

    public Item GetRandomItem() {
        return itemList[Random.Range(0, itemList.Count)];
    }
}
