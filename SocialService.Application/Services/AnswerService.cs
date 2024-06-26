using SocialService.Core;
using SocialService.Core.Enums;
using SocialService.Core.Exceptions;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Models;

namespace SocialService.Application.Services
{
    public class AnswerService: IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserRepository _userRepository;

        public AnswerService(IAnswerRepository answerRepository, IUserRepository userRepository)
        {
            _answerRepository = answerRepository;
            _userRepository = userRepository;
        }

        public async Task<Answer> GetAnswerById(int id)
        {
            return await _answerRepository.GetAnswerById(id);
        }

        public async Task<int> CreateAnswer(int userId, int discussionId, string body)
        {
            if(!await _userRepository.HasUserWithId(userId))
                throw new NotFoundException($"No user with id: {userId}");
            return await _answerRepository.CreateAnswer(userId, discussionId, body);
        }

        public async Task DeleteAnswer(int id)
        {
            await _answerRepository.DeleteAnswer(id);
        }

        public async Task EditAnswer(int id, string newBody)
        {
            var answer = await _answerRepository.GetAnswerById(id);
            if(answer.Body == newBody)
                return;
            await _answerRepository.EditAnswer(id, newBody);
        }

        public async Task<IEnumerable<Answer>> GetAnswersPage(int discussionId, int page, int pageSize, OrderByOptions orderBy)
        {
            if(page < 0 || pageSize <= 0)
                throw new BadRequestException("Page number should be >= 0/page size should be > 0");
            return await _answerRepository.GetAnswersPage(discussionId, page, pageSize, orderBy);
        }
    }
}