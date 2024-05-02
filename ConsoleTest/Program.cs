using GameLibrary.Utility;

DelayQueue<int> queue = new();

Task t1 = Task.Run(async () =>
{
    while (true)
    {
        var res = await queue.DequeueAsync();
        Log(res.ToString());
    }
});

Task t2 = Task.Run(async () =>
{
    Log("start");
    queue.Enqueue(1, TimeSpan.FromSeconds(5));
    await Task.Delay(3000);
    queue.Enqueue(2 , TimeSpan.FromSeconds(1));
    await Task.Delay(1000);
    queue.Enqueue(3, TimeSpan.FromSeconds(2));
    queue.Enqueue(4, TimeSpan.FromSeconds(2));
    await Task.Delay(5000);
    Log("end");
});

await Task.WhenAny(t1, t2);

static void Log(string message)
{
    Console.WriteLine(DateTime.Now + " " + message);
}