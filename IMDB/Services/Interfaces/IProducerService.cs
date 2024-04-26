using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Request;
using IMDB.Models.Response;
namespace IMDB.Services.Interfaces
{
    public interface IProducerService
    {
        void ValidateProducerObject(ProducerRequest producer);
        void ValidateProducerIdExistence(int id);
        Task<List<ProducerResponse>> GetAsync();
        Task<ProducerResponse> GetAsync(int id);
        Task<int> CreateAsync(ProducerRequest producer);
        Task<int> UpdateAsync(ProducerRequest producer);
        Task<int> DeleteAsync(int id);
    }
}
