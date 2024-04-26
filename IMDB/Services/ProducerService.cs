using AutoMapper;
using IMDB.CustomExceptions;
using IMDB.Models.Database;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Repositories;
using IMDB.Repositories.Interfaces;
using IMDB.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;

        public ProducerService(IProducerRepository producerRepository, IMapper mapper)
        {
            _producerRepository = producerRepository;
            _mapper = mapper;
        }

        public void ValidateProducerObject(ProducerRequest producer)
        {
            if (string.IsNullOrWhiteSpace(producer.Name))
                throw new InvalidRequestObjectException("Producer name is required");
            else if (string.IsNullOrWhiteSpace(producer.Gender))
                throw new InvalidRequestObjectException("Producer gender is required");
            else if ((producer.Gender != "female" && producer.Gender != "male" && producer.Gender != "non-binary"))
                throw new InvalidRequestObjectException("Gender can only be - male , female , non-binary");
            else if (producer.Bio.Length > 500)
                throw new InvalidRequestObjectException("producer Bio should be less than 500 characters");
        }

        public void ValidateProducerIdExistence(int id)
        {
            if (_producerRepository.GetAsync(id) == null)
                throw new IdNotExistException($"The producer with id : {id} don't exist");
        }

        public async Task<int> CreateAsync(ProducerRequest producer)
        {
            ValidateProducerObject(producer);
            int id = 0;
            try
            {
                id = await _producerRepository.CreateAsync(_mapper.Map<Producer>(producer));
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
            ValidateProducerIdExistence(id);
            try
            {
                return await _producerRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

        }

        public async Task<List<ProducerResponse>> GetAsync()
        {
            List<Producer> producers = null;
            try
            {
                producers = await _producerRepository.GetAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (producers == null || producers.Count == 0)
                throw new NoItemFoundException("No producers exist");

            return producers.Select(producer => _mapper.Map<ProducerResponse>(producer)).ToList();
        }

        public async Task<ProducerResponse> GetAsync(int id)
        {
            Producer producer ;
            try
            {
                producer = await _producerRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (producer == null)
                throw new NoItemFoundException($"No producer exist with id : {id}");
            return _mapper.Map<ProducerResponse>(producer);
        }

        public async Task<int> UpdateAsync(ProducerRequest producer)
        {
            ValidateProducerIdExistence(producer.Id);
            ValidateProducerObject(producer);
            try
            {
                return await _producerRepository.UpdateAsync(_mapper.Map<Producer>(producer));
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }


        }
    }
}
