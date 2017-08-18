using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Threading.Tasks;

namespace Exam.CustomAttributes
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d <= DateTime.Now;

        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d > DateTime.Now;

        }
    }



    // How can I make a model owner validator
    // https://stackoverflow.com/questions/39256341/how-to-use-action-filters-with-dependency-injection-in-asp-net-core
    // https://stackoverflow.com/questions/39181390/how-do-i-add-a-parameter-to-an-action-filter-in-asp-net
    // https://msdn.microsoft.com/en-us/library/ee707357(v=vs.91).aspx
    // https://stackoverflow.com/questions/31464359/how-do-you-create-a-custom-authorizeattribute-in-asp-net-core 
    // https://stackoverflow.com/questions/12500085/model-ownership-checking
    // 	    In MVC5 filterContext.ActionParameters has become filterContext.ActionDescriptor.GetParameters() (returns an array of System.Web.Mvc.ParameterDescriptor) – barbara.post Aug 3 at 9:28
    // https://stackoverflow.com/questions/16649551/get-user-name-on-action-filter\
    // https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies

    // public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    // {
    //     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    //     {
    //         if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
    //                                    c.Issuer == "http://contoso.com"))
    //         {
    //             // .NET 4.x -> return Task.FromResult(0);
    //             return Task.CompletedTask;
    //         }

    //         var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(
    //             c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "http://contoso.com").Value);

    //         int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
    //         if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
    //         {
    //             calculatedAge--;
    //         }

    //         if (calculatedAge >= requirement.MinimumAge)
    //         {
    //             context.Succeed(requirement);
    //         }
    //         return Task.CompletedTask;
    //     }
    // }

    // public class MinimumAgeRequirement : IAuthorizationRequirement
    // {
    //     public int MinimumAge { get; private set; }

    //     public MinimumAgeRequirement(int minimumAge)
    //     {
    //         MinimumAge = minimumAge;
    //     }
    // }



    // public class VerifyOwnership : AuthorizationFilterAttribute
    // {
    //     public void OnActionExecuting(HttpActionContext actionContext, ActionExecutingContext filterContext)
    //     {
    //         HttpContext
    //             foreach (ParameterDescriptor parameter in filterContext.ActionDescriptor.Parameters)
    //         {
    //             var owned = parameter.ParameterType as IHaveAnOwner;
    //             if (owned != null)
    //             {
    //                 if (owned.OwnerId != WebSecurity.CurrentUserId)
    //                 {
    //                     // ... not found or access denied
    //                 }
    //             }
    //         }
    //     }

    //     public void OnActionExecuted(ActionExecutedContext filterContext)
    //     {

    //     }
    // }
}
