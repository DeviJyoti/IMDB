using IMDB.Models.Database;
using IMDB.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly List<Genre> _genres = new();
        private int _maxId = 1;
        public async Task<int> CreateAsync(Genre genre)
        {
            genre.Id = _maxId++;
            _genres.Add(genre);
            return genre.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Task.FromResult(_genres.RemoveAll(genre => genre.Id == id));
        }

        public async Task<List<Genre>> GetAsync()
        {
            return await Task.FromResult(_genres);
        }

        public async Task<Genre> GetAsync(int id)
        {
            return await Task.FromResult(_genres.Where(genre => genre.Id == id).FirstOrDefault());
        }

        public async Task<int> UpdateAsync(Genre genre)
        {
            int index = _genres.FindIndex(item => item.Id == genre.Id);

            if (index != -1)
            {
                _genres[index] = genre;
                return await Task.FromResult(1);
            }
            else
            {
                return await Task.FromResult(0);
            }
        }
    }
}