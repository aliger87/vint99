using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : CharacterItem
{
    [Header("Tool")]
    public ToolMode Mode;
    public PointMode Point;

    [Header("Rig")]
    public Transform RightTarget;
    public Transform LeftTarget;

    [Header("Components")]
    public PlayerMovementController GetPlayer;
    public ToolSystem GetToolSystem;

    public enum ToolMode
    {
        Basic,
        Drop,
        Helper
    }
    public enum PointMode
    {
        Head,
        Root
    }

    private void OnEnable() => View();
    private void OnDisable() => Hide();
    public virtual void View()
    {

    }
    public virtual void Hide()
    {

    }
}
