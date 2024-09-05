using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    public float BasicSize = 25;
    public BagItem BagItem;
    public List<CharacterItem> Items = new List<CharacterItem>();
    public bool AddItem(CharacterItem item)
    {
        bool can = GetFree() >= item.Size;
        if (can)
        {
            Items.Add(item);
        }
        return can;
    }
    public float GetPower()
    {
        float power = BasicSize;
        if (BagItem != null) power += BagItem.Power;
        return power;
    }
    public float GetNeed()
    {
        float need = 0;
        foreach (CharacterItem item in Items) need += item.Size;
        return need;
    }
    public float GetFree()
    {
        return GetPower() - GetNeed();
    }
}
