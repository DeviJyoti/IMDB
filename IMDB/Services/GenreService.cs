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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        public void ValidateGenreObject(GenreRequest genre)
        {
            if (string.IsNullOrWhiteSpace(genre.Name))
                throw new InvalidRequestObjectException("Genre name is required");
        }
        public void ValidateGenreIdExistence(int id)
        {
            if (_genreRepository.GetAsync(id) == null)
                throw new IdNotExistException($"The genre with id : {id} don't exist");
        }

        public async Task<int> CreateAsync(GenreRequest genre)
        {
            ValidateGenreObject(genre);
            int id;
            try
            {
                id = await _genreRepository.CreateAsync(_mapper.Map<Genre>(genre));
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
            ValidateGenreIdExistence(id);
            try
            {
                return await _genreRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

        }

        public async Task<List<GenreResponse>> GetAsync()
        {
            List<Genre> genres = null;
            try
            {
                genres = await _genreRepository.GetAsync();
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (genres == null || genres.Count == 0)
                throw new NoItemFoundException("No genres exist");

            return genres.Select(genre => _mapper.Map<GenreResponse>(genre)).ToList();
        }

        public async Task<GenreResponse> GetAsync(int id)
        {
            Genre genre = null;
            try
            {
                genre = await _genreRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (genre == null)
                throw new NoItemFoundException($"No genre exist with id : {id}");
            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<int> UpdateAsync(GenreRequest genre)
        {
            ValidateGenreIdExistence(genre.Id);
            ValidateGenreObject(genre);
            try
            {
                return await _genreRepository.UpdateAsync(_mapper.Map<Genre>(genre));
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }


        }
    }
}
