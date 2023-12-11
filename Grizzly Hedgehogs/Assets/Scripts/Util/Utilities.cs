using System.Collections.Generic;
using UnityEngine;

public static class Utillities 
{

	
	static List<Timer> timers = null;


	


	/// <summary>
	/// Creates a new timer.
	/// </summary>
	/// <param name="startTimeInSeonds"></param>
	static public void CreateGlobalTimer(float startTimeInSeonds, ref int id) 
	{
		if (timers == null) timers = new List<Timer>();
		id = timers.Count;
		timers.Add(new Timer(startTimeInSeonds));
	}

	/// <summary>
	/// Used for checking the if the timer is finished.
	/// </summary>
	/// <returns>True if the timer is finsihed. Will return false if the timer isn't started</returns>
	static public bool IsGlobalTimerDone(int id) 
	{
		if(timers == null) return false;
		return timers[id].IsDone;
	}

	/// <summary>
	/// Updates the global timer.
	/// </summary>
	static public void UpdateGlobalTimer(int id) 
	{
		if(timers == null || timers.Count < id) return;
		timers[id].Update(Time.deltaTime);
	}

	/// <summary>
	/// Resets a global timer
	/// </summary>
	/// <param name="id"></param>
	/// <param name="newStartTime"></param>
	static public void ResetGlobalTimer(int id, float newStartTime = 0) 
	{
		if(newStartTime > 0) timers[id].StartTime = newStartTime;
		timers[id].Reset();
	}

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static long GetBitIndex(long x)
	{
		return GetEarliestSetBitIndex64(x);
	}
	

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static ulong GetBitIndex(ulong x)
	{
		return GetEarliestSetBitIndex64(x);
	}

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static uint GetBitIndex(uint x) 
	{
		return (uint) GetEarliestSetBitIndex64(x);
	}

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static int GetBitIndex(int x) 
	{
		return (int) GetEarliestSetBitIndex64(x);
	}

		
	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static ushort GetBitIndex(ushort x) 
	{
		return (ushort) GetEarliestSetBitIndex64(x);
	}

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static short GetBitIndex(short x) 
	{
		return (short) GetEarliestSetBitIndex64(x);
	}

		
	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static byte GetBitIndex(byte x) 
	{
		return (byte) GetEarliestSetBitIndex64(x);
	}

	/// <summary>
	/// Used to get the earliest set bit. 
	/// Example: 8 = 0000_1000, returns 4. 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>bit index</returns>
	public static sbyte GetBitIndex(sbyte x) 
	{
		return (sbyte) GetEarliestSetBitIndex64(x);
	}

	private static ulong GetEarliestSetBitIndex64(ulong x) 
	{
		//Reference used: https://stackoverflow.com/questions/10439242/count-leading-zeroes-in-an-int32
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x |= x >> 32;


		x -= x >> 1 & 0x5555555555555555;
		x = (x >> 2 & 0x3333333333333333) + (x & 0x3333333333333333);
		x = (x >> 4) + x & 0x0F0F0F0F0F0F0F0F;

		x += x >> 8;
		x += x >> 16;
		x += x >> 32;
		

		return 64 - (x & 0x000000000000003F);
	}

	private static long GetEarliestSetBitIndex64(long x)
	{
		//Reference used: https://stackoverflow.com/questions/10439242/count-leading-zeroes-in-an-int32
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x |= x >> 32;


		x -= x >> 1 & 0x5555555555555555;
		x = (x >> 2 & 0x3333333333333333) + (x & 0x3333333333333333);
		x = (x >> 4) + x & 0x0F0F0F0F0F0F0F0F;

		x += x >> 8;
		x += x >> 16;
		x += x >> 32;


		return x & 0x000000000000003F;
	}

}
