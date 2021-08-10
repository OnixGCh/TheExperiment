using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Transform _transform;
    private Transform _modelTransform;
    private Animator _animator;
    private CharacterController _controller;
    private InterfaceScript _interface;
    private WeaponScript _selectedWeapon;
    private float currentHealth;
    private float hitDistance;   
    private bool isAlive = true;

    [SerializeField] private float Speed;
    [SerializeField] private GameObject model;
    [SerializeField] private Transform weapons;
    [SerializeField] private float maxHealth;

    //DELETE
    private bool fill = true;
    public WeaponScript GetSelectedWeapon() { return _selectedWeapon; }
    void Start()
    {
        Physics.queriesHitTriggers = true;

        _transform = GetComponent<Transform>();
        _modelTransform = model.GetComponent<Transform>();
        _animator = model.GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _interface = GameObject.Find("Canvas").GetComponent<InterfaceScript>();
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        if(isAlive)
            MovementLogic();
    }
    private void Update()
    {
        //DELETE
        if (fill)
        {
            WeaponSwitch(0);
            _selectedWeapon.FillAmmo();
            WeaponSwitch(1);
            _selectedWeapon.FillAmmo();
            WeaponSwitch(2);
            _selectedWeapon.FillAmmo();
            WeaponSwitch(0);
            fill = false;
        }

        if (isAlive)
        {
            ShootingLogic();
            LookingLogic();
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                _selectedWeapon.setIsReloading(false);
                WeaponSwitch(0); 
                _animator.SetInteger("selectedWeapon", 0);             
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) 
            {
                _selectedWeapon.setIsReloading(false);
                WeaponSwitch(1); 
                _animator.SetInteger("selectedWeapon", 1); 
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _selectedWeapon.setIsReloading(false);
                WeaponSwitch(2); 
                _animator.SetInteger("selectedWeapon", 2); 
            }
        }

    }
    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = Quaternion.Euler(0, 45, 0) * movement;

        _controller.Move(movement * Speed * Time.fixedDeltaTime);

        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
            _animator.SetBool("isMoving", true);
        else
            _animator.SetBool("isMoving", false);
    }
    private void ShootingLogic()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_selectedWeapon.TryShot())
            {
                RaycastHit hit;
                Ray shot = new Ray(_transform.position, _modelTransform.forward);
                if (Physics.Raycast(shot, out hit, Mathf.Infinity, int.MaxValue, QueryTriggerInteraction.UseGlobal) && hit.transform.GetComponent<GeneralMonsterScript>() != null)
                {
                    hit.transform.GetComponent<GeneralMonsterScript>().GetHit(_selectedWeapon.getDamage());
                }
            }        
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
            _selectedWeapon.setNoAmmoPressed(false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            _selectedWeapon.TryReload();
        }
    }
    private void WeaponSwitch(int weaponIndex)
    {
        for(int i = 0; i < weapons.childCount; i++)
        {
            if(i == weaponIndex)
            {
                weapons.GetChild(i).gameObject.SetActive(true);
                _selectedWeapon = weapons.GetChild(i).gameObject.GetComponent<WeaponScript>();
                _interface.RefreshAmmoInfo(_selectedWeapon);
            }
            else
            {
                weapons.GetChild(i).gameObject.GetComponent<AudioSource>().clip = null;
                weapons.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void LookingLogic()
    {
        Plane plane = new Plane(Vector3.up, _transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out hitDistance)) {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            _modelTransform.rotation = targetRotation;
        }
    }
    public bool GetHit(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            isAlive = false;

        _interface.ChangeHealthBar(currentHealth, maxHealth);

        return isAlive;
    }
}
