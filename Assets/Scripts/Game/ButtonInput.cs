using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 dir;

    private Player player => FindObjectOfType<Player>();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player) player.SetMoveDir(dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player) player.SetMoveDir(Vector3.zero);
    }
}
