using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Yunyong.Mvc
{
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        public Controller()
        {
            BeforeAction += Controller_BeforeAction;
        }

        /// <summary>
        ///     Occurs when [before action].
        /// </summary>
        public event Action BeforeAction;

        /// <summary>
        ///     BeforeAction Event
        /// </summary>
        protected virtual void Controller_BeforeAction()
        {
        }

        /// <summary>
        ///     Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            BeforeAction?.Invoke();
        }
    }

    ///// <summary>
    ///// </summary>
    //public class SecurityRequirementsOperationFilter : IOperationFilter
    //{
    //    /// <summary>
    //    /// </summary>
    //    /// <param name="scope"></param>
    //    public SecurityRequirementsOperationFilter(string scope = null)
    //    {
    //        Scope = scope;
    //    }

    //    private string Scope { get; }

    //    /// <summary>
    //    /// </summary>
    //    /// <param name="operation"></param>
    //    /// <param name="context"></param>
    //    public void Apply(Operation operation, OperationFilterContext context)
    //    {
    //        if (context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Any())
    //        {
    //            operation.Responses.Add("401", new Response { Description = "Unauthorized" });
    //            operation.Responses.Add("403", new Response { Description = "Forbidden" });

    //            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
    //            {
    //                new Dictionary<string, IEnumerable<string>>
    //                {
    //                    {"oauth2", new List<string> {Scope}}
    //                }
    //            };
    //        }
    //    }
    //}
}