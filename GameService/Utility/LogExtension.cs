namespace GameService
{
    public static class LogExtension
    {
        public static void Info<T>(this ILogger<T> logger, string message)
        {
            logger.LogInformation("{Time:yyyy-MM-dd HH:mm:ss.fff}: {Message}", DateTime.Now, message);
        }

        public static void Debug<T>(this ILogger<T> logger, string message, params object?[] args)
        {
            logger.LogDebug("{Time:yyyy-MM-dd HH:mm:ss.fff}: {Message}", DateTime.Now, message);
        }
    }
}