using System;

namespace DiscordArchiver.data {

    public class DMessage {
        public DAttachment[] attachments;
        public bool tts;
        public DEmbed[] embeds;
        public DateTime? timestamp;
        public bool mention_everyone;
        public string id;
        public DateTime? edited_timestamp;
        public DAuthor author;
        public string content;
        public string channel_id;
        public DAuthor[] mentions;
    }
}