using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHp = 100;
    public float maxStamina = 100;
    [SerializeField] private float health;
    public float stamina;

    void Start()
    {
        health = maxHp;
        stamina = maxStamina;
    }

    void Update()
    {
        
    }

    public void AddHp(float value)
    {
        health += value;
        health = Mathf.Min(health, maxHp);
    }

    public void ReduceHp(float value)
    {
        health -= value;
        health = Mathf.Max(health, 0);
    }

    public void AddStamina(float value)
    {
        stamina += value;
        stamina = Mathf.Min(stamina, maxStamina);
    }

    public void ReduceStamina(float value)
    {
        stamina -= value;
        stamina = Mathf.Max(stamina, 0);
        Debug.Log($"스태미나 감소 -> {stamina}");
    }
}
