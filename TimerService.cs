using Microsoft.AspNetCore.SignalR;
using System.Timers;
using EATMeeting;
using Timer = System.Timers.Timer;

public class TimerService
{
    private readonly IHubContext<MeetingHub> _hubContext;
    private Timer _timer;
    private static int _remainingTimeInSeconds;

    public TimerService(IHubContext<MeetingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void StartTimer(int minutes)
    {
        _remainingTimeInSeconds = minutes * 60;

        _timer?.Stop();
        _timer?.Dispose();

        _timer = new Timer(1000) { AutoReset = true };
        _timer.Elapsed += TimerElapsed;
        _timer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        _remainingTimeInSeconds--;

        if (_remainingTimeInSeconds >= 0)
        {
            var minutes = _remainingTimeInSeconds / 60;
            var seconds = _remainingTimeInSeconds % 60;
            _hubContext.Clients.All.SendAsync("UpdateTime", _remainingTimeInSeconds);
        }
        else
        {
            _timer?.Stop();
            // Optionally, notify clients the timer has finished
            _hubContext.Clients.All.SendAsync("TimerFinished");
        }
    }

    // Implement other methods as needed
}