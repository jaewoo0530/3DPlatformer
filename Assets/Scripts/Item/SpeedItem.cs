using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemObject
{
    [SerializeField] private float speedUpTime;
    [SerializeField] private float speedUpValue;

    private Coroutine speedUpRoutine;

    public override void OnInteract()
    {
        base.OnInteract();

        if (speedUpRoutine != null)
            StopCoroutine(speedUpRoutine);

        speedUpRoutine = StartCoroutine(SpeedUp());
    }

    private IEnumerator SpeedUp()
    {
        var controller = CharacterManager.Instance.Player.controller;

        controller.moveSpeed = speedUpValue;
        yield return new WaitForSeconds(speedUpTime);
        controller.moveSpeed = controller.defaultSpeed;

        speedUpRoutine = null;
    }
}
