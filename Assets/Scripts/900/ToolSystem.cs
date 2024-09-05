using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ToolSystem : MonoBehaviour
{
    [Header("Basic")]
    public float LerpSpeed = 5;

    [Header("Static")]
    public Transform RightHandTargetPoint;
    public Transform LeftHandTargetPoint;

    [Header("Edit")]
    public Transform RightHandTarget;
    public Transform LeftHandTarget;

    [Header("IK")]
    public TwoBoneIKConstraint RightIK;
    public TwoBoneIKConstraint LeftIK;

    [Header("Components")]
    public ToolItem StarterTool;
    public ToolItem NowTool;
    public List<ToolItem> Tools;

    //pri
    private PlayerMovementController GetPlayer;
    private void Awake()
    {
        GetPlayer = GetComponent<PlayerMovementController>();
        StartTool(StarterTool);
    }

    private void Update()
    {
        if (RightHandTarget != null && RightHandTarget.gameObject.active)
        {
            RightHandTargetPoint.position = RightHandTarget.position;
            RightHandTargetPoint.rotation = RightHandTarget.rotation;
        }
        if (LeftHandTarget != null && RightHandTarget.gameObject.active)
        {
            LeftHandTargetPoint.position = LeftHandTarget.position;
            LeftHandTargetPoint.rotation = LeftHandTarget.rotation;
        }
    }
    private void FixedUpdate()
    {
        RightIK.weight = Mathf.Lerp(RightIK.weight, RightHandTarget != null ? 1 :0, LerpSpeed);
        LeftIK.weight = Mathf.Lerp(LeftIK.weight, LeftHandTarget != null ? 1 :0, LerpSpeed);
    }

    public void StartTool(ToolItem tool)
    {
        if(NowTool != null) NowTool.gameObject.SetActive(false);
        if (Tools.Contains(tool))
        {
            tool.gameObject.SetActive(true);
        }
        else if(tool != null)
        {
            Transform point = null;
            switch (tool.Point)
            {
                case ToolItem.PointMode.Head:
                    point = GetPlayer.Head;
                    break;
                case ToolItem.PointMode.Root:
                    point = GetPlayer.Root;
                    break;
            }
            tool = Instantiate(tool.gameObject, point).GetComponent<ToolItem>();
            tool.GetPlayer = GetPlayer;
            tool.GetToolSystem = this;
            RightHandTarget = tool.RightTarget;
            LeftHandTarget = tool.LeftTarget;
        }
        tool.gameObject.SetActive(true);
        NowTool = tool;
    }

    public void EndTool(ToolItem tool)
    {
        if (Tools.Contains(tool))
        {
            Destroy(tool.gameObject);
            Tools.Remove(tool);
        }
    }
}
