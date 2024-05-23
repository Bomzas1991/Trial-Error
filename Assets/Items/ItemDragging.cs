using UnityEngine;

public class ItemDragging : MonoBehaviour
{
    public bool isBeingDragged;
    Mouse mouse;

    private void Start()
    {
        mouse = Mouse.Instance;
    }

    private void OnMouseDrag()
    {
        transform.position = mouse.transform.position;
        mouse.SetDragging(true);
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        mouse.SetDragging(false);
    }
}
