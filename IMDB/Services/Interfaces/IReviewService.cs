using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Models.Request;
using IMDB.Models.Response;
namespace IMDB.Services.Interfaces
{
    public interface IReviewService
    {
        void ValidateReviewObject(ReviewRequest review);
        void ValidateReviewIdExistence(int id);
        Task<List<ReviewResponse>> GetAsync(int movieId);
        Task<ReviewResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(ReviewRequest review);
        Task<int> UpdateAsync(ReviewRequest review);
        Task<int> DeleteAsync(int id);
    }
}
