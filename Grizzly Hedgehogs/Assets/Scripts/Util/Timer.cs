
public class Timer
{
	float startTime;
	float currentTime;
	bool isDone = false;

	public Timer(float _startTime)
	{
		this.startTime = _startTime;
		this.currentTime = _startTime;
	}

	/// <summary>
	/// Updates the current state of the timer.
	/// </summary>
	/// <param name="delta"></param>
	public void Update(float delta) 
	{
		if(!isDone)
		{
			currentTime -= delta;
			isDone = currentTime <= 0;
		}
	}

	public void Reset() 
	{
		currentTime = startTime;
		isDone = false;
	}

	/// <summary>
	/// Is true when the timer is finished.
	/// </summary>
	public bool IsDone { get => isDone;}

	/// <summary>
	/// Gets the complete amount of current seconds. (Use this for timers)
	/// </summary>
	public float CurrentTimeRawSeconds { get => currentTime;}

	/// <summary>
	/// Gets the wrapped amount of current seconds. (Use this for clocks)
	/// </summary>
	public float CurrentTimeSeconds { get => currentTime % 60;}

	/// <summary>
	/// Gets the complete amount of current minutes.
	/// </summary>
	public float CurrentTimeMinutes { get => currentTime / 60;}

	/// <summary>
	/// Start time of the timer.
	/// </summary>
	public float StartTime { get => startTime; set => startTime = value; }

}
