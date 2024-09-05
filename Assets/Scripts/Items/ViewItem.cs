using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewItem : MonoBehaviour
{
    public GameObject Item;

    GameObject LastView;
    private void Awake()
    {
        UpdateView();
    }
    public void UpdateView()
    {
        if(LastView != null) Destroy(LastView);
        Instantiate(Item.GetComponent<CharacterItem>().View, transform);
    }

#if DEBUG
    public bool Fixed;
    private void FixedUpdate()
    {
        if(Fixed) UpdateView();
    }
#endif
}
