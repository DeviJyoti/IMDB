using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Request;
using IMDB.Models.Response;
namespace IMDB.Services.Interfaces
{
    public interface IMovieService
    {
        void ValidateMovieObject(MovieRequest movie);
        void ValidateMovieIdExistence(int id);
        Task<List<MovieResponse>> GetAsync();
        Task<List<MovieResponse>> GetByYearAsync(int year);
        Task<MovieResponse> GetAsync(int id);
        Task<int> CreateAsync(MovieRequest movie);
        Task<int> UpdateAsync(MovieRequest movie);
        Task<int> DeleteAsync(int id);
    }
}
