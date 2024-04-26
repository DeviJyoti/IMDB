using IMDB.Models.Request;
using IMDB.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace IMDB.Services.Interfaces
{
    public interface IActorService
    {
        void ValidateActorObject(ActorRequest actor);
        void ValidateActorIdExistence(int id);
        Task<List<ActorResponse>> GetAsync();
        Task<ActorResponse> GetAsync(int id);
        Task<int> CreateAsync(ActorRequest actor);
        Task<int> UpdateAsync(ActorRequest actor);
        Task<int> DeleteAsync(int id);
    }
}
