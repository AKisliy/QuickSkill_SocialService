using System.Runtime.CompilerServices;
using AutoMapper;
using MassTransit;
using RabbitMQ.Client;
using Shared;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Models;

namespace SocialService.Infrastructure.Consumers
{
    public class UserChangedConsumer : IConsumer<UserChangedEvent>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserChangedConsumer(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UserChangedEvent> context)
        {
            var updatedUser = context.Message;
            System.Console.WriteLine(updatedUser.Photo);
            var user = _mapper.Map<User>(updatedUser);
            await _repository.Update(user);
            await Task.CompletedTask;
        }
    }
}