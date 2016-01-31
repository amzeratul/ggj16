/*
MathX for Unity by Alec Holowka
http://InfiniteAmmo.com - @infinite_ammo
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RandomFloat
{
	public float min;
	public float max;
	float value;
	bool picked;

	public float GetPicked()
	{
		if (!picked)
			Pick();
		return value;
	}

	public float Get()
	{
		return Random.Range(min, max);
		//return picked ? value : Random.Range(min, max);
	}

    public float Pick()
	{
		value = Random.Range(min, max);
		picked = true;
		return value;
	}
}

public class MathX
{
	const float PI = 3.14159265359f;
	const float TAU = 6.28318530718f;
	
	static public float Bias(float time, float bias)
	{
		return (time / ((((1f/bias) - 2f)*(1f - time))+1f));
	}
	
	static public float Gain(float time, float gain)
	{
		if(time < .5f)
			return Bias(time * 2f, gain) / 2f;
		else
			return Bias(time * 2f - 1f, 1f - gain) / 2f + .5f;
	}
	
	static public float PingPong(float progress, float period = 1f)
	{
		int num = (int)(progress / period);
		float diff = progress - (num * period);
		if (num % 2 == 0)
			return diff / period;
		return 1f - (diff / period);
	}
	
	public static float Map(float t, float min, float max)
	{
		return (t * (max-min)) + min;
	}

	public static void Constrain(ref float num, float min, float max)
	{
		if (num < min) 		num = min;
		else if (num > max) num = max;
	}
	
	public static float Wave(float p)
	{
		return Mathf.Sin(p * TAU);
	}
	
	public static float Hill(float p)
	{
		return Mathf.Sin(p * PI);
	}
	
	static public float QuadIn(float t) { return t * t; }
	static public float QuadOut(float t) { return 1 - QuadIn(1 - t); }
	static public float QuadInOut(float t) { return (t <= 0.5f) ? QuadIn(t * 2) / 2 : QuadOut(t * 2 - 1) / 2 + 0.5f; }
	static public float CubeIn(float t) { return t * t * t; }
	static public float CubeOut(float t) { return 1 - CubeIn(1 - t); }
	static public float CubeInOut(float t) { return (t <= 0.5f) ? CubeIn(t * 2) / 2 : CubeOut(t * 2 - 1) / 2 + 0.5f; }
	static public float BackIn(float t) { return t * t * (2.70158f * t - 1.70158f); }
	static public float BackOut(float t) { return 1 - BackIn(1 - t); }
	static public float BackInOut(float t) { return (t <= 0.5f) ? BackIn(t * 2) / 2 : BackOut(t * 2 - 1) / 2 + 0.5f; }
	static public float ExpoIn(float t) { return Mathf.Pow(2, 10 * (t - 1)); }
	static public float ExpoOut(float t) { return 1 - ExpoIn(t); }
	static public float ExpoInOut(float t) { return t < .5f ? ExpoIn(t * 2) / 2 : ExpoOut(t * 2) / 2; }
	static public float ExpoCurve(float t)
	{
		if (t < .5f)
			return ExpoOut(1f-t);
		return ExpoOut(t);
	}
	static public float SineIn(float t) { return -Mathf.Cos(Mathf.PI/2 * t) + 1; }
	static public float SineOut(float t) { return Mathf.Sin(Mathf.PI/2 * t); }
	static public float SineInOut(float t) { return -Mathf.Cos(Mathf.PI * t) / 2f + .5f; }
	static public float ElasticIn( float t, float d )
	{
		if( t == 0 )
			return 0;
		
		if( ( t /= d ) == 1 )
			return 1;
		
		float p = d * .3f;
		float s = p / 4;
		return -( 1 * Mathf.Pow( 2, 10 * ( t -= 1 ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) );
	}
	static public float ElasticOut( float t, float d )
	{
		if( t == 0 )
			return 0;
		
		if( ( t /= d ) == 1 )
			return 1;
		
		float p = d * .3f;
		float s = p / 4;
		return ( 1 * Mathf.Pow( 2, -10 * t ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) + 1 );
	}
	static public float ElasticInOut( float t, float d )
	{
		if( t == 0 )
			return 0;
		
		if( ( t /= d / 2 ) == 2 )
			return 1;
		
		float p = d * ( .3f * 1.5f );
		float s = p / 4;
		
		if( t < 1 )
			return -.5f * ( Mathf.Pow( 2, 10 * ( t -= 1 ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) );
		
		return ( Mathf.Pow( 2f, -10f * ( t -= 1f ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) * 0.5f + 1f );
	}
}