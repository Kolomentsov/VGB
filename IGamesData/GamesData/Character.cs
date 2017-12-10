using System.Collections.Generic;
using IGamesData.GamesData;

namespace IGamesData
{
    public class Character
    {
        public List<Repository.Game> Games { get; set; }

        public string Descriprtion { get; set; }
        public string Name { get; set; }

        public string Poster { get; set; }
    }
}
