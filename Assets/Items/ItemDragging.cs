using UnityEngine;

public class ItemDragging : MonoBehaviour
{
    Mouse mouse;
    Vector3 offset;
    
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
    }

    void OnMouseUp()
    {
        mouse.SetDragging(false);
    }
}
