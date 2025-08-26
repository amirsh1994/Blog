using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.ViewComponents.Admin.ResultComponent;

[ViewComponent(Name ="ResultViewComponent")]
public class OperationResultViewComponent:ViewComponent
{
    public async Task<IViewComponentResult>InvokeAsync()
    {

        return View();
    }
}