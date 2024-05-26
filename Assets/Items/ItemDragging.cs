using UnityEngine;

public class ItemDragging : MonoBehaviour
{
    Mouse mouse;
    Vector3 offset;
    public bool isDragging;
    
    void Start()
    {
        mouse = Mouse.Instance;
    }

    void OnMouseDown()
    {
        offset = transform.position - mouse.transform.position;
    }

    void OnMouseDrag()
    {
        transform.position = mouse.transform.position + offset;
        mouse.SetDragging(true);
        isDragging = true;
    }

    void OnMouseUp()
    {
        mouse.SetDragging(false);
        isDragging = false;
    }
}
