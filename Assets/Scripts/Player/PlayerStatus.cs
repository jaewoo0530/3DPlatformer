using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHp = 100;
    public float maxStamina = 100;
    public float curHealth;
    public float curStamina;

    void Start()
    {
        curHealth = maxHp;
        curStamina = maxStamina;
    }

    public void AddHp(float value)
    {
        curHealth += value;
        curHealth = Mathf.Min(curHealth, maxHp);
    }

    public void ReduceHp(float value)
    {
        curHealth -= value;
        curHealth = Mathf.Max(curHealth, 0);
    }

    public void AddStamina(float value)
    {
        curStamina += value;
        curStamina = Mathf.Min(curStamina, maxStamina);
    }

    public void ReduceStamina(float value)
    {
        curStamina -= value;
        curStamina = Mathf.Max(curStamina, 0);
        Debug.Log($"스태미나 감소 -> {curStamina}");
    }

    public float GetPercentageHp()
    {
        return curHealth / maxHp;
    }

    public float GetPercentageStamina()
    {
        return curStamina / maxStamina;
    }
}
