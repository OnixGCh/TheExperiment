using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyScript : MonoBehaviour
{
    private PlayerScript _player;
    private Weapons selectedWeapon;
    private int ammoCount;
    private float healthValue;
    private bool mode;
    //false - heal
    //true - ammo
    void Start()
    {

        if (Random.Range(1, 10) <= 3)
            mode = false;
        else
            mode = true;

        _player = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (mode)
        {
            int chances = Random.Range(1, 11);

            if(chances <= 5)
            {
                selectedWeapon = Weapons.Pistol;
                ammoCount = Random.Range(5, 8);
            }
            else if(chances > 5 && chances <= 9)
            {
                selectedWeapon = Weapons.SMG;
                ammoCount = Random.Range(3, 6);
            }
            else
            {
                selectedWeapon = Weapons.Rifle;
                ammoCount = Random.Range(2, 4);
            }
        }
        else
        {                   
            healthValue = Random.Range(3, 15);
        }
    }

    public void GetSupply()
    {
        if (mode)
        {
            _player.AddAmmo(selectedWeapon, ammoCount);
        }
        else
        {
            _player.Heal(healthValue);
        }
    }
}
