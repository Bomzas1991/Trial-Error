using UnityEngine;
using UnityEngine.Events;
public class Shake : MonoBehaviour
{
	public float strength = 0.25f;
	public bool shakeForever;
	public float duration = 0.3f;
	public float secondsLeft = -1f;
	public bool invokeOnStart;
	Vector3 originalPos;
	Vector3 lastFrameOffset;
	Vector3 offset;

	public UnityEvent<float> onShakeStart; // param: how many seconds will the shake last
	public UnityEvent onShakeEnd;

	void Start()
	{
		if (invokeOnStart)
		{
			StartShake();
		}
	}

	void Update()
	{
		if (shakeForever) secondsLeft = Time.deltaTime;
		if (secondsLeft <= 0) return;
		
		offset = Random.insideUnitSphere * strength; // get new random offset
		offset.z = 0; // we don't want to change the z axis

		var tr = transform;
		originalPos = tr.position;
		tr.position = originalPos - lastFrameOffset + offset; // cancel out previous frame offset and apply new one

		if (shakeForever) return;
		secondsLeft -= Time.deltaTime;
		if (secondsLeft <= 0)
		{
			StopShake();
		}
	}

	void LateUpdate()
	{
		lastFrameOffset = offset;
	}

	public void StartShake()
	{
		secondsLeft = duration;
		onShakeStart.Invoke(shakeForever ? -1f : secondsLeft);
	}
	public void StartShake(float time)
	{
		secondsLeft = time;
		onShakeStart.Invoke(shakeForever ? -1f : secondsLeft);
	}

	public void StopShake()
	{
		secondsLeft = -1f;
		transform.position = originalPos - lastFrameOffset;
		lastFrameOffset = Vector3.zero;
		onShakeEnd.Invoke();
	}
}
