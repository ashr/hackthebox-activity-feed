using System;
using System.Net;
using System.Collections.Generic;

using HtmlAgilityPack;

namespace hackthebox_activity_feed
{
    public class HackTheBoxActivityParser{
        public static List<ActivityModel> LoadActivities(){
            Console.Write("LoadActivities:");
            List<ActivityModel> activities = new List<ActivityModel>();

            string EOF = "Not much happening here, yet.";
            bool EOFFlag = false;
            int pageCounter = 1;            
            WebClient wc = new WebClient();
            WebProxy wp = new WebProxy("http://localhost:3128");
            wc.Proxy = wp;
            
            while(!EOFFlag){
                string pageData = wc.DownloadString("https://forum.hackthebox.eu/activity?Page=p" + pageCounter.ToString());
                if (pageData.Contains(EOF)){
                    EOFFlag = true;
                    continue;
                }

                List<ActivityModel> activitiesForPage = ParseActivities(pageData);
                
                if (activitiesForPage != null && activitiesForPage.Count > 0)
                    activities.AddRange(activitiesForPage);

                pageCounter++;
            }

            Console.WriteLine(activities.Count);
            return activities;
        }

        public static List<ActivityModel> ParseActivities(string pageData){
            List<ActivityModel> activities = new List<ActivityModel>();

            HtmlDocument htmlData = new HtmlDocument ();
            htmlData.LoadHtml (pageData);

            HtmlNodeCollection nodes = htmlData.DocumentNode.SelectNodes ("//ul [@class='DataList Activities']");

            if (nodes != null && nodes.Count > 0) {
                HtmlNode pageNode = nodes [0];

                if (pageNode.ChildNodes.Count > 0) {
                    foreach(HtmlNode node in pageNode.ChildNodes){
                        if (node.Name == "li"){
                            Console.WriteLine(node.ChildNodes.Count);
                            ActivityModel am = new ActivityModel();
                            am.activityId = node.Id;
                            am.content = node.SelectNodes("div")[1].SelectNodes("div[@class='Title']")[0].InnerText;
                            if (node.SelectNodes("div")[1].SelectNodes("div[@class='Excerpt userContent']") != null)
                                am.content += " " + node.SelectNodes("div")[1].SelectNodes("div[@class='Excerpt userContent']")[0].InnerText;

                            am.authorName = node.SelectNodes("div")[0].SelectNodes("a")[0].Attributes["href"].Value;
                            am.authorPicUrl = node.SelectNodes("div")[0].SelectSingleNode("a").InnerHtml;//node.SelectNodes("div")[0].SelectSingleNode("a").SelectSingleNode("img").Attributes["data-cfsrc"].Value;
                            am.postDate = node.SelectNodes("div")[1].SelectNodes("div[@class='Meta']")[0].SelectNodes("span")[0].InnerText;

                            /*
                            Console.WriteLine("Created:");
                            Console.WriteLine("\tID:" + am.activityId);
                            Console.WriteLine("\tHASH:" + am.activityHash);
                            Console.WriteLine("\tAuthor:" + am.authorName);
                            Console.WriteLine("\tAuthorPic:" + am.authorPicUrl);
                            Console.WriteLine("\tContent:" + am.content);
                            Console.WriteLine("\tPost Date:" + am.postDate);
                            */
                            
                            activities.Add(am);
                        }
                    }
                }
            }


            return activities;
        }
    }
}