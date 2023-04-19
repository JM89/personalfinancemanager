using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Models.UserProfile;
using PersonalFinanceManager.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class UserProfileController : BaseController
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._userProfileService = userProfileService;
        }

        /// <summary>
        /// Initialize the View.
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ViewDetails")]
        public async Task<ActionResult> ViewDetails()
        {
            var userProfileModel = await _userProfileService.GetByUserId(User.Identity.GetUserId());

            return View("View", userProfileModel);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var userProfileEditModel = new UserProfileEditModel();

            return View(userProfileEditModel);
        }

        /// <summary>
        /// Create a new user profile.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserProfileEditModel userProfileEditModel)
        {
            if (ModelState.IsValid)
            {
                userProfileEditModel.User_Id = User.Identity.GetUserId();
                await _userProfileService.CreateUserProfile(userProfileEditModel);

                return RedirectToAction("ViewDetails");
            }

            return View(userProfileEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Country id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userProfileModel = await _userProfileService.GetById(id.Value);
            
            if (userProfileModel == null)
            {
                return HttpNotFound();
            }

            return View(userProfileModel);
        }

        /// <summary>
        /// Update an existing user profile.
        /// </summary>
        /// <param name="userProfileEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserProfileEditModel userProfileEditModel)
        {
            if (ModelState.IsValid)
            {
                await _userProfileService.EditUserProfile(userProfileEditModel);
                
                return RedirectToAction("ViewDetails");
            }
            return View(userProfileEditModel);
        }
    }
}
