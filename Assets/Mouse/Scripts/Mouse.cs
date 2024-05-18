using System;
using UnityEngine;
using UnityEngine.Events;

public class Mouse : MonoBehaviour
{
	// Singleton
	public static Mouse Instance { get; private set; }

	public MouseState mouseState = MouseState.Default;
	public float defaultScale = 0.3f;
	public float multiplier = 1.25f;
	public float scaleSpeedSeconds = 0.05f;
	public Vector3 offset;
	Camera camera;
	MouseState lastFrameMouseState;

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
	}
	void Update()
	{
		MoveMouse();
		ScaleMouse();

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
}
