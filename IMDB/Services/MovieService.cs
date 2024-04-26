using AutoMapper;
using IMDB.CustomExceptions;
using IMDB.Models.Database;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Repositories;
using IMDB.Repositories.Interfaces;
using IMDB.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace IMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorService _actorService;
        private readonly IGenreService _genreService;
        private readonly IProducerService _producerService;
        private readonly IMapper _mapper;
        public MovieService(IMovieRepository movieRepository, IActorService actorService, IGenreService genreService, IProducerService producerService, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _actorService = actorService;
            _genreService = genreService;
            _producerService = producerService;
            _mapper = mapper;
        }

        public void ValidateMovieObject(MovieRequest movie)
        {
            if (string.IsNullOrWhiteSpace(movie.Name))
                throw new InvalidRequestObjectException("Movie Name is required");
            else if (movie.YearOfRelease < 1888)
                throw new InvalidRequestObjectException("First movie of world was released in 1888 :). So, Fill correct year of release");
            else if (movie.Plot.Length > 1000)
                throw new InvalidRequestObjectException("Plot length should be less than 1000");

            _producerService.ValidateProducerIdExistence(movie.ProducerId);
            
            movie.Genres.ForEach(genId => _genreService.ValidateGenreIdExistence(genId));
            
            movie.Actors.ForEach(actId => _actorService.ValidateActorIdExistence(actId));
        }

        public void ValidateMovieIdExistence(int id)
        {
            if (_movieRepository.GetAsync(id) == null)
                throw new IdNotExistException($"The movie with id : {id} don't exist");
        }

        public async Task<int> CreateAsync(MovieRequest movie)
        { 
            ValidateMovieObject(movie);
            int id = 0;
            try
            {
                id = await _movieRepository.CreateAsync(_mapper.Map<Movie>(movie), movie.Actors, movie.Genres);
            }
            catch(Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (id != 0)
            {
                return id;
            }
            else
                throw new ErrorInRepositoryException("Got some error due to Repository.. try again");
        }

        public async Task<List<MovieResponse>> GetAsync()
        {
            List<MovieResponse> result = new List<MovieResponse>();
            try
            {
                List<Movie> movies = await _movieRepository.GetAsync();
                
                foreach (var movie in movies)
                {
                    MovieResponse movieUpdated = await GetAsync(movie.Id);
                    result.Add(movieUpdated);
                }

            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            if (result != null && result.Count > 0)
                return result;
            else
                throw new NoItemFoundException("No movies found");
        }

        public async Task<List<MovieResponse>> GetByYearAsync(int year)
        {
            List<MovieResponse> result = new List<MovieResponse>();
            try
            {
                List<Movie> movies = await _movieRepository.GetAsync();
                movies = movies.Where(m => m.YearOfRelease == year).ToList();
                foreach (var movie in movies)
                {
                    MovieResponse movieUpdated = await GetAsync(movie.Id);
                    result.Add(movieUpdated);
                }

            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            if (result != null && result.Count > 0)
                return result;
            else
                throw new NoItemFoundException("No movies found");
        }

        public async Task<MovieResponse> GetAsync(int id)
        {
            MovieResponse result = null;
            try
            {
                Movie movie = await _movieRepository.GetAsync(id);
                List<int> actorsIds = await _movieRepository.GetActorsAsync(id);
                List<int> genresIds = await _movieRepository.GetGenresAsync(id);

                result = _mapper.Map<MovieResponse>(movie);

                foreach (var actid in actorsIds)
                {
                    ActorResponse actor = await _actorService.GetAsync(actid);
                    result.Actors.Add(actor);
                }

                foreach (var genid in genresIds)
                {
                    GenreResponse genre = await _genreService.GetAsync(genid);
                    result.Genres.Add(genre);
                }
                result.Producer = await _producerService.GetAsync(movie.ProducerId);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (result != null)
                return result;
            else
                throw new NoItemFoundException("No movie found");
        }

        public async Task<int> UpdateAsync(MovieRequest movie)
        {
            ValidateMovieIdExistence(movie.Id);
            ValidateMovieObject(movie);
            try
            {
                return await _movieRepository.UpdateAsync(_mapper.Map<Movie>(movie), movie.Actors, movie.Genres);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
                
        }
        public async Task<int> DeleteAsync(int id)
        {
            ValidateMovieIdExistence(id);
            try
            {
                return await _movieRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            
        }


    }
}
