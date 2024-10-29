using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingUI : MonoBehaviour
{
    TextMeshPro textFloating;
    float offsetY = 0.5f;
    float destoryTime = 2f;
    float moveSpeed = 1.5f;
    Color alpha;
   

    private void Awake()
    {
        textFloating = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        alpha = textFloating.color;
        Invoke("DestroyObj", destoryTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);
        alpha.a = Mathf.Lerp(alpha.a, 0f, Time.deltaTime);
        textFloating.color = alpha;
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }

    public void SetTextDamage(int damage)
    {
        textFloating.text = damage.ToString();
    }
}
