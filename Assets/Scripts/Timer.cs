using UnityEngine;

public class Timer
{
    private bool _isRunning;
    private bool _isPaused;
    private float _start;
    private readonly float _interval;
    private float _remaining;

    public Timer(float end)
    {
        _interval = end;
    }

    public void Start()
    {
        if (_isPaused)
        {
            Resume();
            return;
        }

        _start = Time.time;
        _isRunning = true;
    }

    public void Pause()
    {
        if (!_isRunning) return;
        _remaining = (_start + _interval) - Time.time;
        _isRunning = false;
    }

    private void Resume()
    {
        //
    }

    public bool HasEnded()
    {
        return _start + _interval <= Time.time;
    }

    public bool IsRunning()
    {
        if (!_isRunning || HasEnded()) return false;
        return true;
    }
}