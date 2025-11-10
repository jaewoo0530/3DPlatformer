using UnityEngine.UI;
using UnityEngine;

public class UIStatus : MonoBehaviour
{
    public Image hpUIBar;
    public Image staminaUIBar;

    private void Update()
    {
        hpUIBar.fillAmount = CharacterManager.Instance.Player.status.GetPercentageHp();
        staminaUIBar.fillAmount = CharacterManager.Instance.Player.status.GetPercentageStamina();
    }
}
