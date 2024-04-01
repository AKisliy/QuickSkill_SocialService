using SocialService.Core.Exceptions;

namespace SocialService.WebApi.Extensions
{
    public static class HttpExtension
    {
        private const string claimPrefix = "X-Claim-";
        public static int GetUserIdFromHeader(this HttpContext context)
        {
            foreach(var key in context.Request.Headers.ToDictionary().Keys)
                System.Console.WriteLine(key.ToString());
            if(!context.Request.Headers.TryGetValue(claimPrefix + "userId", out var claimValue))
                throw new BadRequestException("User id is missing!");
            if(!int.TryParse(claimValue, out int id))
                throw new BadRequestException("User id isn't valid!!");
            return id;
        }
    }
}