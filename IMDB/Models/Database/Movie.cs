using System.Collections.Generic;

namespace IMDB.Models.Database
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public int ProducerId { get; set; }
        public string CoverImage { get; set; } // Type as string - URL of the cover image stored in Firebase
    }
}
