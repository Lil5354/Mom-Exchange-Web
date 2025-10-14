// File: Controllers/AccountController.cs
using MomExchange.Models;
using MomExchange.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin.Security;

namespace MomExchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository userRepository;

        public AccountController()
        {
            userRepository = new UserRepository();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userRepository?.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            // Debug: Log OWIN context
            try
            {
                var owinContext = System.Web.HttpContext.Current.GetOwinContext();
                System.Diagnostics.Debug.WriteLine($"=== LOGIN PAGE DEBUG ===");
                System.Diagnostics.Debug.WriteLine($"OWIN Context exists: {owinContext != null}");
                System.Diagnostics.Debug.WriteLine($"Authentication exists: {owinContext?.Authentication != null}");
                System.Diagnostics.Debug.WriteLine($"Current URL: {Request.Url}");
                System.Diagnostics.Debug.WriteLine($"========================");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR getting OWIN context: {ex.Message}");
            }
            
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Lấy user từ database (theo email hoặc username)
                User user = userRepository.GetUserByEmailOrUsername(model.EmailOrUsername);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email/Tên đăng nhập hoặc mật khẩu không đúng.");
                    return View(model);
                }

                // Kiểm tra tài khoản có active không
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị vô hiệu hóa.");
                    return View(model);
                }

                // Verify password
                if (!PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Email/Tên đăng nhập hoặc mật khẩu không đúng.");
                    return View(model);
                }

                // Đăng nhập thành công
                // Tạo authentication cookie
                FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);

                // Lưu thông tin user vào session
                Session["UserID"] = user.UserID;
                Session["UserEmail"] = user.Email;
                Session["FullName"] = user.UserDetails?.FullName ?? "User";
                Session["Role"] = user.Role;

                // Chuyển hướng
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.");
                // Log error here if needed
                return View(model);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra username đã tồn tại chưa (chỉ khi có nhập username)
                    if (!string.IsNullOrEmpty(model.UserName) && userRepository.UsernameExists(model.UserName))
                    {
                        ModelState.AddModelError("UserName", "Tên đăng nhập này đã được sử dụng.");
                        ViewBag.ShowRegister = true;
                        return View("Login", model);
                    }

                    // Kiểm tra email đã tồn tại chưa
                    if (userRepository.EmailExists(model.Email))
                    {
                        ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                        ViewBag.ShowRegister = true;
                        return View("Login", model);
                    }

                    // Tạo user mới
                    User newUser = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        PasswordHash = PasswordHelper.HashPassword(model.Password),
                        Role = 2, // Default role: Mom
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    // Tạo user details
                    UserDetails newUserDetails = new UserDetails
                    {
                        FullName = model.FullName,
                        ReputationScore = 0
                    };

                    // Lưu vào database
                    bool result = userRepository.CreateUser(newUser, newUserDetails);

                    if (result)
                    {
                        // Đăng ký thành công
                        TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                        ViewBag.ShowRegister = true;
                        return View("Login", model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                    // Log error here if needed
                    ViewBag.ShowRegister = true;
                    return View("Login", model);
                }
            }

            // Nếu có lỗi, quay lại form với các lỗi được hiển thị
            ViewBag.ShowRegister = true;
            return View("Login", model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Debug logging
            System.Diagnostics.Debug.WriteLine($"=== EXTERNAL LOGIN ===");
            System.Diagnostics.Debug.WriteLine($"Provider: {provider}");
            System.Diagnostics.Debug.WriteLine($"Return URL: {returnUrl}");
            
            // Request a redirect to the external login provider
            var redirectUri = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }, Request.Url.Scheme);
            System.Diagnostics.Debug.WriteLine($"Redirect URI: {redirectUri}");
            System.Diagnostics.Debug.WriteLine($"======================");
            
            return new ChallengeResult(provider, redirectUri);
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== GOOGLE CALLBACK ===");
                System.Diagnostics.Debug.WriteLine($"Return URL: {returnUrl}");

                var loginInfo = System.Web.HttpContext.Current.GetOwinContext().Authentication.GetExternalLoginInfoAsync().Result;
                if (loginInfo == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: loginInfo is null");
                    TempData["ErrorMessage"] = "Không thể lấy thông tin từ Google. Vui lòng thử lại.";
                    return RedirectToAction("Login");
                }

                // Lấy thông tin từ Google
                var email = loginInfo.Email;
                var name = loginInfo.ExternalIdentity.Name;
                
                System.Diagnostics.Debug.WriteLine($"Google Email: {email}");
                System.Diagnostics.Debug.WriteLine($"Google Name: {name}");

                if (string.IsNullOrEmpty(email))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Email is null or empty");
                    TempData["ErrorMessage"] = "Google không cung cấp địa chỉ email. Vui lòng thử lại.";
                    return RedirectToAction("Login");
                }

                // Kiểm tra user đã tồn tại chưa
                var existingUser = userRepository.GetUserByEmail(email);
                
                if (existingUser != null)
                {
                    // User đã tồn tại - đăng nhập
                    System.Diagnostics.Debug.WriteLine($"EXISTING USER: {existingUser.Email}");
                    
                    FormsAuthentication.SetAuthCookie(existingUser.Email, false);
                    Session["UserID"] = existingUser.UserID;
                    Session["UserEmail"] = existingUser.Email;
                    Session["FullName"] = existingUser.UserDetails?.FullName ?? name;
                    Session["Role"] = existingUser.Role;

                    System.Diagnostics.Debug.WriteLine($"LOGIN SUCCESS: {existingUser.Email}");
                    TempData["SuccessMessage"] = $"Chào mừng trở lại, {existingUser.UserDetails?.FullName ?? name}!";
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    // Tạo user mới từ Google account
                    System.Diagnostics.Debug.WriteLine($"CREATING NEW USER: {email}");
                    
                    var newUser = new User
                    {
                        Email = email,
                        PasswordHash = PasswordHelper.HashPassword(Guid.NewGuid().ToString()), // Random password
                        Role = 2, // Mom role
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    var newUserDetails = new UserDetails
                    {
                        FullName = name ?? email.Split('@')[0],
                        ReputationScore = 0
                    };

                    System.Diagnostics.Debug.WriteLine($"Attempting to create user: {email}");
                    System.Diagnostics.Debug.WriteLine($"User details: FullName={newUserDetails.FullName}");
                    
                    bool result = userRepository.CreateUser(newUser, newUserDetails);

                    if (result)
                    {
                        // Đăng nhập sau khi tạo account
                        var createdUser = userRepository.GetUserByEmail(email);
                        if (createdUser != null)
                        {
                            // Lưu thông tin vào session để hiển thị form complete profile
                            Session["TempUserID"] = createdUser.UserID;
                            Session["TempEmail"] = createdUser.Email;
                            Session["TempFullName"] = createdUser.UserDetails?.FullName;
                            Session["TempRole"] = createdUser.Role;

                            System.Diagnostics.Debug.WriteLine($"REGISTRATION SUCCESS: {createdUser.Email}");
                            
                            // Redirect đến trang complete profile thay vì đăng nhập ngay
                            return RedirectToAction("CompleteProfile");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: User created but cannot retrieve from database");
                            TempData["ErrorMessage"] = "Tài khoản đã được tạo nhưng không thể đăng nhập. Vui lòng thử đăng nhập thủ công.";
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Failed to create user - check database connection and structure");
                        TempData["ErrorMessage"] = "Không thể tạo tài khoản. Vui lòng kiểm tra kết nối database hoặc chạy script FixDatabase.sql.";
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION in ExternalLoginCallback: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.";
                return RedirectToAction("Login");
            }
        }

        // GET: /Account/CompleteProfile
        [AllowAnonymous]
        public ActionResult CompleteProfile()
        {
            // Kiểm tra session có thông tin temp user không
            if (Session["TempUserID"] == null)
            {
                TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login");
            }

            var model = new CompleteProfileViewModel
            {
                UserID = (int)Session["TempUserID"],
                Email = Session["TempEmail"]?.ToString(),
                FullName = Session["TempFullName"]?.ToString()
            };

            return View(model);
        }

        // POST: /Account/CompleteProfile
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteProfile(CompleteProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kiểm tra session
                if (Session["TempUserID"] == null)
                {
                    TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login");
                }

                // Kiểm tra username đã tồn tại chưa
                if (!string.IsNullOrEmpty(model.UserName) && userRepository.UsernameExists(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập này đã được sử dụng.");
                    return View(model);
                }

                // Lấy user hiện tại
                var user = userRepository.GetUserByEmail(model.Email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Login");
                }

                // Cập nhật thông tin user
                user.UserName = model.UserName;
                user.PasswordHash = PasswordHelper.HashPassword(model.Password);
                user.PhoneNumber = model.PhoneNumber;

                // Cập nhật user details
                var userDetails = userRepository.GetUserDetails(user.UserID);
                if (userDetails != null)
                {
                    userDetails.Address = model.Address;
                }

                // Lưu thay đổi
                bool updateResult = userRepository.UpdateUser(user);
                if (userDetails != null)
                {
                    userRepository.UpdateUserDetails(userDetails);
                }

                if (updateResult)
                {
                    // Xóa session temp
                    Session.Remove("TempUserID");
                    Session.Remove("TempEmail");
                    Session.Remove("TempFullName");
                    Session.Remove("TempRole");

                    // Đăng nhập user
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["UserID"] = user.UserID;
                    Session["UserEmail"] = user.Email;
                    Session["FullName"] = user.UserDetails?.FullName;
                    Session["Role"] = user.Role;

                    TempData["SuccessMessage"] = $"Chào mừng đến với MomExchange, {user.UserDetails?.FullName}! Hồ sơ của bạn đã được hoàn thiện.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in CompleteProfile: {ex.Message}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại.");
                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // Challenge Result for OAuth
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                System.Web.HttpContext.Current.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}