using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    private Transform _transform;
    private Transform[] spawns = new Transform[3];
    private bool spawned = false;

    [SerializeField] GameObject Hunter;
    void Start()
    {
        _transform = GetComponent<Transform>();

        for (int i = 0; i < _transform.childCount; i++)
        {
            spawns[i] = _transform.GetChild(i);
        }
    }
    void Update()
    {
        if (!spawned)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        spawned = true;
        Transform spawn = spawns[Random.Range(0, 3)];
        Instantiate(Hunter, spawn.position, spawn.rotation);
        yield return new WaitForSeconds(5);
        spawned = false;
    }
}
