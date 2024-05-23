using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Mouse : MonoBehaviour
{
	// Singleton
	public static Mouse Instance { get; private set; }

	[Header("Mouse Settings")]
	public MouseState mouseState = MouseState.Default;
	public float defaultScale = 0.3f;
	public float multiplier = 1.25f;
	public float scaleSpeedSeconds = 0.05f;
	public Vector3 positionOffset;
	public List<Sprite> sprites; // 0 - default; 1 - wait
	Camera camera;
	MouseState lastFrameMouseState;
	float timeUntilNextRotation; // loading mouse rotation
	SpriteRenderer childRend;

	[SerializeField]bool isDragging, isHovering, isCutscene;
	[SerializeField]int interactableCount;

	// Param: last frame mouse state
	// Param: new mouse state
	public UnityEvent<MouseState, MouseState> onMouseStateChange;
	public UnityEvent onMouseClick;

	public enum MouseState
	{
		Default,
		Hover,
		Drag,
		Wait,
	}
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		Cursor.visible = false;
		camera = Camera.main;
		
		childRend  = transform.GetChild(0).GetComponent<SpriteRenderer>(); // watch out!
	}

	void Update()
	{
		MoveMouse();
		DetectItemHover();
		ScaleMouse();
		AnimateMouse();
		ChangeMouseIcon();
		ChangeMouseState();

		if (Input.GetMouseButtonDown(0))
		{
			onMouseClick.Invoke();
		}

		if (lastFrameMouseState != mouseState)
		{
			onMouseStateChange.Invoke(lastFrameMouseState, mouseState);
		}
	}

    void LateUpdate()
	{
		lastFrameMouseState = mouseState;
	}

	void MoveMouse()
	{
		var pos = camera.ScreenToWorldPoint(Input.mousePosition) + positionOffset;
		pos.z = 0.1f; // weird offset, so mouse can drag items
		transform.position = pos;
	} // Move mouse to mouse cursor

	void ScaleMouse()
	{
		// if mouse state isn't default or wait => scale mouse
		var multiply = mouseState is (MouseState.Hover or MouseState.Drag) ? multiplier : 1f;
		// smoothly scale mouse cursor
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (defaultScale * multiply), Time.deltaTime / scaleSpeedSeconds); // TODO: Replace with easing library
	} // Scale mouse when hovering/dragging
	
	void AnimateMouse()
	{
		if (mouseState != MouseState.Wait)
		{
			transform.rotation = Quaternion.identity;
			return;
		}
		// Loading Mouse Rotation
		if (timeUntilNextRotation <= 0)
		{
			timeUntilNextRotation = 0.5f;
			transform.Rotate(new Vector3(0, 0, 90f)); // Might need to rotate the children instead
		}
		timeUntilNextRotation -= Time.deltaTime;
	} // Animate loading mouse (rotation)

	void ChangeMouseIcon()
	{
		if (mouseState == MouseState.Wait)
		{
			childRend.sprite = sprites[1];
			return;
		}
		childRend.sprite = sprites[0];
	} // Switch between mouse loading and default

	void ChangeMouseState()
	{
        if (isDragging)
		{
			mouseState = MouseState.Drag;
			return;
		}
		if (isHovering)
		{
			mouseState = MouseState.Hover;
			return;
		}
		if (isCutscene)
		{
			mouseState = MouseState.Wait;
			return;
		}
		mouseState = MouseState.Default;
	} // Set mouseState to correct MouseState

	void DetectItemHover()
    {
		// TODO: Fix bugs
        switch (interactableCount)
        {
            case > 0:
                isHovering = true;
                break;
            case <= 0:
				isHovering = false;
                interactableCount = 0;
                break;
        }
    } // Detect if mouse is hovering over an item

    private void OnTriggerEnter2D(Collider2D other)
    {
		interactableCount++;
	} // TODO: shoot a raycast/circlecast instead of a collider

	private void OnTriggerExit2D(Collider2D collision)
    {
		interactableCount--;
    }

    public void SetDragging(bool _isDragging)
    {
		isDragging = _isDragging;
    }
}
