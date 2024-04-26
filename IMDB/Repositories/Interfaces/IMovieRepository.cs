using IMDB.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace IMDB.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAsync();
        Task<Movie> GetAsync(int id);
        Task<int> CreateAsync(Movie movie, List<int> actorsIds, List<int> genresIds);
        Task<int> UpdateAsync(Movie movie, List<int> actors, List<int> genres);
        Task<int> DeleteAsync(int id);


        Task AddActorsIdsAsync(int movieId, List<int> actorsIds);
        Task<List<int>> GetActorsAsync(int movieId);
        Task RemoveActorsAsync(int movieId, List<int> actorsToBeRemoved);
        Task AddGenresIdsAsync(int movieId, List<int> genresIds);
        Task<List<int>> GetGenresAsync(int movieId);
        Task RemoveGenresAsync(int movieId, List<int> genresToBeRemoved);
    }
}
