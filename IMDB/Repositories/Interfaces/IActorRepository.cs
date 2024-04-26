using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Database;
namespace IMDB.Repositories.Interfaces
{
    public interface IActorRepository
    {
        Task<List<Actor>> GetAsync();
        Task<Actor> GetAsync(int id);
        Task<int> CreateAsync(Actor actor);
        Task<int> UpdateAsync(Actor actor);
        Task<int> DeleteAsync(int id);

    }
}
