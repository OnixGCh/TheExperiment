using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private float currentShotDelay;
    private int ammoLoaded;
    private int ammoInventory;
    private bool isReloading = false;
    private bool noAmmoPressed = false;
    [SerializeField] private float damage;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shotDelay;
    [SerializeField] private int magazineMaxAmmo;
    [SerializeField] private int inventoryMaxAmmo;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private GameObject shotFlash;
    [SerializeField] private Transform shotFlashSpawn;
    private InterfaceScript _interface;
    private AudioSource _audioSource;

    private void Start()
    {
        _interface = GameObject.Find("Canvas").GetComponent<InterfaceScript>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
    }
    private void Update()
    {
        if(currentShotDelay >= 0)
            currentShotDelay -= Time.deltaTime;
    }
    public int getAmmoLoaded() { return ammoLoaded; }
    public int getAmmoInventory() { return ammoInventory; }
    public int getInventoryMaxAmmo() { return inventoryMaxAmmo; }
    public float getDamage() { return damage; }
    public void setNoAmmoPressed(bool value) { noAmmoPressed = value; }
    public bool TryShot() 
    {        
        if (!isReloading)
        {
            if (ammoLoaded == 0)
            {
                if (!noAmmoPressed)
                {
                    setNoAmmoPressed(true);
                    _audioSource.clip = noAmmoSound;
                    _audioSource.Play();
                }
                return false;
            }

            if (currentShotDelay < 0)
            {
                setNoAmmoPressed(false);
                currentShotDelay = shotDelay;
                ammoLoaded--;
                _audioSource.clip = shotSound;
                _audioSource.Play();
                Instantiate(shotFlash, shotFlashSpawn.position, shotFlashSpawn.rotation);
                _interface.RefreshAmmoInfo(this);
                return true;
            }
        }

        return false;
    }
    public void TryReload()
    {
        if (ammoInventory != 0 && ammoLoaded != magazineMaxAmmo + 1)
            StartCoroutine(Reload());
    }

    private int TakeInventoryAmmo(int request)
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

    IEnumerator Reload()
    {
        _audioSource.clip = reloadSound;
        _audioSource.Play();
        isReloading = true;
        setNoAmmoPressed(false);

        yield return new WaitForSeconds(reloadTime);

        int difference;
        if (ammoLoaded > 1)
        {
            difference = magazineMaxAmmo - ammoLoaded + 1;
            ammoLoaded += TakeInventoryAmmo(difference);
        }
        else
            ammoLoaded += TakeInventoryAmmo(magazineMaxAmmo);

        _interface.RefreshAmmoInfo(this);
        isReloading = false;
    }

    public void FillAmmo() 
    { 
        ammoInventory = inventoryMaxAmmo;
        _interface.RefreshAmmoInfo(this);
    }
}
