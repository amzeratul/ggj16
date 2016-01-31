/*
Tween for Unity by Alec Holowka
http://InfiniteAmmo.com - @infinite_ammo
*/

using UnityEngine;
using System.Collections;

public class TweenBase : MonoBehaviour
{
}

public class TweenVolume : TweenBase
{
	public float start;
	public float end;
	public float time;
	public Tween.EaseType easeType;
	
	private float timer;
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		GetComponent<AudioSource>().volume = start + (end - start) * Tween.EaseValue(easeType, timer/time);
		if (timer >= time)
			Destroy(this);
	}
}


public class TweenMove : TweenBase
{
	public Vector3 start;
	public Vector3 end;
	public float time;
	public Tween.EaseType easeType;
	public bool isLocal;
	
	private float timer;
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		if (isLocal)
			transform.localPosition = start + (end - start) * Tween.EaseValue(easeType, timer/time);
		else
			transform.position = start + (end - start) * Tween.EaseValue(easeType, timer/time);
		if (timer >= time)
			Destroy(this);
	}
}

public class TweenWobbler : TweenBase
{
	public float amount;
	public Vector3 axis;
	public Vector3 start;
	public float time;
	private float timer;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;

		float p = timer/time;

		transform.localScale = start
			+ Mathf.Sin(p * Mathf.PI * 2f) * Vector3.right * (1f-p)
			+ Mathf.Sin(p * Mathf.PI * 2f + Mathf.PI) * Vector3.right * (1f-p);
	}
}

public class TweenScale : TweenBase
{
	public Vector3 start;
	public Vector3 end;
	public float time;
	public Tween.EaseType easeType;
	
	float timer;
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		transform.localScale = start + (end - start) * Tween.EaseValue(easeType, timer/time);
		if (timer >= time)
			Destroy(this);
	}
}

public class TweenPulseScale : TweenBase
{
	public Vector3 start, end;
	public float time;
	public Tween.EaseType easeType;

	float timer;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		transform.localScale = start + (end - start) * Mathf.Sin((timer/time) * Mathf.PI);
		if (timer >= time)
			Destroy(this);
	}
}

public class TweenRotate : TweenBase
{
	public Quaternion start;
	public Quaternion end;
	public float time;
	public Tween.EaseType easeType;
	public bool isLocal;
	
	private float timer;
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		if (isLocal)
			transform.localRotation = Quaternion.Slerp(start, end, Tween.EaseValue(easeType, timer/time));
		else
			transform.rotation = Quaternion.Slerp(start, end, Tween.EaseValue(easeType, timer/time));
		if (timer >= time)
			Destroy(this);
	}
}

public class TweenColor : TweenBase
{
    public Color start;
    public Color end;
    public float time;
    public Tween.EaseType easeType;
    public string colorName;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > time)
            timer = time;
        GetComponent<Renderer>().material.SetColor(colorName, start + (end - start) * Tween.EaseValue(easeType, timer / time));
        if (timer >= time)
            Destroy(this);
    }
}

public class TweenSpriteColor : TweenBase
{
	public Color start;
	public Color end;
	public float time;
	public Tween.EaseType easeType;
	float timer;
	SpriteRenderer spriteRenderer;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
			timer = time;
		spriteRenderer.color = start + (end - start) * Tween.EaseValue(easeType, timer / time);
		if (timer >= time)
			Destroy(this);
	}
}

public class TweenSpin : TweenBase
{
	public float maxAngularSpeed;
	public float angularSpeed;
	public float acceleration;
	public float deceleration;
	public float time;
	public Vector3 axis;
	
	private int state;
	
	void Update()
	{
		if (state == 0)
		{
			angularSpeed += Time.deltaTime * acceleration;
			if (angularSpeed > maxAngularSpeed)
			{
				angularSpeed = maxAngularSpeed;
				state = 1;
			}
		}
		else if (state == 1)
		{
			time -= Time.deltaTime;
			if (time < 0f)
				state = 2;
		}
		else if (state == 2)
		{
			angularSpeed -= Time.deltaTime * deceleration;
			if (angularSpeed < 0f)
				Destroy(this);
		}
		gameObject.transform.localEulerAngles += angularSpeed * axis * Time.deltaTime;
	}	
}

public class TweenSpinToAngle : TweenBase
{
	public int revolutions;
	public float time;
	public Vector3 axis;

