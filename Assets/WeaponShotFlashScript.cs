using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class WeaponShotFlashScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Flash());
    }
    IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.03f);
        Destroy(this.gameObject);
    }
}
