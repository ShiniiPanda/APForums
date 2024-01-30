using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data
{
    public class ServicesApiRoutes
    {
        public static readonly string API_BASE = MauiProgram.ServerAddress + "api/";
        public static readonly string API_USERS = API_BASE + "Users";
        public static readonly string API_PAGES = API_BASE + "Pages";
        public static readonly string API_CLUBS = API_BASE + "Clubs";
        public static readonly string API_SOCIALS = API_BASE + "Socials";
        public static readonly string API_TAGS = API_BASE + "Tags";
        public static readonly string API_POSTS = API_BASE + "Posts";
        public static readonly string API_FORUMS = API_BASE+ "Forums";
        public static readonly string API_EVENTS = API_BASE + "Events";
        public static readonly string API_ACTIVITIES = API_BASE + "Activities";
    }
}
