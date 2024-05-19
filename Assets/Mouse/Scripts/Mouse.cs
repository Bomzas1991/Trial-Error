using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Mouse : MonoBehaviour
{
	// Singleton
	public static Mouse Instance { get; private set; }

	public MouseState mouseState = MouseState.Default;
	public float defaultScale = 0.3f;
	public float multiplier = 1.25f;
	public float scaleSpeedSeconds = 0.05f;
	public Vector3 offset;
	public List<Sprite> sprites; // 0 - default; 1 - wait
	Camera camera;
	MouseState lastFrameMouseState;
	float timeUntilNextRotation; // loading mouse rotation
	SpriteRenderer childRend;

	// Param: last frame mouse state
	// Param: new mouse state
	public UnityEvent<MouseState, MouseState> onMouseStateChange;
	public UnityEvent onMouseClick;

	public enum MouseState
	{
		Default,
		Hover, // TODO: Implement object hovering
		Drag, // TODO: Implement object dragging
		Wait, // TODO: Implement waiting for cutscenes
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
		ScaleMouse();
		AnimateMouse();
		ChangeMouseTexture();

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
		var pos = camera.ScreenToWorldPoint(Input.mousePosition) + offset;
		pos.z = 0;
		transform.position = pos;
	}

	void ScaleMouse()
	{
		// if mouse state isn't default or wait => scale mouse
		var multiply = mouseState is not (MouseState.Default or MouseState.Wait) ? multiplier : 1f;
		// smoothly scale mouse cursor
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (defaultScale * multiply), Time.deltaTime / scaleSpeedSeconds); // TODO: Replace with easing library
	}
	
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
	}

	void ChangeMouseTexture()
	{
		if (mouseState == MouseState.Wait)
		{
			childRend.sprite = sprites[1];
			return;
		}
		childRend.sprite = sprites[0];
	}
}
