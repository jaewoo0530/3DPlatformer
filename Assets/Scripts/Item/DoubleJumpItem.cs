using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItem : ItemObject
{
    public override void OnInteract()
    {
        base.OnInteract();
        EffectManager.Instance.ApplyDoubleJump();
    }
}
