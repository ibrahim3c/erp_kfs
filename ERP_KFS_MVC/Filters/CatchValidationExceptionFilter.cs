using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Modules.Shared.Application.Exceptions;

namespace ERP_KFS_MVC.Filters
{
    public class CatchValidationExceptionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // بنسيب الـ Action يشتغل عادي جداً
            var executedContext = await next();

            // بنسأل: هل حصل Exception ونوعه ValidationException؟
            if (executedContext.Exception is ValidationException validationException)
            {
                // 1. بنقول للسيستم: خلاص إحنا اتعاملنا مع المشكلة، ماتعملش Crash
                executedContext.ExceptionHandled = true;

                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    // 2. بنضيف الأخطاء جوا الـ ModelState
                    foreach (var error in validationException.Errors)
                    {
                        controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    // 3. بنجيب الـ ViewModel اللي اليوزر كان باعته عشان نرجعهوله بالداتا بتاعته
                    var model = context.ActionArguments.Values.FirstOrDefault();

                    // 4. بنجبر الـ Controller يرجع الـ View بدل ما يكمل الـ Pipeline
                    executedContext.Result = new ViewResult
                    {
                        ViewName = context.ActionDescriptor.RouteValues["action"], // نفس اسم الـ Action
                        ViewData = controller.ViewData, // هنا الـ ModelState اتحدث بالأخطاء
                        TempData = controller.TempData
                    };
                }
            }
        }
    }
}