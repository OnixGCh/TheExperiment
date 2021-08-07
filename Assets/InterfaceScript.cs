using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private GameObject actionProgressBar;
    [SerializeField] private GameObject ammoLoaded;
    [SerializeField] private GameObject ammoInventory;
    private Slider actionProgressBarSlider;

    private void Start()
    {
        actionProgressBarSlider = actionProgressBar.GetComponent<Slider>();
    }
    public void ChangeHealthBar(float currentHealth, float maxHealth)
    {
        if (currentHealth < 0)
            currentHealth = 0;
        HealthBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
    }
    public void RefreshAmmoInfo(WeaponScript weapon)
    {
        ammoLoaded.GetComponent<Text>().text = weapon.getAmmoLoaded().ToString();
        ammoInventory.GetComponent<Text>().text = weapon.getAmmoInventory().ToString();
    }

    public void ActionProgressBarEnable() { actionProgressBar.SetActive(true); }
    public void ActionProgressBarDisable() { actionProgressBar.SetActive(false); }
    public void SetActionProgressBarValue(float value)
    {
        actionProgressBarSlider.value = value;
    }
}
