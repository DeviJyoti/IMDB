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
    public class ReviewService : IReviewService
    {
        private readonly IMovieService _movieService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewService(IReviewRepository reviewRepository, IMapper mapper,IMovieService movieService)
        {
            _movieService = movieService;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        public void ValidateReviewObject(ReviewRequest review)
        {
            if (string.IsNullOrWhiteSpace(review.Message))
                throw new InvalidRequestObjectException("Review message is required");
            else if (review.Message.Length<=1000)
                throw new InvalidRequestObjectException("Review message length should be than 1000 characters");
            
            _movieService.ValidateMovieIdExistence(review.MovieId);
        }
        public void ValidateReviewIdExistence(int id)
        {
            if (_reviewRepository.GetAsync(id) == null)
                throw new IdNotExistException($"The review with id : {id} don't exist");
        }


        public async Task<int> CreateAsync(ReviewRequest review)
        {
            ValidateReviewObject(review);
            int id = 0;
            try
            {
                id = await _reviewRepository.CreateAsync(_mapper.Map<Review>(review));
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
            ValidateReviewIdExistence(id);
            try
            {
                return await _reviewRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

        }

        public async Task<List<ReviewResponse>> GetAsync(int movieId)
        {
            List<Review> reviews = null;
            try
            {
                reviews = await _reviewRepository.GetAsync(movieId);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (reviews == null || reviews.Count == 0)
                throw new NoItemFoundException("No reviews exist");

            return reviews.Select(review => _mapper.Map<ReviewResponse>(review)).ToList();
        }

        public async Task<ReviewResponse> GetByIdAsync(int reviewId)
        {
            Review review;
            try
            {
                review = await _reviewRepository.GetByIdAsync(reviewId);
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }

            if (review == null)
                throw new NoItemFoundException($"No review exist with id : {reviewId}");
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<int> UpdateAsync(ReviewRequest review)
        {
            ValidateReviewIdExistence(review.Id);
            ValidateReviewObject(review);
            try
            {
                return await _reviewRepository.UpdateAsync(_mapper.Map<Review>(review));
            }
            catch (Exception ex)
            {
                throw new ErrorInRepositoryException(ex.Message);
            }


        }
    }
}
