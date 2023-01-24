using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor?.HttpContext?.User?.GetClaim<string>(ClaimTypes.NameIdentifier);
        }
        public static string GetUserName(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor?.HttpContext?.User?.GetClaim<string>("name");
        }
        public static string GetFullName(this IHttpContextAccessor httpContextAccessor)
        {
            return string.Format($"{httpContextAccessor?.HttpContext?.User?.GetClaim<string>(ClaimTypes.GivenName)} {httpContextAccessor?.HttpContext?.User?.GetClaim<string>(ClaimTypes.Surname)}");
        }
        public static string GetEmailUser(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor?.HttpContext?.User?.GetClaim<string>(ClaimTypes.Email);
        }

        public static TId GetClaim<TId>(this ClaimsPrincipal principal, string type)
        {
            if (principal == null || principal.Identity == null ||
                !principal.Identity.IsAuthenticated)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var loggedInUserId = principal.FindFirst(type)?.Value;

            if (typeof(TId) == typeof(string) ||
                typeof(TId) == typeof(int) ||
                typeof(TId) == typeof(long) ||
                typeof(TId) == typeof(Guid))
            {
                var converter = TypeDescriptor.GetConverter(typeof(TId));

                return (TId)converter.ConvertFromInvariantString(loggedInUserId);
            }

            throw new InvalidOperationException("The user id type is invalid.");
        }
    }
}
