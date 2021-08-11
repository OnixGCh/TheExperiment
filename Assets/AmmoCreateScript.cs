using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCreateScript : MonoBehaviour
{
    /*private GameObject player;
    private PlayerScript _playerScript;
    private InterfaceScript _interface;
    private Transform _transform;
    private float timer = 3f;
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        player = GameObject.Find("Player");
        _playerScript = player.GetComponent<PlayerScript>();
        _transform = GetComponent<Transform>();
        _interface = GameObject.Find("Canvas").GetComponent<InterfaceScript>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.E) 
        && Vector3.Distance(_transform.position, player.transform.position) < 3f 
        && _playerScript.GetSelectedWeapon().getAmmoInventory() != _playerScript.GetSelectedWeapon().getInventoryMaxAmmo())
        {
            _interface.ActionProgressBarEnable();
            _interface.SetActionProgressBarValue((3f - timer) / 3f);
            timer -= Time.deltaTime;
        }
        
        if(Input.GetKeyUp(KeyCode.E))
        {
            timer = 3f;
            _interface.ActionProgressBarDisable();
        }

        if(timer <= 0)
        {
            _playerScript.GetSelectedWeapon().FillAmmo();
            timer = 3f;
            _interface.ActionProgressBarDisable();
        }
    }*/
}
