using System;
using System.Collections.Generic;

namespace IMDB.Models.Response
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public List<ActorResponse> Actors { get; set; } // Collection of Actors (Not part of DB Model Class)
        public List<GenreResponse> Genres { get; set; } // Collection of Genre (Not part of DB Model Class)
        public ProducerResponse Producer { get; set; }
        public string CoverImage { get; set; } // Type as string - URL of the cover image stored in Firebase

        public static implicit operator MovieResponse(List<MovieResponse> v)
        {
            throw new NotImplementedException();
        }
    }
}
