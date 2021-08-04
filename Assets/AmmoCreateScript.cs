using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCreateScript : MonoBehaviour
{
    private GameObject player;
    private Transform _transform;
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        player = GameObject.Find("Player");
        _transform = GetComponent<Transform>();
    }
    private void Update()
    {

    }
}
