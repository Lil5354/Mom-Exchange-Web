using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using B_M.Models.Entities;
using B_M.Models.ViewModels;
using B_M.Filters;
using B_M.Data;

namespace B_M.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork?.Dispose();
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
                var users = unitOfWork.Users.GetAllUsers();
                
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
                var user = unitOfWork.Users.GetUserForAdminEdit(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                    return RedirectToAction("Users");
                }

                var viewModel = new AdminUserEditViewModel
                {
                    UserID = user.UserID,
                    Email = user.Email,
                    UserName = user.UserName ?? "Chưa thiết lập",
                    PhoneNumber = user.PhoneNumber ?? "Chưa cập nhật",
                    FullName = user.UserDetails?.FullName ?? "Chưa cập nhật",
                    Address = user.UserDetails?.Address ?? "Chưa cập nhật",
                    IsActive = user.IsActive,
                    Role = user.Role,
                    RoleName = GetRoleName(user.Role),
                    StatusName = user.IsActive ? "HOẠT ĐỘNG" : "BỊ KHÓA",
                    CreatedAt = user.CreatedAt,
                    ReputationScore = user.UserDetails?.ReputationScore ?? 0
                };

                return View(viewModel);
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
        public ActionResult ToggleUserStatus(int UserID, bool IsActive)
        {
            try
            {
                var user = unitOfWork.Users.GetUserById(UserID);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự vô hiệu hóa tài khoản của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể khóa tài khoản của chính mình." });
                }

                bool result = unitOfWork.Users.UpdateUserStatus(UserID, IsActive);

                if (result)
                {
                    string statusName = IsActive ? "HOẠT ĐỘNG" : "BỊ KHÓA";
                    string actionText = IsActive ? "mở khóa" : "khóa";
                    
                    return Json(new { 
                        success = true, 
                        message = $"Đã {actionText} tài khoản thành công.",
                        data = new {
                            isActive = IsActive,
                            statusName = statusName,
                            statusClass = IsActive ? "badge-success" : "badge-danger",
                            buttonText = IsActive ? "Khóa" : "Mở khóa",
                            buttonIcon = IsActive ? "lock" : "unlock"
                        }
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
        public ActionResult ChangeUserRole(int UserID, byte NewRole)
        {
            try
            {
                var user = unitOfWork.Users.GetUserById(UserID);
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
                if (NewRole < 1 || NewRole > 3)
                {
                    return Json(new { success = false, message = "Quyền không hợp lệ." });
                }

                bool result = unitOfWork.Users.UpdateUserRole(UserID, NewRole);

                if (result)
                {
                    string roleName = GetRoleName(NewRole);
                    return Json(new { 
                        success = true, 
                        message = $"Đã thay đổi quyền thành {roleName}.",
                        data = new {
                            role = NewRole,
                            roleName = roleName,
                            roleClass = GetRoleClass(NewRole)
                        }
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

        // POST: Admin/UpdateUserProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUserProfile(AdminUserEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                // Get current user
                var user = unitOfWork.Users.GetUserForAdminEdit(model.UserID);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Validation: Check for duplicate email (excluding current user)
                if (unitOfWork.Users.EmailExistsExcludingUser(model.Email, model.UserID))
                {
                    return Json(new { success = false, message = "Email này đã được sử dụng bởi người dùng khác." });
                }

                // Validation: Check for duplicate username (excluding current user)
                if (!string.IsNullOrEmpty(model.UserName) && 
                    unitOfWork.Users.UsernameExistsExcludingUser(model.UserName, model.UserID))
                {
                    return Json(new { success = false, message = "Tên đăng nhập này đã được sử dụng bởi người dùng khác." });
                }

                // Update User information
                user.Email = model.Email;
                user.UserName = string.IsNullOrEmpty(model.UserName) ? null : model.UserName;
                user.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? null : model.PhoneNumber;

                // Update UserDetails
                var userDetails = unitOfWork.Users.GetUserDetails(model.UserID);
                if (userDetails != null)
                {
                    userDetails.FullName = model.FullName;
                    userDetails.Address = string.IsNullOrEmpty(model.Address) ? null : model.Address;
                }
                else
                {
                    // Create UserDetails if not exists
                    userDetails = new UserDetails
                    {
                        UserID = model.UserID,
                        FullName = model.FullName,
                        Address = string.IsNullOrEmpty(model.Address) ? null : model.Address,
                        ReputationScore = 0
                    };
                }

                // Save changes
                bool result = unitOfWork.Users.UpdateUserProfile(user, userDetails);

                if (result)
                {
                    return Json(new { 
                        success = true, 
                        message = "Cập nhật thông tin người dùng thành công.",
                        data = new {
                            email = user.Email,
                            username = user.UserName ?? "Chưa thiết lập",
                            phoneNumber = user.PhoneNumber ?? "Chưa cập nhật",
                            fullName = userDetails.FullName,
                            address = userDetails.Address ?? "Chưa cập nhật"
                        }
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật thông tin." });
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
                var user = unitOfWork.Users.GetUserById(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự xóa tài khoản của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể xóa tài khoản của chính mình." });
                }

                bool result = unitOfWork.Users.DeleteUser(userId);

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
                var users = unitOfWork.Users.GetAllUsers();
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
            var users = unitOfWork.Users.GetAllUsers();
            
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

        private string GetRoleClass(byte role)
        {
            switch (role)
            {
                case 1: return "badge-danger";
                case 2: return "badge-warning";
                case 3: return "badge-info";
                default: return "badge-secondary";
            }
        }
    }
}
