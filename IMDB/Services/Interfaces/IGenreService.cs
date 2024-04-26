using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Request;
using IMDB.Models.Response;
namespace IMDB.Services.Interfaces
{
    public interface IGenreService
    {
        void ValidateGenreObject(GenreRequest genre);
        void ValidateGenreIdExistence(int id);
        Task<List<GenreResponse>> GetAsync();
        Task<GenreResponse> GetAsync(int id);
        Task<int> CreateAsync(GenreRequest genre);
        Task<int> UpdateAsync(GenreRequest genre);
        Task<int> DeleteAsync(int id);
    }
}
