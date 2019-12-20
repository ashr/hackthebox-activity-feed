using System;
using System.Collections.Generic;

namespace hackthebox_activity_feed
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ActivityModel> activities = HackTheBoxActivityParser.LoadActivities();

            if (activities != null && activities.Count > 0){
                HackTheBoxDatabase.Update(activities);
            }
        }
    }
}
