using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class WarningSign : MonoBehaviour
{
    [SerializeField] SpriteRenderer warningCircle;
    [SerializeField] SpriteRenderer fillCircle;

    private float currnetTime;
    private float completeTime;
    private float circleScale;

    public event EventHandler warningCompEvent;

    public void WarningCircle(float scale, Vector3 position, float completeTime)
    {
        warningCircle.transform.localScale = new Vector3(scale, scale, scale);
        currnetTime = 0f;
        this.completeTime = completeTime;

        StartCoroutine(FillWarning());
    }

    private IEnumerator FillWarning()
    {
        while(currnetTime < completeTime)
        {
            currnetTime += Time.deltaTime;
            circleScale = Mathf.Lerp(0f, 1f, currnetTime / completeTime);
            fillCircle.transform.localScale = new Vector3(circleScale, circleScale, circleScale);

            yield return null;
        }

        warningCompEvent?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }
}
