using SocialService.Core.Enums;

namespace SocialService.DataAccess.Extensions
{
    public static class IQueryableExtesion
    {
        public static IQueryable<AnswerEntity> OrderAnswersBy(this IQueryable<AnswerEntity> answers, OrderByOptions option)
        {
            return option switch
            {
                OrderByOptions.SimpleOrder => answers.OrderBy(a => a.Id),
                OrderByOptions.ByDate => answers.OrderBy(a => a.PublishedOn),
                OrderByOptions.ByLikes => answers.OrderByDescending(a => a.Likes),
                _ => throw new InvalidOperationException($"No such option {option} to order"),
            };
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            if (pageNumZeroStart != 0)
                query.Skip(pageNumZeroStart * pageSize);
            return query.Take(pageSize);
        }
    }
}