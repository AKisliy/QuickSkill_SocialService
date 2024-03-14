using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialService.Core.Enums;

namespace SocialService.DataAccess.Extensions
{
    public static class IQueryableExtesion
    {
        public static IQueryable<Answer> OrderAnswersBy(this IQueryable<Answer> answers, OrderByOptions option)
        {
            switch(option)
            {
                case OrderByOptions.SimpleOrder:
                    return answers.OrderBy(a => a.Id);
                case OrderByOptions.ByDate:
                    return answers.OrderBy(a => a.PublishedOn);
                case OrderByOptions.ByLikes:
                    return answers.OrderByDescending(a => a.Likes);
                default:
                    throw new InvalidOperationException($"No such option {option} to order");
            }
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            if(pageSize <= 0)
                throw new ArgumentOutOfRangeException("Page size can't be negative or 0");
            if(pageNumZeroStart != 0)
                query.Skip(pageNumZeroStart * pageSize);
            return query.Take(pageSize);
        }
    }
}