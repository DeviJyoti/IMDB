using IMDB.Models.Database;
using IMDB.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly List<Actor> _actors = new();
        private int _maxId = 1;
        public async Task<int> CreateAsync(Actor actor)
        {
            actor.Id = _maxId++;
            _actors.Add(actor);
            return await Task.FromResult(actor.Id);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Task.FromResult(_actors.RemoveAll(actor => actor.Id == id));
        }

        public async Task<List<Actor>> GetAsync()
        {
            return await Task.FromResult(_actors);
        }

        public async Task<Actor> GetAsync(int id)
        {
            return await Task.FromResult(_actors.Where(actor => actor.Id == id).FirstOrDefault());
        }

        public async Task<int> UpdateAsync(Actor actor)
        {
            int index = _actors.FindIndex(item => item.Id == actor.Id);

            if(index != -1)
            {
                _actors[index]= actor;
                return await Task.FromResult(1);
            }
            else
            {
                return await Task.FromResult(0);
            }
        }
    }
}
