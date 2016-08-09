using System;

namespace DiscordArchiver.data {
    public class DMessage {
        public DateTime timestamp;
        public ulong id;
        public DAuthor author;
        public string content;
        public ulong channel_id;
        public DAuthor[] mentions;
    }
}
