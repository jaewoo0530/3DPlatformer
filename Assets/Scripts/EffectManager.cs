// EffectManager.cs
using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("EffectManager").AddComponent<EffectManager>();
            }
            return _instance;
        }
    }

    private PlayerController controller;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ApplySpeedBuff(float value, float duration)
    {
        StartCoroutine(SpeedBuffRoutine(value, duration));
    }

    private IEnumerator SpeedBuffRoutine(float value, float duration)
    {
        if (controller == null)
        {
            controller = CharacterManager.Instance.Player.controller;
        }

        controller.playerState = PlayerState.SpeedUp;

        controller.moveSpeed = value;
        yield return new WaitForSeconds(duration);
        controller.playerState = PlayerState.Walk;
        controller.moveSpeed = controller.defaultSpeed;
    }

    public void ApplyDoubleJump()
    {
        if (controller == null)
        {
            controller = CharacterManager.Instance.Player.controller;
        }

        controller.isDoubleJump = true;
    }
}
