namespace DiscordArchiver {

    public class DEmbed {
        public string url;
        public string type;
        public string description;
        public string title;
        public DThumbnail thumbnail;
    }

    public class DThumbnail {
        public string url;
        public int width;
        public int height;
        public string proxy_url;
    }
}