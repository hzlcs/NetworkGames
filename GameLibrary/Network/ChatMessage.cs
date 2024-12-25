using System;

namespace GameLibrary.Network;

public record ChatMessage(string Username, string Message, DateTime Timestamp);