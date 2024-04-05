using EATMeeting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<TimerService>();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MeetingHub>("/meetingHub");
});
// app.MapGet("/", () => "Hello World!");

app.Run();
