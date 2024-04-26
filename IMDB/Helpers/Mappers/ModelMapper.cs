using AutoMapper;
using System.Collections.Generic;

namespace IMDB.Helpers.Mappers
{
    public class ModelMapper:Profile
    {
        public ModelMapper() 
        {
            CreateMap<Models.Request.ActorRequest, Models.Database.Actor>();
            CreateMap<Models.Database.Actor, Models.Response.ActorResponse>();

            CreateMap<Models.Request.GenreRequest, Models.Database.Genre>();
            CreateMap<Models.Database.Genre, Models.Response.GenreResponse>();

            CreateMap<Models.Request.ProducerRequest, Models.Database.Producer>();
            CreateMap<Models.Database.Producer, Models.Response.ProducerResponse>();

            CreateMap<Models.Request.ReviewRequest, Models.Database.Review>();
            CreateMap<Models.Database.Review, Models.Response.ReviewResponse>();

            CreateMap<Models.Request.MovieRequest, Models.Database.Movie>();
            CreateMap<Models.Database.Movie, Models.Response.MovieResponse>()
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => new List<Models.Response.ActorResponse>()))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => new List<Models.Response.GenreResponse>()));        }
    }
}
