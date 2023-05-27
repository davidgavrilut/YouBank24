using Microsoft.AspNetCore.Mvc;

namespace YouBank24.Controllers {
    public class TransactionListController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
