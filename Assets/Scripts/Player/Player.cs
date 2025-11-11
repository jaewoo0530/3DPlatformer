using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Walk,
    Run,
    SpeedUp
}

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStatus status;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        status = GetComponent<PlayerStatus>();
    }
}
