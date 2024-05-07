# QuickSkill_SocialService
Part of backend of QuickSkill. SocialService microservice.
## Description 👨‍💻
SocialService is a part of QuickSkill project. This microservice is responsible for:
1. Comments system
2. Likes system
3. Leaderboard logic
4. Community (Posts and answers) system
5. and some other stuff
## Stack 🛠️
- .NET
- Entity Framework
- Hangfire
- PostgreSQL
- Docker
- RabbitMQ
## Main features 💡
### Leaderboard system
The logic of weekly leaderboards contains:
1. Moving user to an appropriate league (lower or higher) according to their weekly XP
2. Generating new leaderboards every week

Every leaderboard should include 20 users. However, number of registered users can't be always divided by 20, so SocialService is also capable of:

3. Creating bots 🤖, who will act like users, to fill all leaderboards
### Community 👥
Community is a part of QuickSkill, which provides following functions:
1. Creating posts
2. Give answers to posts

SocialService processes all the necessary information about these things and handles creation, updating and deletion of posts and answers.

### Likes system❤️
> [!NOTE]
> This part is still in development

User can give like to other user's comment, post or answer. So, SocialService should provide fast and responsive like system. To create one, NoSQL database should be used, because relational DB isn't the best choice for this purpose.
### Other stuff
- Subscription logic
- Handling leagues

## Patterns
In this project, mainly two patterns are used:
1. Repository (for data access layer)
2. Mediator (send information about changed data to RabbitMQ)

