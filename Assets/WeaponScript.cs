using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private int ammoLoaded;
    private int ammoInventory;
    [SerializeField] private float damage;
    [SerializeField] private int magazineMaxAmmo;
    [SerializeField] private AudioClip shotSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
    }
    public int getAmmoLoaded() { return ammoLoaded; }
    public int getAmmoInventory() { return ammoInventory; }
    public float getDamage() { return damage; }
    public void Shot() 
    { 
        ammoLoaded--;
        _audioSource.clip = shotSound;
        _audioSource.Play();        
    }
    public bool TryReload()
    {
        int difference;
        if(ammoLoaded > 1)
        {
            difference = magazineMaxAmmo - ammoLoaded + 1;

            if (ammoInventory != 0)
            {
                ammoLoaded += TryToTakeInventoryAmmo(difference);
                return true;
            }
            else
                return false;
        }
        else
        {
            if (ammoInventory != 0)
            {
                ammoLoaded += TryToTakeInventoryAmmo(magazineMaxAmmo);
                return true;
            }
            else
                return false;
        }
    }

    private int TryToTakeInventoryAmmo(int request)
    {
        if(ammoInventory >= request)
        {
            ammoInventory -= request;
            return request;
        }
        else
        {
            request = ammoInventory;
            ammoInventory = 0;
            return request;
        }
    }

    public void AddAmmo(int count)
    {
        ammoInventory += count;
    }
}
