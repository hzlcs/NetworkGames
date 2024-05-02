using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Network
{
    public class Message
    {
        public string Id { get; set; } = null!;
        public MessageType Type { get; set; }
        public string? Msg { get; set; }
    }

    public enum MessageType
    {
        None,
        CommonMessage
    }

}
