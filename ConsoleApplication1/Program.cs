//Author : Shank
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Yaml;
using System.Yaml.Serialization;
using YamlDotNet.Serialization;
using YaTools.Yaml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            string headers = "City,Date,MatchType,WinByType,WinByValue,Winner,Overs,PlayerOfTheMatch,Team1,Team2,TossDecision,TossWinner,Umpire1,Umpire2,Venue,Competition,ReferenceFileName";

            var csv = new StringBuilder();
            csv.AppendLine(headers);


             string[] files = Directory.GetFiles(@"E:\OneDriveFolder\Documents\NIU\Spring2015\DataViz\all");
            //string filename = "211028.yaml";
           // string[] files = { @"E:\OneDriveFolder\Documents\NIU\Spring2015\DataViz\all\335983.yaml" };
            int count = 1;
            List<string> allnames = new List<string>();
            foreach (string filepath in files)
            {
                if (filepath.Contains(".yaml"))
                {
                    string newLine = PopulateData(filepath,allnames);
                    csv.AppendLine(newLine);
                    Console.WriteLine(count++);
                }
            }
            //after your loop
            File.WriteAllText("data.csv", csv.ToString());
            Console.WriteLine("Finished");

            Console.ReadLine();

            
        }

        private static string PopulateData(string filename,List<string> allNames)
        {
            YamlSerializer ys = new YamlSerializer();
            Dictionary<object, object> root = (Dictionary<object, object>)ys.DeserializeFromFile(filename)[0];

            //Info
            Dictionary<object, object> info = (Dictionary<object, object>)root["info"];

            foreach (var key in info.Keys)
            {
                if (!allNames.Contains(key.ToString()))
                    allNames.Add(key.ToString());   
            }

            //city
            string city = "";
            if (info.Keys.Contains("city"))
                city = info["city"].ToString();

            //Date
            var gameDates = info["dates"];
            string gameDate = Convert.ToString(((Object[])gameDates)[0]);


            //match_type
            string type = info["match_type"].ToString();

            //out
            Dictionary<object, object> outcome = (Dictionary<object, object>)info["outcome"];

            string winbytype = "tie";
            string winbyvalue = "";
            string winner = "tie";
            if (outcome.Keys.Contains("by"))
            {
                Dictionary<object, object> winby = (Dictionary<object, object>)outcome["by"];
                winbytype = winby.Keys.Contains("runs") ? "runs" : "wickets";
                winbyvalue = winby[winbytype].ToString();
                winner = (outcome)["winner"].ToString();
            }
            else if (outcome.Keys.Contains("result"))
            {
                winbytype = outcome["result"].ToString();

                winner = outcome["result"].ToString(); ;
            }
            //overs
            string overs = "";
            if (info.Keys.Contains("overs"))
                overs = info["overs"].ToString();

            //player of the match
            string playerofthematch = "";
            if (info.Keys.Contains("player_of_match"))
                playerofthematch = ((object[])info["player_of_match"])[0].ToString();

            //teams
            string team1 = ((object[])info["teams"])[0].ToString();
            string team2 = ((object[])info["teams"])[1].ToString();

            //toss
            Dictionary<object, object> toss = (Dictionary<object, object>)info["toss"];
            string toss_decision = toss["decision"].ToString();
            string toss_winner = toss["winner"].ToString();

            //umpires
            string umpire1 = ((object[])info["umpires"])[0].ToString();
            string umpire2 = ((object[])info["umpires"])[1].ToString();

            //venue
            string venue = info["venue"].ToString();
            venue = "\"" + venue + "\"";
            //competition
            string competition = "";
            if (info.Keys.Contains("competition"))
                competition = info["competition"].ToString();

            //"City,Date,MatchType,WinByType,WinByValue,Winner,Overs,PlayerOfTheMatch,
            //Team1,Team2,TossDecision,TossWinner,Umpire1,Umpire2,Venue,Competition,ReferenceFileName";
            string newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                city, gameDate, type, winbytype, winbyvalue, winner, overs, playerofthematch,
                team1, team2, toss_decision, toss_winner, umpire1, umpire2, venue,competition,filename);
            return newLine;
        }
    }
}
