using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace YoutubeSubscriberManager.Subscriber
{
    public class Subscriber
    {
        public Subscriber()
        {
            ViewCounts = new List<double>();
        }
        public string Name { get; set; }
        public int Watches { get; set; }
        public bool CommentedLately { get; set; }

        public bool Watched => Watches > 0;

        public int Videos { get; set; }
        public double AverageViewCount { get; set; }
        public List<double> ViewCounts { get; set; }
    }
}
