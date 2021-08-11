using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private float currentShotDelay;
    private int ammoLoaded;
    private bool isReloading = false;
    private bool noAmmoPressed = false;
    [SerializeField] private float damage;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shotDelay;
    [SerializeField] private int magazineMaxAmmo;
    [SerializeField] private int inventoryMaxAmmo;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyAmmoSound;
    [SerializeField] private GameObject shotFlash;
    [SerializeField] private Transform shotFlashSpawn;
    private Weapons thisWeapon;
    private PlayerScript _player;
    private InterfaceScript _interface;
    private AudioSource _audioSource;


    public int getAmmoLoaded() { return ammoLoaded; }
    public int getInventoryMaxAmmo() { return inventoryMaxAmmo; }
    public float getDamage() { return damage; }
    public void setNoAmmoPressed(bool value) { noAmmoPressed = value; }
    public void setIsReloading(bool value) { isReloading = value; }
    private void Start()
    {
        switch (this.gameObject.name)
        {
            case "Pistol":
                thisWeapon = Weapons.Pistol;
                break;

            case "SMG":
                thisWeapon = Weapons.SMG;
                break;

            case "Rifle":
                thisWeapon = Weapons.Rifle;
                break;
        }

        _player = GameObject.Find("Player").GetComponent<PlayerScript>();
        _interface = GameObject.Find("Canvas").GetComponent<InterfaceScript>();
        _audioSource = GetComponent<AudioSource>();
        ammoLoaded = 0;

        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(currentShotDelay >= 0)
            currentShotDelay -= Time.deltaTime;
    }
    public bool TryShot() 
    {        
        if (!isReloading)
        {
            if (ammoLoaded == 0)
            {
                if (!noAmmoPressed)
                {
                    setNoAmmoPressed(true);
                    _audioSource.PlayOneShot(emptyAmmoSound);
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
                _interface.RefreshLoadedAmmo(this);
                return true;
            }
        }

        return false;
    }
    public void TryReload(int ammoInventory)
    {
        if (ammoInventory != 0 && ammoLoaded != magazineMaxAmmo + 1 && !isReloading)
            StartCoroutine(Reload(ammoInventory));
    }

    IEnumerator Reload(int ammoInventory)
    {
        _audioSource.PlayOneShot(reloadSound);
        isReloading = true;
        setNoAmmoPressed(false);

        yield return new WaitForSeconds(reloadTime);

        int request;
        if (ammoLoaded > 1)
        {
            request = magazineMaxAmmo - ammoLoaded + 1;
        }
        else 
        {
            request = magazineMaxAmmo;
        }

        if (request > ammoInventory)
            request = ammoInventory;

        ammoLoaded += request;
        ammoInventory -= request;

        _interface.RefreshLoadedAmmo(this);
        _interface.RefreshAmmoInventory(ammoInventory);
        isReloading = false;
        _player.SetAmmoInventory(thisWeapon, ammoInventory);
    }
}
