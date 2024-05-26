using UnityEngine;

public class Screw : MonoBehaviour
{
    Mouse mouse;
    [SerializeField] bool mouseOver;
    [SerializeField] GameObject screwdriver;
    ItemDragging sdDrag;
    [SerializeField] bool unscrewing;
    [SerializeField] float timeLeft;

    private void Start()
    {
        mouse = Mouse.Instance;
        sdDrag = screwdriver.GetComponent<ItemDragging>();
    }
    private void OnMouseEnter()
    {
        if (mouse.mouseState != Mouse.MouseState.Drag) return;
        if (!sdDrag.isDragging) return;
        mouseOver = true;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            if (!mouseOver)
            {
                return;
            }
            mouseOver = false;
            AudioManager.Instance.SetVolume(0.5f);
            AudioManager.Instance.Play(0);
            Destroy(gameObject);
        }
    }
}
