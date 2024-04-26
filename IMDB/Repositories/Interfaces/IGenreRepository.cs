using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Database;
namespace IMDB.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAsync();
        Task<Genre> GetAsync(int id);
        Task<int> CreateAsync(Genre genre);
        Task<int> UpdateAsync(Genre genre);
        Task<int> DeleteAsync(int id);
    }
}
