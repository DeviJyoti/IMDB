using System.Collections.Generic;

namespace IMDB.Models.Request
{
    public class MovieRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public List<int> Actors { get; set; } // Collection of Actors (Not part of DB Model Class)
        public List<int> Genres { get; set; } // Collection of Genre (Not part of DB Model Class)
        public int ProducerId { get; set; }
        public string CoverImage { get; set; } // Type as string - URL of the cover image stored in Firebase
    }
}