	public float startAngle;
	public float targetAngle;

	public Tween.EaseType easeType;

	float timer;

	void Update()
	{
		timer += Time.deltaTime;
        if (timer > time)
            timer = time;
        float p = Tween.EaseValue(easeType, timer/time);
        gameObject.transform.localEulerAngles = axis * ((targetAngle - startAngle) * p + startAngle);
        if (timer >= time)
            Destroy(this);
	}
}

public class Tween : MonoBehaviour
{
	public enum EaseType
	{
		Linear,
		BackIn,
		BackOut,
		BackInOut,
		SineIn,
		SineOut,
		SineInOut,
		CubeIn,
		CubeOut,
		CubeInOut,
		ElasticIn,
		ElasticOut,
		ElasticInOut,
		Max
	}
	
	public static TweenScale ScaleFrom(GameObject gameObject, Vector3 scale, float time, EaseType easeType)
	{
		if (time <= 0f)
		{
			gameObject.transform.localScale = scale;
			return null;
		}
		
		TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
		tweenScale.start = scale;
		tweenScale.end = gameObject.transform.localScale;
		tweenScale.time = time;
		tweenScale.easeType = easeType;
		gameObject.transform.localScale = scale;
		return tweenScale;
	}
	
	public static TweenScale ScaleTo(GameObject gameObject, Vector3 scale, float time, EaseType easeType)
	{
		if (time <= 0f)
		{
			gameObject.transform.localScale = scale;
			return null;
		}
		
		TweenScale tweenScale = (TweenScale)gameObject.AddComponent<TweenScale>();
		tweenScale.start = gameObject.transform.localScale;
		tweenScale.end = scale;
		tweenScale.time = time;
		tweenScale.easeType = easeType;
		return tweenScale;
	}
	
	public static TweenMove MoveFrom(GameObject gameObject, Vector3 position, float time, EaseType easeType, bool isLocal)
	{
		if (time <= 0f)
		{
			if (isLocal)
				gameObject.transform.localPosition = position;
			else
				gameObject.transform.position = position;
			return null;
		}
		
		TweenMove tweenMove = gameObject.AddComponent<TweenMove>();
		tweenMove.start = position;
		tweenMove.end = isLocal ? gameObject.transform.localPosition : gameObject.transform.position;
		tweenMove.time = time;
		tweenMove.easeType = easeType;
		tweenMove.isLocal = isLocal;
		if (isLocal)
			gameObject.transform.localPosition = position;
		else
			gameObject.transform.position = position;
		return tweenMove;
	}
	
	public static TweenMove MoveTo(GameObject gameObject, Vector3 position, float time, EaseType easeType)
	{
		return MoveTo(gameObject, position, time, easeType, false);
	}

	public static TweenMove MoveTo(GameObject gameObject, Vector3 position, float time, EaseType easeType, bool isLocal)
	{
		if (time <= 0f)
		{
			if (isLocal)
				gameObject.transform.localPosition = position;
			else
				gameObject.transform.position = position;
			return null;
		}
		
		TweenMove tweenMove = gameObject.AddComponent<TweenMove>();
		tweenMove.start = isLocal ? gameObject.transform.localPosition : gameObject.transform.position;
		tweenMove.end = position;
		tweenMove.time = time;
		tweenMove.easeType = easeType;
		tweenMove.isLocal = isLocal;
		return tweenMove;
	}
	
