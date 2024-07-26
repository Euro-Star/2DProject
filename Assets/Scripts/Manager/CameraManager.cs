using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    public Vector2 mapSize;

    Vector3 cameraOffset = new Vector3(0, 0, -10);
    Vector2 Center = Vector2.zero;
    float height, width;


    private void Awake()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = targetTransform.position + cameraOffset;

        float limit_x = mapSize.x - width;
        float limit_y = mapSize.y - height;

        float clamp_x = Mathf.Clamp(transform.position.x, Center.x - limit_x, Center.x + limit_x);
        float clamp_y = Mathf.Clamp(transform.position.y, Center.y - limit_y, Center.y + limit_y);

        transform.position = new Vector3(clamp_x, clamp_y, cameraOffset.z);
    }
}
