using IMDB.Models.Database;
using IMDB.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Dictionary<int, List<int>> actor_movies = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> genre_movies = new Dictionary<int, List<int>>();
        private readonly List<Movie> _movies = new List<Movie>();
        private readonly IReviewRepository _reviewRepository;
        private int _maxId = 1;

        public MovieRepository(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<List<Movie>> GetAsync()
        {
                return await Task.FromResult(_movies);
        }

        public async Task<Movie> GetAsync(int id)
        {
            return await Task.FromResult(_movies.Where(movie => movie.Id == id).FirstOrDefault());
        }

        public async Task<int> CreateAsync(Movie movie, List<int> actorsIds, List<int> genresIds)
        {
            movie.Id = _maxId++;
            _movies.Add(movie);

            actor_movies[movie.Id] = actorsIds;
            genre_movies[movie.Id] = genresIds;
            return await Task.FromResult(movie.Id);
        }

        public async Task<int> UpdateAsync(Movie movie, List<int> actors, List<int> genres)
        {
            int index = _movies.FindIndex(item => item.Id == movie.Id);

            if (index != -1)
            {
                _movies[index] = movie;
                if (actors.Count > 0)
                {
                    actor_movies[index] = actors;
                }

                if (genres.Count > 0)
                {
                    genre_movies[index] = genres;
                }
                return await Task.FromResult(1);
            }
            else
                return await Task.FromResult(0);
        }

        public async Task<int> DeleteAsync(int id)
        {
            actor_movies.Remove(id);
            genre_movies.Remove(id);
            int _x = await _reviewRepository.DeleteReviewOfMovieAsync(id);
            return await Task.FromResult(_movies.RemoveAll(movie => movie.Id == id));
        }

        public async Task AddActorsIdsAsync(int movieId, List<int> actorsIds)
        {
            if (actor_movies.ContainsKey(movieId))
            {
                var actors = actor_movies[movieId];
                actors.AddRange(actorsIds);
                actor_movies[movieId] = actors.Distinct().ToList();
            }
            else
            {
                actor_movies.Add(movieId, actorsIds.Distinct().ToList());
            }
            await Task.CompletedTask;
        }


        public async Task<List<int>> GetActorsAsync(int movieId)
        {
            actor_movies.TryGetValue(movieId, out var actors);
            return await Task.FromResult(actors);
        }

        public async Task RemoveActorsAsync(int movieId, List<int> actorsToBeRemoved)
        {
            if (actor_movies.TryGetValue(movieId, out var actors) && actorsToBeRemoved != null)
            {
                foreach (var actor in actorsToBeRemoved)
                {
                    actors.Remove(actor);
                }
            }

            await Task.CompletedTask;
        }


        public async Task AddGenresIdsAsync(int movieId, List<int> genresIds)
        {
            if (genre_movies.ContainsKey(movieId))
            {
                var actors = actor_movies[movieId];
                actors.AddRange(genresIds);
                actor_movies[movieId] = actors.Distinct().ToList();
            }
            else
            {
                actor_movies.Add(movieId, genresIds.Distinct().ToList());
            }

            await Task.CompletedTask;
        }

        public async Task<List<int>> GetGenresAsync(int movieId)
        {
            genre_movies.TryGetValue(movieId, out var genres);
            return await Task.FromResult(genres);
        }

        public async Task RemoveGenresAsync(int movieId, List<int> genresToBeRemoved)
        {
            genre_movies.TryGetValue(movieId, out var genres);
            if (genresToBeRemoved != null)
            {
                foreach (var genre in genresToBeRemoved)
                {
                    genres.Remove(genre);
                }
            }
            await Task.CompletedTask;
        }
    }
}
