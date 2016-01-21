using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FileSystem)]
    public class Margin : Robot
    {
        [Parameter("Timer seconds", DefaultValue = 5)]
        public int m { get; set; }

        [Parameter("Path", DefaultValue = "c:\folder\file.txt")]
        public string path { get; set; }

        protected override void OnStart()
        {
            Timer.Start(m);
        }

        protected override void OnTimer()
        {
            double margin = Math.Round(Account.FreeMargin, 2);

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(margin);
            file.Close();
            Print("Free margin : " + margin + " " + path);
        }

        protected override void OnTick()
        {

        }
    }
}
