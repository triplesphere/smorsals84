using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using DiscordArchiver.data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordArchiver {

    class Program {

        public static string BaseUrl = "https://discordapp.com/api/channels/{0}/messages?token={1}&before={2}&limit={3}";

        public static string Channel, Token, Out = "out.json", Before = "", After = "";
        public static int Limit = 100;

        static void Main(string[] args) {

            Channel = args[0];
            Token = args[1];

            if (args.Length >= 3) {
                Out = args[2];
            }
            if (args.Length >= 4) {
                Before = args[3];
            }
            if (args.Length >= 5) {
                After = args[4];
            }
            if (args.Length >= 6) {
                int.TryParse(args[5], out Limit);
            }

            var fullLogs = new List<DMessage>();

            var counter = 0;
            var exit = false;
            var bw = new BackgroundWorker();

            bw.DoWork += (sender, eventArgs) => {
                while (true) {
                    counter++;
                    Console.WriteLine($"Downloading Logs Part {counter}");

                    var currentLog = "";
                    using (var wc = new WebClient()) {
                        currentLog = wc.DownloadString(string.Format(BaseUrl, Channel, Token, Before, Limit));
                    }

                    Console.WriteLine($"Downloaded Log Part {counter}, Parsing");

                    var jar = JArray.Parse(currentLog);

                    Before = jar[jar.Count - 1]["id"].ToString();
                    Console.WriteLine($"Before: {Before}");
                    var g = -1;
                    var mg = -1;
                    foreach (var jToken in jar) {
                        g++;
                        if ((string)jToken["id"] != After) continue;
                        mg = g;
                        break;
                    }

                    if (mg > -1) {
                        exit = true;
                        for (var i = mg; i < jar.Count - 1; i++) {
                            jar.RemoveAt(i);
                        }
                    }

                    fullLogs.InsertRange(0, JArray.Parse(currentLog).Select(jToken => jToken.ToObject<DMessage>()).Reverse().ToList());

                    if (counter % 50 == 0) {
                        Console.WriteLine($"Writing partial logs to file {Out}");
                        File.WriteAllText(Out, JsonConvert.SerializeObject(fullLogs, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
                    }

                    Thread.Sleep(500);

                    if (jar.Count < Limit || exit) break;
                }
            };

            bw.RunWorkerAsync();

            bw.RunWorkerCompleted += (sender, eventArgs) => {
                Console.WriteLine($"Writing logs to file {Out}");
                File.WriteAllText(Out, JsonConvert.SerializeObject(fullLogs, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
                Console.WriteLine("All done, press any key to exit...");
            };

            Console.ReadKey();
        }
    }
}