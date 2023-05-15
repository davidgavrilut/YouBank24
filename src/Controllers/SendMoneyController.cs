using Microsoft.AspNetCore.Mvc;

namespace YouBank24.Controllers; 
public class SendMoneyController : Controller {
    public IActionResult Index() {
        return View();
    }
}