	public static TweenMove MoveToX(GameObject gameObject, float x, float time, EaseType easeType, bool isLocal)
	{
		return MoveTo(gameObject, new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z), time, easeType, isLocal);
	}
	
	public static TweenMove MoveToY(GameObject gameObject, float y, float time, EaseType easeType, bool isLocal)
	{
		return MoveTo(gameObject, new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z), time, easeType, isLocal);
	}
	
	public static TweenMove MoveToZ(GameObject gameObject, float z, float time, EaseType easeType, bool isLocal)
	{
		return MoveTo(gameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z), time, easeType, isLocal);
	}

    public static TweenMove MoveFromX(GameObject gameObject, float x, float time, EaseType easeType, bool isLocal)
    {
        return MoveFrom(gameObject, new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z), time, easeType, isLocal);
    }
    
    public static TweenMove MoveFromY(GameObject gameObject, float y, float time, EaseType easeType, bool isLocal)
    {
        return MoveFrom(gameObject, new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z), time, easeType, isLocal);
    }
    
    public static TweenMove MoveFromZ(GameObject gameObject, float z, float time, EaseType easeType, bool isLocal)
    {
        return MoveFrom(gameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z), time, easeType, isLocal);
    }

	public static TweenRotate RotateFrom(GameObject gameObject, Vector3 eulerAngles, float time, EaseType easeType, bool isLocal)
	{
		TweenRotate tweenRotate = gameObject.AddComponent<TweenRotate>();
		tweenRotate.start = Quaternion.Euler(eulerAngles);
		tweenRotate.end = isLocal ? gameObject.transform.localRotation : gameObject.transform.rotation;
		tweenRotate.time = time;
		tweenRotate.easeType = easeType;
		tweenRotate.isLocal = isLocal;
		if (isLocal)
			gameObject.transform.localRotation = Quaternion.Euler(eulerAngles);
		else
			gameObject.transform.rotation = Quaternion.Euler(eulerAngles);
		return tweenRotate;
	}
	
	public static TweenRotate RotateTo(GameObject gameObject, Vector3 eulerAngles, float time, EaseType easeType, bool isLocal)
	{
		TweenRotate tweenRotate = null;
		if (time > 0f)
		{
			tweenRotate = gameObject.AddComponent<TweenRotate>();
			tweenRotate.start = isLocal ? gameObject.transform.localRotation : gameObject.transform.rotation;
			tweenRotate.end = Quaternion.Euler(eulerAngles);
			tweenRotate.time = time;
			tweenRotate.easeType = easeType;
			tweenRotate.isLocal = isLocal;
		}
		else
		{
			if (isLocal)
				gameObject.transform.localRotation = Quaternion.Euler(eulerAngles);
			else
				gameObject.transform.rotation = Quaternion.Euler(eulerAngles);
		}
		return tweenRotate;
	}

    public static TweenColor ColorTo(GameObject gameObject, Color color, string colorName, float time, EaseType easeType)
    {
    	TweenColor tweenColor = null;
    	if (time > 0f)
    	{
	        tweenColor = gameObject.AddComponent<TweenColor>();
	        tweenColor.start = gameObject.GetComponent<Renderer>().material.GetColor(colorName);
	        tweenColor.end = color;
	        tweenColor.time = time;
	        tweenColor.easeType = easeType;
	        tweenColor.colorName = colorName;
	    }
	    else
	    {
	    	gameObject.GetComponent<Renderer>().material.SetColor(colorName, color);
	    }
        return tweenColor;
    }

    public static TweenColor ColorFrom(GameObject gameObject, Color color, string colorName, float time, EaseType easeType)
    {
        TweenColor tweenColor = gameObject.AddComponent<TweenColor>();
        tweenColor.start = color;
        tweenColor.end = gameObject.GetComponent<Renderer>().material.GetColor(colorName);
        tweenColor.time = time;
        tweenColor.easeType = easeType;
        tweenColor.colorName = colorName;
		gameObject.GetComponent<Renderer>().material.SetColor(colorName, color);
        return tweenColor;
    }

	public static TweenSpriteColor SpriteColorTo(GameObject gameObject, Color color, float time, EaseType easeType)
	{
		if (time == 0f)
		{
			gameObject.GetComponent<SpriteRenderer>().color = color;
			return null;
		}
		else
		{
			TweenSpriteColor tweenSpriteColor = gameObject.AddComponent<TweenSpriteColor>();
			tweenSpriteColor.start = gameObject.GetComponent<SpriteRenderer>().color;
			tweenSpriteColor.end = color;
			tweenSpriteColor.time = time;
			tweenSpriteColor.easeType = easeType;
			return tweenSpriteColor;
		}
	}
	
	public static TweenSpriteColor SpriteColorFrom(GameObject gameObject, Color color, float time, EaseType easeType)
	{
		if (time == 0f)
		{
			return null;
		}
		else
		{
			TweenSpriteColor tweenSpriteColor = gameObject.AddComponent<TweenSpriteColor>();
			tweenSpriteColor.start = color;
			tweenSpriteColor.end = gameObject.GetComponent<SpriteRenderer>().color;
			tweenSpriteColor.time = time;
			tweenSpriteColor.easeType = easeType;
			return tweenSpriteColor;
		}
	}
	
	public static TweenSpin Spin(GameObject gameObject, float maxAngularSpeed, float accelerationTime, float decelerationTime, float time, Vector3 axis) 
	{
		TweenSpin tweenSpin = gameObject.AddComponent<TweenSpin>();
		tweenSpin.maxAngularSpeed = maxAngularSpeed;
		tweenSpin.acceleration = maxAngularSpeed / accelerationTime;
		tweenSpin.deceleration = maxAngularSpeed / decelerationTime;
		tweenSpin.time = time;
		tweenSpin.axis = axis;
		return tweenSpin;
	}

	public static TweenSpinToAngle SpinToAngle(GameObject gameObject, float toAngle, int revolutions, Vector3 axis, float time, Tween.EaseType easeType)
	{
		TweenSpinToAngle tweenSpinToAngle = gameObject.AddComponent<TweenSpinToAngle>();
		tweenSpinToAngle.revolutions = revolutions;
		tweenSpinToAngle.targetAngle = revolutions * 360f + toAngle;
		tweenSpinToAngle.time = time;
		tweenSpinToAngle.easeType = easeType;
		tweenSpinToAngle.axis = axis;
		tweenSpinToAngle.startAngle = Vector3.Scale(gameObject.transform.localEulerAngles, axis).magnitude;
		return tweenSpinToAngle;
	}

	public static TweenWobbler Wobble(GameObject gameObject, float amount, float time)
	{
		TweenWobbler tweenWobbler = gameObject.AddComponent<TweenWobbler>();
		tweenWobbler.start = gameObject.transform.localScale;
		tweenWobbler.amount = amount;
		tweenWobbler.time = time;
		return tweenWobbler;
	}
	
	public static TweenVolume VolumeFrom(GameObject gameObject, float volume, float time, EaseType easeType)
	{
		TweenVolume tweenVolume = gameObject.AddComponent<TweenVolume>();
		tweenVolume.start = volume;
		tweenVolume.end = gameObject.GetComponent<AudioSource>().volume;
		tweenVolume.time = time;
		tweenVolume.easeType = easeType;
		gameObject.GetComponent<AudioSource>().volume = volume;
		return tweenVolume;
	}
	
	public static TweenVolume VolumeTo(GameObject gameObject, float volume, float time, EaseType easeType)
	{
		TweenVolume tweenVolume = gameObject.AddComponent<TweenVolume>();
		tweenVolume.start = gameObject.GetComponent<AudioSource>().volume;
		tweenVolume.end = volume;
		tweenVolume.time = time;
		tweenVolume.easeType = easeType;
		return tweenVolume;
	}

	public static TweenPulseScale PulseScaleFromTo(GameObject gameObject, Vector3 start, Vector3 end, float time)
	{
		var tween = gameObject.AddComponent<TweenPulseScale>();
		tween.start = start;
		tween.end = end;
		tween.time=  time;
		return tween;
	}
	
	public static void Stop(GameObject gameObject)
	{
		foreach (TweenBase tweenBase in gameObject.GetComponents<TweenBase>())
		{
			Destroy(tweenBase);
		}
	}
	
	public static int Count(GameObject gameObject)
	{
		return gameObject.GetComponents<TweenBase>().Length;
	}
	
	public static float EaseValue(EaseType easeType, float t)
	{
		switch (easeType)
		{
		case EaseType.BackIn:
			return MathX.BackIn(t);
		case EaseType.BackOut:
			return MathX.BackOut(t);
		case EaseType.BackInOut:
			return MathX.BackInOut(t);
		case EaseType.SineIn:
			return MathX.SineIn(t);
		case EaseType.SineOut:
			return MathX.SineOut(t);
		case EaseType.SineInOut:
			return MathX.SineInOut(t);
		case EaseType.CubeIn:
			return MathX.CubeIn(t);
		case EaseType.CubeOut:
			return MathX.CubeOut(t);
		case EaseType.CubeInOut:
			return MathX.CubeInOut(t);
		case EaseType.ElasticIn:
			return MathX.ElasticIn(t, 1f);
		case EaseType.ElasticOut:
			return MathX.ElasticOut(t, 1f);
		case EaseType.ElasticInOut:
			return MathX.ElasticInOut(t, 1f);
		}
		return t;
	}
}
