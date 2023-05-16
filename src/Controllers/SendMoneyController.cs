using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Controllers; 
public class SendMoneyController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private ClaimsIdentity _claimsIdentity;
    private Claim _claim;


    public SendMoneyController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() {
        return View();
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetSendMoneyData() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll(x => x.Id != _claim.Value);
        foreach(var user in users) {
            return Json(new { id = user.Id, firstName = user.FirstName, lastName = user.LastName, email = user.Email });
        }
        return NotFound();
    }   
    #endregion
}
