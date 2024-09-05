using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItem : MonoBehaviour
{
    public string Name;
    public string Description;
    public Sprite Image;
    public GameObject View;
    public float Size = 1;

    public virtual void Take(PlayerMovementController player)
    {

    }
}
