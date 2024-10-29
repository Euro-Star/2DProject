using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHitEffect : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
