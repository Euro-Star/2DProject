using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSignSquare : MonoBehaviour
{
    [SerializeField] private SpriteRenderer warningSquare;

    public event EventHandler warningCompEvent;

    private float completeTime;

    public void WarningSquare(float thick, float completeTime)
    {
        warningSquare.transform.localScale = new Vector3(30.0f, thick, 1.0f);
        this.completeTime = completeTime;

        StartCoroutine(DelayWarning());
    }

    private IEnumerator DelayWarning()
    {
        yield return new WaitForSeconds(completeTime);

        warningCompEvent?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }
}
