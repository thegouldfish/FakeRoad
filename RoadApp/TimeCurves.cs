


namespace Sentia.Maths
{
	/******************************************
	 * 
	 *  Class of static time curve functions
	 * 
	 *  Each takes in a start, end and time amount (0.0f to 1.0f).
	 * 
	 * 
	 ********************************************/

	public enum TimeCurveTypes
	{
		kLinear,
		kEaseIn,
		kEaseOut,
		kEaseInOut
	}

	public class TimeCurves
	{
		public static float pow3(float x)
		{
			return x * x * x;
		}

		public static float EaseIn (float start, float end, float time) 
		{
		  return end * pow3(time) + start;
		}
		 
		public static float EaseOut (float start, float end, float time) 
		{
		  return end*(pow3(time-1.0f) + 1.0f) + start;
		}
		 
		public static float EaseInOut (float start, float end, float time) 
		{
			if ( (time * 2.0f) < 1.0f)
			  return end / 2.0f * pow3(time * 2.0f) + start;
			else
			  return end / 2.0f * (pow3((time * 2.0f) - 2.0f) + 2.0f) + start;
		}
	}
}