using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using B_M.Models;
using B_M.Filters;

namespace B_M.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly UserRepository userRepository;

        public AdminController()
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

        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                // Lấy thống kê tổng quan
                var stats = GetDashboardStats();
                return View(stats);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải trang quản trị: " + ex.Message;
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: Admin/Debug - Debug authentication
        [AllowAnonymous]
        public ActionResult Debug()
        {
            var debugInfo = new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                SessionUserID = Session["UserID"],
                SessionRole = Session["Role"],
                SessionIsActive = Session["IsActive"],
                CurrentTime = DateTime.Now.ToString(),
                RequestUrl = Request.Url?.ToString()
            };
            
            return Content($"Debug Info:<br/>" +
                $"IsAuthenticated: {debugInfo.IsAuthenticated}<br/>" +
                $"UserName: {debugInfo.UserName}<br/>" +
                $"Session UserID: {debugInfo.SessionUserID}<br/>" +
                $"Session Role: {debugInfo.SessionRole}<br/>" +
                $"Session IsActive: {debugInfo.SessionIsActive}<br/>" +
                $"Current Time: {debugInfo.CurrentTime}<br/>" +
                $"Request URL: {debugInfo.RequestUrl}");
        }


        // GET: Admin/Users
        public ActionResult Users(int? page, string search, string roleFilter)
        {
            try
            {
                var users = userRepository.GetAllUsers();
                
                // Lọc theo tìm kiếm
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(u => 
                        u.Email.Contains(search) || 
                        u.UserName.Contains(search) ||
                        (u.UserDetails != null && u.UserDetails.FullName.Contains(search))
                    ).ToList();
                }

                // Lọc theo role
                if (!string.IsNullOrEmpty(roleFilter) && int.TryParse(roleFilter, out int role))
                {
                    users = users.Where(u => u.Role == role).ToList();
                }

                // Phân trang
                int pageSize = 10;
                int pageNumber = page ?? 1;
                var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new AdminUsersViewModel
                {
                    Users = pagedUsers,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)users.Count / pageSize),
                    SearchTerm = search,
                    RoleFilter = roleFilter
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách người dùng: " + ex.Message;
                return View(new AdminUsersViewModel());
            }
        }

        // GET: Admin/UserDetails/5
        public ActionResult UserDetails(int id)
        {
            try
            {
                var user = userRepository.GetUserById(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                    return RedirectToAction("Users");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải thông tin người dùng: " + ex.Message;
                return RedirectToAction("Users");
            }
        }

        // POST: Admin/ToggleUserStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleUserStatus(int userId)
        {
            try
            {
                var user = userRepository.GetUserById(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự vô hiệu hóa tài khoản của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể vô hiệu hóa tài khoản của chính mình." });
                }

                user.IsActive = !user.IsActive;
                bool result = userRepository.UpdateUser(user);

                if (result)
                {
                    return Json(new { 
                        success = true, 
                        message = user.IsActive ? "Đã kích hoạt tài khoản." : "Đã vô hiệu hóa tài khoản.",
                        isActive = user.IsActive
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật trạng thái tài khoản." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/ChangeUserRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserRole(int userId, byte newRole)
        {
            try
            {
                var user = userRepository.GetUserById(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự thay đổi role của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể thay đổi quyền của chính mình." });
                }

                // Kiểm tra role hợp lệ
                if (newRole < 1 || newRole > 3)
                {
                    return Json(new { success = false, message = "Quyền không hợp lệ." });
                }

                user.Role = newRole;
                bool result = userRepository.UpdateUser(user);

                if (result)
                {
                    string roleName = GetRoleName(newRole);
                    return Json(new { 
                        success = true, 
                        message = $"Đã thay đổi quyền thành {roleName}.",
                        role = newRole,
                        roleName = roleName
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật quyền." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Admin/Posts
        public ActionResult Posts(int? page, string search, string typeFilter)
        {
            try
            {
                // TODO: Implement posts management with TradePost and CommunityPost
                // For now, return empty view
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách bài viết: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Reports
        public ActionResult Reports()
        {
            try
            {
                // TODO: Implement reports and analytics
                // For now, return empty view
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải báo cáo: " + ex.Message;
                return View();
            }
        }

        // GET: Admin/Settings
        public ActionResult Settings()
        {
            try
            {
                // TODO: Implement system settings
                // For now, return empty view
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải cài đặt: " + ex.Message;
                return View();
            }
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int userId)
        {
            try
            {
                var user = userRepository.GetUserById(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự xóa tài khoản của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể xóa tài khoản của chính mình." });
                }

                bool result = userRepository.DeleteUser(userId);

                if (result)
                {
                    return Json(new { 
                        success = true, 
                        message = "Đã xóa tài khoản thành công."
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi xóa tài khoản." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Admin/ExportUsers
        public ActionResult ExportUsers()
        {
            try
            {
                var users = userRepository.GetAllUsers();
                // TODO: Implement CSV/Excel export
                return Json(new { success = false, message = "Chức năng export chưa được implement." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Helper methods
        private AdminDashboardViewModel GetDashboardStats()
        {
            var users = userRepository.GetAllUsers();
            
            return new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                ActiveUsers = users.Count(u => u.IsActive),
                AdminUsers = users.Count(u => u.Role == 1),
                MomUsers = users.Count(u => u.Role == 2),
                BrandUsers = users.Count(u => u.Role == 3),
                NewUsersThisMonth = users.Count(u => u.CreatedAt >= DateTime.Now.AddMonths(-1)),
                RecentUsers = users.OrderByDescending(u => u.CreatedAt).Take(5).ToList()
            };
        }

        private string GetRoleName(byte role)
        {
            switch (role)
            {
                case 1: return "Quản trị viên";
                case 2: return "Mẹ bỉm";
                case 3: return "Nhãn hàng";
                default: return "Không xác định";
            }
        }
    }
}
