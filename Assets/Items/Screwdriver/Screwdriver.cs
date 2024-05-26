using UnityEngine;

public class Screwdriver : MonoBehaviour
{
    [SerializeField] GameObject screwdriver;
    int mouseClicks;

    private void OnMouseDown()
    {
        if (mouseClicks < 7)
        {
            mouseClicks++;
            transform.Rotate(new Vector3(0, 0, -90f));
            return;
        }
        screwdriver.SetActive(true);
        Destroy(gameObject);
    }
}
