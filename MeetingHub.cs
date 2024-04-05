using Microsoft.AspNetCore.SignalR;
using System.Timers;
using Timer = System.Timers.Timer;

namespace EATMeeting;

public class MeetingHub : Hub
{
    private static List<string> UserQueue = new List<string>();
    private static Timer timer;
    private static int remainingTimeInSeconds;
    private readonly TimerService _timerService;
    
    public MeetingHub(TimerService timerService)
    {
        _timerService = timerService;
    }
    
    public async Task StartTimer(int minutes)
    {
        _timerService.StartTimer(minutes);
    }

    public async Task QueueUser(string name)
    {
        // Add user to queue and notify all clients
        UserQueue.Add(name);
        await Clients.All.SendAsync("UpdateQueue", UserQueue);
    }

    public async Task RemoveFirstUser()
    {
        // Remove the first user from the queue and notify all clients
        if (UserQueue.Count > 0)
        {
            UserQueue.RemoveAt(0);
            await Clients.All.SendAsync("UpdateQueue", UserQueue);
        }
    }
}