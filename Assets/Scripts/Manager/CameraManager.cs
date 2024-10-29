using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    private void FixedUpdate()
    {
        transform.position = targetTransform.position + cameraOffset;

        float limit_x = mapSize.x - width;
        float limit_y = mapSize.y - height;

        if(limit_x < 0) { limit_x= 0; }
        if(limit_y < 0) { limit_y= 0; }

        float clamp_x = Mathf.Clamp(transform.position.x, Center.x - limit_x, Center.x + limit_x);
        float clamp_y = Mathf.Clamp(transform.position.y, Center.y - limit_y, Center.y + limit_y);

        transform.position = new Vector3(clamp_x, clamp_y, cameraOffset.z);
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Stage_5")
        {
            mapSize = new Vector2(12.45f, 7f);
        }
        else
        {
            mapSize = new Vector2(20.4f, 13.5f);
        }
    }
}
