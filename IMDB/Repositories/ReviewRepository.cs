using IMDB.Models.Database;
using IMDB.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly List<Review> _reviews = new();
        private int _maxId = 1;
        public async Task<int> CreateAsync(Review review)
        {
            review.Id = _maxId++;
            _reviews.Add(review);
            return await Task.FromResult(review.Id);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Task.FromResult(_reviews.RemoveAll(review => review.Id == id));
        }
        public async Task<int> DeleteReviewOfMovieAsync(int id)
        {
            return await Task.FromResult(_reviews.RemoveAll(review => review.MovieId == id));
        }

        public async Task<List<Review>> GetAsync(int movieId)
        {
            return await Task.FromResult(_reviews.Where(r => r.MovieId == movieId).ToList());
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await Task.FromResult(_reviews.Where(review => review.Id == id).FirstOrDefault());
        }

        public async Task<int> UpdateAsync(Review review)
        {
            int index = _reviews.FindIndex(item => item.Id == review.Id);

            if (index != -1)
            {
                _reviews[index] = review;
                return await Task.FromResult(1);
            }
            else
            {
                return await Task.FromResult(0);
            }
        }
    }
}
