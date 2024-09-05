using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagItem : CharacterItem
{
    public float Power = 10;
    public override void Take(PlayerMovementController player)
    {
        player.bag.BagItem = this;
        Instantiate(gameObject, player.Root);
        base.Take(player);
    }
}
