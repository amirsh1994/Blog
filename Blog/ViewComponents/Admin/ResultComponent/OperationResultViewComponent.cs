using Blog.Core.Common;
using Blog.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.ViewComponents.Admin.ResultComponent;

[ViewComponent(Name = "ResultViewComponent")]
public class OperationResultViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var op = new OperationResult();

        if (TempData[TempDataName.ResultTempData] is not null)
        {
            op = JsonConvert.DeserializeObject<OperationResult>(TempData[TempDataName.ResultTempData]?.ToString() ?? "");
        }

        return View(op);
    }
}