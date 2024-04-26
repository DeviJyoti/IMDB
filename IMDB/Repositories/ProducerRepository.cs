using IMDB.Models.Database;
using IMDB.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly List<Producer> _producers = new();
        private int _maxId = 1;
        private readonly IMovieRepository _movieRepository;
        public ProducerRepository(IMovieRepository movieRepository) 
        {
            _movieRepository = movieRepository;
        }
        public async Task<int> CreateAsync(Producer producer)
        {
            producer.Id = _maxId++;
            _producers.Add(producer);
            return await Task.FromResult(producer.Id);
        }

        public async Task<int> DeleteAsync(int id)
        {
            List<Movie> movieList = await _movieRepository.GetAsync();
            List<int> movieIds = movieList.Where(x => x.ProducerId == id).Select(x => x.Id).ToList();
            foreach(var movieid in movieIds)
            {
                var res = await _movieRepository.DeleteAsync(movieid);
            }
            
            return await Task.FromResult(_producers.RemoveAll(producer => producer.Id == id));
        }

        public async Task<List<Producer>> GetAsync()
        {
            return await Task.FromResult(_producers);
        }

        public async Task<Producer> GetAsync(int id)
        {
            return await Task.FromResult(_producers.Where(producer => producer.Id == id).FirstOrDefault());
        }

        public async Task<int> UpdateAsync(Producer producer)
        {
            int index = _producers.FindIndex(item => item.Id == producer.Id);

            if (index != -1)
            {
                _producers[index] = producer;
                return await Task.FromResult(1);
            }
            else
            {
                return await Task.FromResult(0);
            }
        }
    }
}