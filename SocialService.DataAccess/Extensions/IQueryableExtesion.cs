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
                _ => throw new InvalidOperationException($"No such option {option} to order")
            };
        }

        public static IQueryable<DiscussionEntity> OrderDiscussionsBy(this IQueryable<DiscussionEntity> discussions, OrderByOptions option)
        {
            return option switch
            {
                OrderByOptions.SimpleOrder => discussions.OrderBy(d => d.Id),
                OrderByOptions.ByAnswerAmount => discussions.OrderBy(d => d.Answers.Count),
                OrderByOptions.ByDate => discussions.OrderBy(d => d.PublishedOn),
                OrderByOptions.ByLikes => discussions.OrderBy(d => d.Likes),
                _ => throw new InvalidOperationException($"No such option {option} to order")
            };
        }

        public static IQueryable<CommentEntity> OrderCommentsBy(this IQueryable<CommentEntity> comments, OrderByOptions option)
        {
            return option switch
            {
                OrderByOptions.SimpleOrder => comments.OrderBy(c => c.Id),
                OrderByOptions.ByDate => comments.OrderBy(c => c.PublishedOn),
                OrderByOptions.ByLikes => comments.OrderBy(c => c.Likes),
                _ => throw new InvalidOperationException($"No such option {option} to order")
            };
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
            if (pageNumZeroStart != 0)
                query = query.Skip(pageNumZeroStart * pageSize);
            return query.Take(pageSize);
        }
    }
}