using FakeNews.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FakeNews.View.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
