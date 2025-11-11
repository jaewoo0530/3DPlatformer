using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemObject
{
    [SerializeField] private float speedUpValue;
    [SerializeField] private float speedUpTime;

    public override void OnInteract()
    {
        base.OnInteract();
        EffectManager.Instance.ApplySpeedBuff(speedUpValue, speedUpTime);
    }
}
