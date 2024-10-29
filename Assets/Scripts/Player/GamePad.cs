using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform rectBack;
    [SerializeField] private RectTransform rectJoystick;

    float fRadius;

    Vector2 vecNormal;
    bool bTouch = false;


    void Start()
    {
        fRadius = rectBack.rect.width * 0.5f;
    }

    void Update()
    {
        if (Player.player.IsDeath())
        {
            Player.player.SetInputVec(Vector2.zero);
            return;
        }

        if (bTouch)
        {
            Player.player.SetInputVec(vecNormal);
        }

    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - rectBack.position.x, vecTouch.y - rectBack.position.y);
        vec = Vector2.ClampMagnitude(vec, fRadius);

        rectJoystick.localPosition = vec;
        vecNormal = vec.normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        bTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Player.player.SetInputVec(Vector2.zero);
        rectJoystick.localPosition = Vector2.zero;
        bTouch = false;
    }
}
