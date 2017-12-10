using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGamesData.GamesData
{
    public class Repository
    {
        public class TimeToBeat
        {
            public int hastly { get; set; }
            public int normally { get; set; }
            public int completely { get; set; }
        }

        public class ReleaseDate
        {
            public int category { get; set; }
            public int platform { get; set; }
            public object date { get; set; }
            public int region { get; set; }
            public string human { get; set; }
            public int y { get; set; }
            public int m { get; set; }
        }

        public class AlternativeName
        {
            public string name { get; set; }
            public string comment { get; set; }
        }

        public class Screenshot
        {
            public string url { get; set; }
            public string cloudinary_id { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Video
        {
            public string name { get; set; }
            public string video_id { get; set; }
        }

        public class Cover
        {
            public string url { get; set; }
            public string cloudinary_id { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Esrb
        {
            public int rating { get; set; }
            public string synopsis { get; set; }
        }

        public class Pegi
        {
            public int rating { get; set; }
            public string synopsis { get; set; }
        }

        public class Website
        {
            public int category { get; set; }
            public string url { get; set; }
        }

        public class MultiplayerMode
        {
            public int platform { get; set; }
            public bool offlinecoop { get; set; }
            public bool onlinecoop { get; set; }
            public bool lancoop { get; set; }
            public bool campaigncoop { get; set; }
            public bool splitscreenonline { get; set; }
            public bool splitscreen { get; set; }
            public bool dropin { get; set; }
            public int offlinecoopmax { get; set; }
            public int onlinecoopmax { get; set; }
            public int onlinemax { get; set; }
            public int offlinemax { get; set; }
        }

        public class Game
        {
            public int id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public string url { get; set; }
            public object created_at { get; set; }
            public object updated_at { get; set; }
            public string summary { get; set; }
            public int collection { get; set; }
            public int hypes { get; set; }
            public double rating { get; set; }
            public double popularity { get; set; }
            public double aggregated_rating { get; set; }
            public int aggregated_rating_count { get; set; }
            public double total_rating { get; set; }
            public int total_rating_count { get; set; }
            public int rating_count { get; set; }
            public List<int> games { get; set; }
            public List<int> tags { get; set; }
            public List<int> developers { get; set; }
            public List<int> publishers { get; set; }
            public List<int> game_engines { get; set; }
            public int category { get; set; }
            public TimeToBeat time_to_beat { get; set; }
            public List<int> player_perspectives { get; set; }
            public List<int> game_modes { get; set; }
            public List<int> keywords { get; set; }
            public List<int> themes { get; set; }
            public List<int> genres { get; set; }
            public List<int> expansions { get; set; }
            public List<int> dlcs { get; set; }
            public object first_release_date { get; set; }
            public int pulse_count { get; set; }
            public List<int> platforms { get; set; }
            public List<ReleaseDate> release_dates { get; set; }
            public List<AlternativeName> alternative_names { get; set; }
            public List<Screenshot> screenshots { get; set; }
            public List<Video> videos { get; set; }
            public Cover cover { get; set; }
            public Esrb esrb { get; set; }
            public Pegi pegi { get; set; }
            public List<Website> websites { get; set; }
            public List<MultiplayerMode> multiplayer_modes { get; set; }
            public string storyline { get; set; }
            public int? game { get; set; }
            public int? franchise { get; set; }
            public List<int?> franchises { get; set; }
        }
    }
}
        
