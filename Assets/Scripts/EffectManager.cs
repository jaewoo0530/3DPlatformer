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
        PlayerController controller = CharacterManager.Instance.Player.controller;
        controller.moveSpeed = value;
        yield return new WaitForSeconds(duration);
        controller.moveSpeed = controller.defaultSpeed;
    }
}
