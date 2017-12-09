using System.Collections.Generic;

namespace IGamesData
{
    public class Game
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public string Cover { get; set; }

        public uint IGdbID { get; set; }

        public int Category { get; set; }

        public List<int> GameMod { get; set; }

        public string Discription { get; set; }

        public double IGdbRating { get; set; }

        public List<int> Publisher { get; set; }

        public int Status { get; set; }

        private string _getigdbrating;

        public string _IGDBlink
        {
            get { return _getigdbrating; }
            set
            {
                _getigdbrating = value != null ? string.Concat("https://www.igdb.com/games_top100/", value) : value;
            }
        }
    }
}
