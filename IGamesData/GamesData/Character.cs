using System.Collections.Generic;
using IGamesData.GamesData;

namespace IGamesData
{
    public class Character
    {
        public List<int> Games { get; set; }

        public string Descriprtion { get; set; }

        public string Poster { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public object created_at { get; set; }
        public object updated_at { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public IList<int> people { get; set; }
        public IList<int> games { get; set; }
        public int? gender { get; set; }
        public int? species { get; set; }
    }
}
