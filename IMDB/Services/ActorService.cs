using AutoMapper;
using IMDB.Models.Database;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Repositories.Interfaces;
using IMDB.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDB.CustomExceptions;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Numerics;
namespace IMDB.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }
        public void ValidateActorObject(ActorRequest actor)
        {
            if (string.IsNullOrWhiteSpace(actor.Name))
                throw new InvalidRequestObjectException("Actor name is required");
            else if (string.IsNullOrWhiteSpace(actor.Gender))
                throw new InvalidRequestObjectException("Actor gender is required");
            else if ((actor.Gender != "female" && actor.Gender != "male" && actor.Gender != "non-binary"))
                throw new InvalidRequestObjectException("Gender can only be - male , female , non-binary");
            else if(actor.Bio.Length > 500)
                throw new InvalidRequestObjectException("Actor Bio should be less than 500 characters");
        }
        public void ValidateActorIdExistence(int id)
        {
            if (_actorRepository.GetAsync(id) == null)
                throw new IdNotExistException($"The actor with id : {id} don't exist");
        }

        public async Task<int> CreateAsync(ActorRequest actor)
        {
            ValidateActorObject(actor);
            int id = 0;
            try
            {
                id = await _actorRepository.CreateAsync(_mapper.Map<Actor>(actor));
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            
            if (id != 0)
            {
                return id;
            }
            else
                throw new ErrorInRepositoryException("Got some error due to Repository.. try again");
        }

        public async Task<int> DeleteAsync(int id)
        {
            ValidateActorIdExistence(id);
            try
            {
                return await _actorRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            
        }

        public async Task<List<ActorResponse>> GetAsync()
        {
            List<Actor> actors = null;
            try
            {
                actors = await _actorRepository.GetAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            
            if (actors == null || actors.Count == 0)
                throw new NoItemFoundException("No actors exist");

            return actors.Select(actor => _mapper.Map<ActorResponse>(actor)).ToList();
        }

        public async Task<ActorResponse> GetAsync(int id)
        {
            Actor actor = null;
            try
            {
                actor = await _actorRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
            
            if (actor == null)
                throw new NoItemFoundException($"No actor exist with id : {id}");
            return _mapper.Map<ActorResponse>(actor);
        }

        public async Task<int> UpdateAsync(ActorRequest actor)
        {
            ValidateActorIdExistence(actor.Id);
            ValidateActorObject(actor);
            try
            {
                return await _actorRepository.UpdateAsync(_mapper.Map<Actor>(actor));
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }
        }
    }
}
