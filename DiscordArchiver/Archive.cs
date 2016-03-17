using System.Collections.Generic;
using DiscordArchiver.data;

namespace DiscordArchiver {

    public class Archive {

        private static Archive _archvive;
        public static Archive GetInstance() {
            return _archvive ?? (_archvive = new Archive());
        }

        public List<DMessage> MessageArchive = new List<DMessage>(); 

    }
}