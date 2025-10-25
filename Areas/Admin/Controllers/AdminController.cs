using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using B_M.Models;
using B_M.Filters;
using B_M.Helpers;

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
        public ActionResult Users(int? page, string search, string roleFilter, 
            string emailSearch, string usernameSearch, string fullNameSearch, 
            string phoneSearch, string addressSearch, string statusFilter,
            DateTime? createdFrom, DateTime? createdTo, string sortBy, string sortOrder,
            bool showAdvancedSearch = false, bool caseSensitive = false, bool exactMatch = false)
        {
            try
            {
                var users = userRepository.GetAllUsers();
                var totalUsersCount = users.Count;
                
                // Apply advanced search filters
                users = ApplyAdvancedSearchFilters(users, search, roleFilter, emailSearch, 
                    usernameSearch, fullNameSearch, phoneSearch, addressSearch, 
                    statusFilter, createdFrom, createdTo, caseSensitive, exactMatch);
                
                // Apply sorting
                users = ApplySorting(users, sortBy, sortOrder);
                
                // Pagination
                int pageSize = 10;
                int pageNumber = page ?? 1;
                var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new AdminUsersViewModel
                {
                    Users = pagedUsers,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)users.Count / pageSize),
                    TotalUsers = totalUsersCount,
                    
                    // Basic search
                    SearchTerm = search,
                    RoleFilter = roleFilter,
                    
                    // Advanced search
                    EmailSearch = emailSearch,
                    UsernameSearch = usernameSearch,
                    FullNameSearch = fullNameSearch,
                    PhoneSearch = phoneSearch,
                    AddressSearch = addressSearch,
                    StatusFilter = statusFilter,
                    CreatedFrom = createdFrom,
                    CreatedTo = createdTo,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    
                    // Search options
                    ShowAdvancedSearch = showAdvancedSearch,
                    CaseSensitive = caseSensitive,
                    ExactMatch = exactMatch
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
                var user = userRepository.GetUserForAdminEdit(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                    return RedirectToAction("Users");
                }

                var viewModel = new B_M.Models.AdminUserEditViewModel
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
                var user = userRepository.GetUserById(UserID);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Không cho phép admin tự vô hiệu hóa tài khoản của mình
                if (user.UserID == (int)Session["UserID"])
                {
                    return Json(new { success = false, message = "Bạn không thể khóa tài khoản của chính mình." });
                }

                bool result = userRepository.UpdateUserStatus(UserID, IsActive);

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
                var user = userRepository.GetUserById(UserID);
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

                bool result = userRepository.UpdateUserRole(UserID, NewRole);

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
        public ActionResult UpdateUserProfile(B_M.Models.AdminUserEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                // Get current user
                var user = userRepository.GetUserForAdminEdit(model.UserID);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                // Validation: Check for duplicate email (excluding current user)
                if (userRepository.EmailExistsExcludingUser(model.Email, model.UserID))
                {
                    return Json(new { success = false, message = "Email này đã được sử dụng bởi người dùng khác." });
                }

                // Validation: Check for duplicate username (excluding current user)
                if (!string.IsNullOrEmpty(model.UserName) && 
                    userRepository.UsernameExistsExcludingUser(model.UserName, model.UserID))
                {
                    return Json(new { success = false, message = "Tên đăng nhập này đã được sử dụng bởi người dùng khác." });
                }

                // Update User information
                user.Email = model.Email;
                user.UserName = string.IsNullOrEmpty(model.UserName) ? null : model.UserName;
                user.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? null : model.PhoneNumber;

                // Update UserDetails
                var userDetails = userRepository.GetUserDetails(model.UserID);
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
                bool result = userRepository.UpdateUserProfile(user, userDetails);

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
        public ActionResult Posts(int? page, string search, string typeFilter, string statusFilter,
            string authorSearch, string titleSearch, string contentSearch, 
            DateTime? createdFrom, DateTime? createdTo, string sortBy, string sortOrder,
            bool showAdvancedSearch = false, bool caseSensitive = false, bool exactMatch = false)
        {
            try
            {
                // Get all posts from different sources
                var allPosts = GetAllPosts();
                var totalPostsCount = allPosts.Count;
                
                // Apply filters
                allPosts = ApplyPostFilters(allPosts, search, typeFilter, statusFilter, 
                    authorSearch, titleSearch, contentSearch, createdFrom, createdTo, 
                    caseSensitive, exactMatch);
                
                // Apply sorting
                allPosts = ApplyPostSorting(allPosts, sortBy, sortOrder);
                
                // Pagination
                int pageSize = 10;
                int pageNumber = page ?? 1;
                var pagedPosts = allPosts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new AdminPostsViewModel
                {
                    Posts = pagedPosts,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)allPosts.Count / pageSize),
                    TotalPosts = totalPostsCount,
                    
                    // Basic search
                    SearchTerm = search,
                    TypeFilter = typeFilter,
                    StatusFilter = statusFilter,
                    
                    // Advanced search
                    AuthorSearch = authorSearch,
                    TitleSearch = titleSearch,
                    ContentSearch = contentSearch,
                    CreatedFrom = createdFrom,
                    CreatedTo = createdTo,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    
                    // Search options
                    ShowAdvancedSearch = showAdvancedSearch,
                    CaseSensitive = caseSensitive,
                    ExactMatch = exactMatch,
                    
                    // Statistics
                    TotalCommunityPosts = allPosts.Count(p => p.Type == "Community"),
                    TotalTradePosts = allPosts.Count(p => p.Type == "Trade"),
                    TotalSocialPosts = allPosts.Count(p => p.Type == "Social"),
                    TotalMilkPosts = allPosts.Count(p => p.Type == "Milk"),
                    PendingPosts = allPosts.Count(p => p.Status == "Pending"),
                    RejectedPosts = allPosts.Count(p => p.Status == "Rejected")
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách bài viết: " + ex.Message;
                return View(new AdminPostsViewModel());
            }
        }

        // GET: Admin/Reports
        public ActionResult Reports()
        {
            try
            {
                // Debug: Log bắt đầu
                System.Diagnostics.Debug.WriteLine("=== REPORTS ACTION STARTED ===");
                
                // Get dashboard stats for reports
                var stats = GetDashboardStats();
                System.Diagnostics.Debug.WriteLine($"Stats loaded: Total={stats.TotalUsers}");
                
                // Create reports view model
                var reportsViewModel = new AdminDashboardViewModel
                {
                    TotalUsers = stats.TotalUsers,
                    ActiveUsers = stats.ActiveUsers,
                    AdminUsers = stats.AdminUsers,
                    MomUsers = stats.MomUsers,
                    BrandUsers = stats.BrandUsers,
                    NewUsersThisMonth = stats.NewUsersThisMonth,
                    RecentUsers = stats.RecentUsers
                };
                
                System.Diagnostics.Debug.WriteLine($"Model created: Total={reportsViewModel.TotalUsers}");
                
                return View(reportsViewModel);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                System.Diagnostics.Debug.WriteLine($"=== ERROR IN REPORTS ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                
                // Hiển thị lỗi cho user
                TempData["ErrorMessage"] = $"LỖI CHI TIẾT: {ex.Message}";
                
                // Return view với model rỗng
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: Admin/TestReports - Test action
        public ActionResult TestReports()
        {
            return Content("TEST SUCCESS - " + DateTime.Now + " - Reports functionality is working!");
        }

        // GET: Admin/TestSimple - Test đơn giản nhất
        public ActionResult TestSimple()
        {
            return Content("SIMPLE TEST OK");
        }

        // GET: Admin/TestReportsModel - Test với model đơn giản
        public ActionResult TestReportsModel()
        {
            try
            {
                // Test với model đơn giản
                var testModel = new AdminDashboardViewModel
                {
                    TotalUsers = 100,
                    ActiveUsers = 80,
                    AdminUsers = 5,
                    MomUsers = 70,
                    BrandUsers = 25,
                    NewUsersThisMonth = 10,
                    RecentUsers = new List<User>()
                };
                
                return View("Reports", testModel);
            }
            catch (Exception ex)
            {
                return Content("ERROR: " + ex.Message);
            }
        }

        // GET: Admin/TestDatabase - Test database connection
        public ActionResult TestDatabase()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== TESTING DATABASE CONNECTION ===");
                
                var users = userRepository.GetAllUsers();
                System.Diagnostics.Debug.WriteLine($"Database test: Found {users.Count} users");
                
                var result = $"DATABASE TEST SUCCESS!<br/>" +
                           $"Found {users.Count} users<br/>" +
                           $"Active users: {users.Count(u => u.IsActive)}<br/>" +
                           $"Mom users: {users.Count(u => u.Role == 2)}<br/>" +
                           $"Brand users: {users.Count(u => u.Role == 3)}<br/>" +
                           $"Time: {DateTime.Now}";
                
                return Content(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DATABASE ERROR: {ex.Message}");
                return Content($"DATABASE ERROR: {ex.Message}<br/>Stack: {ex.StackTrace}");
            }
        }

        // GET: Admin/TestStats - Test GetDashboardStats method
        public ActionResult TestStats()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== TESTING GETDASHBOARDSTATS ===");
                
                var stats = GetDashboardStats();
                System.Diagnostics.Debug.WriteLine($"Stats test: Total={stats.TotalUsers}, Active={stats.ActiveUsers}");
                
                var result = $"STATS TEST SUCCESS!<br/>" +
                           $"Total Users: {stats.TotalUsers}<br/>" +
                           $"Active Users: {stats.ActiveUsers}<br/>" +
                           $"Admin Users: {stats.AdminUsers}<br/>" +
                           $"Mom Users: {stats.MomUsers}<br/>" +
                           $"Brand Users: {stats.BrandUsers}<br/>" +
                           $"New This Month: {stats.NewUsersThisMonth}<br/>" +
                           $"Time: {DateTime.Now}";
                
                return Content(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"STATS ERROR: {ex.Message}");
                return Content($"STATS ERROR: {ex.Message}<br/>Stack: {ex.StackTrace}");
            }
        }

        // GET: Admin/Settings
        public ActionResult Settings()
        {
            try
            {
                var viewModel = new SettingsViewModel();
                
                // Load default settings (in real implementation, load from database)
                LoadDefaultSettings(viewModel);
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải cài đặt: " + ex.Message;
                return View(new SettingsViewModel());
            }
        }

        // POST: Admin/SaveEmailSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmailSettings(EmailSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save email settings to database
                // For now, just return success
                
                return Json(new { 
                    success = true, 
                    message = "Cài đặt email đã được lưu thành công.",
                    data = new {
                        smtpHost = model.SmtpHost,
                        smtpPort = model.SmtpPort,
                        enableSSL = model.EnableSSL,
                        fromEmail = model.FromEmail,
                        fromName = model.FromName,
                        isEnabled = model.IsEnabled
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/TestEmailSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestEmailSettings(EmailSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Implement actual email testing
                // For now, simulate test result
                bool testResult = !string.IsNullOrEmpty(model.SmtpHost) && !string.IsNullOrEmpty(model.Username);
                
                return Json(new { 
                    success = true, 
                    message = testResult ? "Test email thành công!" : "Test email thất bại!",
                    data = new {
                        testResult = testResult,
                        testTime = DateTime.Now
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi test email: " + ex.Message });
            }
        }

        // POST: Admin/SaveSecuritySettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSecuritySettings(SecuritySettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save security settings to database
                
                return Json(new { 
                    success = true, 
                    message = "Cài đặt bảo mật đã được lưu thành công.",
                    data = new {
                        minPasswordLength = model.MinPasswordLength,
                        sessionTimeout = model.SessionTimeoutMinutes,
                        maxLoginAttempts = model.MaxLoginAttempts,
                        enableTwoFactor = model.EnableTwoFactor
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/SaveNotificationSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNotificationSettings(NotificationSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save notification settings to database
                
                return Json(new { 
                    success = true, 
                    message = "Cài đặt thông báo đã được lưu thành công.",
                    data = new {
                        enableEmail = model.EnableEmailNotifications,
                        enablePush = model.EnablePushNotifications,
                        notifyNewUser = model.NotifyNewUserRegistration
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/SaveSystemConfiguration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSystemConfiguration(SystemConfigurationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save system configuration to database
                
                return Json(new { 
                    success = true, 
                    message = "Cấu hình hệ thống đã được lưu thành công.",
                    data = new {
                        siteName = model.SiteName,
                        maintenanceMode = model.MaintenanceMode,
                        maxFileSize = model.MaxFileUploadSizeMB
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/SaveBackupSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBackupSettings(BackupSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save backup settings to database
                
                return Json(new { 
                    success = true, 
                    message = "Cài đặt sao lưu đã được lưu thành công.",
                    data = new {
                        enableAutoBackup = model.EnableAutoBackup,
                        backupFrequency = model.BackupFrequencyDays,
                        keepBackupCount = model.KeepBackupCount
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/SaveMonitoringSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMonitoringSettings(MonitoringSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = GetModelErrors() });
                }

                // TODO: Save monitoring settings to database
                
                return Json(new { 
                    success = true, 
                    message = "Cài đặt giám sát đã được lưu thành công.",
                    data = new {
                        enableSystemMonitoring = model.EnableSystemMonitoring,
                        enableErrorTracking = model.EnableErrorTracking,
                        logRetentionDays = model.LogRetentionDays
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/CreateBackup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBackup()
        {
            try
            {
                // TODO: Implement actual backup creation
                // For now, simulate backup process
                
                return Json(new { 
                    success = true, 
                    message = "Sao lưu đã được tạo thành công.",
                    data = new {
                        backupId = Guid.NewGuid().ToString(),
                        backupTime = DateTime.Now,
                        backupSize = "25.6 MB"
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo sao lưu: " + ex.Message });
            }
        }

        // Helper methods
        private void LoadDefaultSettings(SettingsViewModel viewModel)
        {
            // Email Settings
            viewModel.EmailSettings.SmtpHost = "smtp.gmail.com";
            viewModel.EmailSettings.SmtpPort = 587;
            viewModel.EmailSettings.EnableSSL = true;
            viewModel.EmailSettings.FromEmail = "noreply@momexchange.com";
            viewModel.EmailSettings.FromName = "MomExchange System";
            viewModel.EmailSettings.IsEnabled = true;

            // Security Settings
            viewModel.SecuritySettings.MinPasswordLength = 8;
            viewModel.SecuritySettings.RequireSpecialChars = true;
            viewModel.SecuritySettings.RequireNumbers = true;
            viewModel.SecuritySettings.RequireUppercase = true;
            viewModel.SecuritySettings.SessionTimeoutMinutes = 30;
            viewModel.SecuritySettings.MaxLoginAttempts = 5;
            viewModel.SecuritySettings.EnableTwoFactor = false;
            viewModel.SecuritySettings.AccountLockoutMinutes = 15;
            viewModel.SecuritySettings.LogSecurityEvents = true;
            viewModel.SecuritySettings.PasswordChangeDays = 90;

            // Notification Settings
            viewModel.NotificationSettings.EnableEmailNotifications = true;
            viewModel.NotificationSettings.EnablePushNotifications = true;
            viewModel.NotificationSettings.NotifyNewUserRegistration = true;
            viewModel.NotificationSettings.NotifyPasswordReset = true;
            viewModel.NotificationSettings.NotifyAccountLocked = true;
            viewModel.NotificationSettings.NotifySystemMaintenance = true;
            viewModel.NotificationSettings.NotifySecurityAlerts = true;

            // System Configuration
            viewModel.SystemConfiguration.SiteName = "MomExchange";
            viewModel.SystemConfiguration.SiteDescription = "Nền tảng trao đổi và chia sẻ cho các bà mẹ";
            viewModel.SystemConfiguration.SiteUrl = "https://momexchange.com";
            viewModel.SystemConfiguration.ContactEmail = "contact@momexchange.com";
            viewModel.SystemConfiguration.MaintenanceMode = false;
            viewModel.SystemConfiguration.MaxFileUploadSizeMB = 10;
            viewModel.SystemConfiguration.MaxFileUploadCount = 5;
            viewModel.SystemConfiguration.EnableCaching = true;
            viewModel.SystemConfiguration.CacheExpirationMinutes = 60;
            viewModel.SystemConfiguration.ApiRateLimitPerMinute = 100;
            viewModel.SystemConfiguration.AllowedFileExtensions = "jpg,jpeg,png,gif,pdf,doc,docx";

            // Backup Settings
            viewModel.BackupSettings.EnableAutoBackup = true;
            viewModel.BackupSettings.BackupFrequencyDays = 7;
            viewModel.BackupSettings.KeepBackupCount = 5;
            viewModel.BackupSettings.BackupLocation = "/backups/";
            viewModel.BackupSettings.CompressBackup = true;
            viewModel.BackupSettings.IncludeFiles = true;
            viewModel.BackupSettings.IncludeDatabase = true;
            viewModel.BackupSettings.EnableEmailNotification = true;

            // Monitoring Settings
            viewModel.MonitoringSettings.EnableSystemMonitoring = true;
            viewModel.MonitoringSettings.EnableErrorTracking = true;
            viewModel.MonitoringSettings.EnablePerformanceMonitoring = true;
            viewModel.MonitoringSettings.EnableUserActivityLogging = true;
            viewModel.MonitoringSettings.LogRetentionDays = 30;
            viewModel.MonitoringSettings.MaxLogEntries = 1000;
            viewModel.MonitoringSettings.EnableRealTimeLogging = true;
            viewModel.MonitoringSettings.LogLevel = "Info";
            viewModel.MonitoringSettings.EnableEmailAlerts = true;
            viewModel.MonitoringSettings.DiskSpaceThresholdPercent = 80;
            viewModel.MonitoringSettings.MemoryThresholdPercent = 85;
            viewModel.MonitoringSettings.CPUThresholdPercent = 90;
        }

        private List<string> GetModelErrors()
        {
            var errors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
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

        // Posts Management Helper Methods
        private List<PostItem> GetAllPosts()
        {
            var posts = new List<PostItem>();
            
            // Add sample Community Posts
            posts.AddRange(GetSampleCommunityPosts());
            
            // Add sample Trade Posts
            posts.AddRange(GetSampleTradePosts());
            
            // Add sample Social Posts
            posts.AddRange(GetSampleSocialPosts());
            
            // Add sample Milk Posts
            posts.AddRange(GetSampleMilkPosts());
            
            return posts;
        }

        private List<PostItem> GetSampleCommunityPosts()
        {
            return new List<PostItem>
            {
                new PostItem
                {
                    Id = 1,
                    Type = "Community",
                    Title = "Kinh nghiệm chăm sóc trẻ sơ sinh",
                    AuthorName = "Mẹ Bối Bối",
                    AuthorEmail = "meboi@example.com",
                    Content = "Chia sẻ kinh nghiệm chăm sóc trẻ sơ sinh từ những ngày đầu...",
                    Excerpt = "Chia sẻ kinh nghiệm chăm sóc trẻ sơ sinh từ những ngày đầu...",
                    CreatedAt = DateTime.Now.AddDays(-5),
                    Status = "Active",
                    LikeCount = 25,
                    CommentCount = 8,
                    ViewCount = 150,
                    ImageUrl = "/images/community1.jpg",
                    Tags = new List<string> { "chăm sóc", "trẻ sơ sinh", "kinh nghiệm" },
                    Location = "Hà Nội"
                },
                new PostItem
                {
                    Id = 2,
                    Type = "Community",
                    Title = "Cách chọn sữa công thức phù hợp",
                    AuthorName = "Mẹ Minh Anh",
                    AuthorEmail = "meminhanh@example.com",
                    Content = "Hướng dẫn chi tiết cách chọn sữa công thức phù hợp với từng độ tuổi...",
                    Excerpt = "Hướng dẫn chi tiết cách chọn sữa công thức phù hợp...",
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Status = "Pending",
                    LikeCount = 12,
                    CommentCount = 5,
                    ViewCount = 80,
                    ImageUrl = "/images/community2.jpg",
                    Tags = new List<string> { "sữa công thức", "dinh dưỡng" },
                    Location = "TP.HCM"
                }
            };
        }

        private List<PostItem> GetSampleTradePosts()
        {
            return new List<PostItem>
            {
                new PostItem
                {
                    Id = 3,
                    Type = "Trade",
                    Title = "Trao đổi quần áo trẻ em",
                    AuthorName = "Mẹ Lan Anh",
                    AuthorEmail = "melananh@example.com",
                    Content = "Mình có nhiều quần áo trẻ em size 6-12 tháng muốn trao đổi...",
                    Excerpt = "Mình có nhiều quần áo trẻ em size 6-12 tháng muốn trao đổi...",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Status = "Active",
                    LikeCount = 8,
                    CommentCount = 3,
                    ViewCount = 45,
                    ImageUrl = "/images/trade1.jpg",
                    Tags = new List<string> { "trao đổi", "quần áo", "trẻ em" },
                    Location = "Đà Nẵng"
                }
            };
        }

        private List<PostItem> GetSampleSocialPosts()
        {
            return new List<PostItem>
            {
                new PostItem
                {
                    Id = 4,
                    Type = "Social",
                    Title = "Chia sẻ khoảnh khắc đáng yêu",
                    AuthorName = "Mẹ Hương",
                    AuthorEmail = "mehuong@example.com",
                    Content = "Khoảnh khắc bé yêu tập đi đầu tiên...",
                    Excerpt = "Khoảnh khắc bé yêu tập đi đầu tiên...",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Status = "Active",
                    LikeCount = 35,
                    CommentCount = 12,
                    ViewCount = 200,
                    ImageUrl = "/images/social1.jpg",
                    Tags = new List<string> { "khoảnh khắc", "bé yêu" },
                    Location = "Hải Phòng"
                }
            };
        }

        private List<PostItem> GetSampleMilkPosts()
        {
            return new List<PostItem>
            {
                new PostItem
                {
                    Id = 5,
                    Type = "Milk",
                    Title = "Hiến tặng sữa mẹ",
                    AuthorName = "Mẹ Thu Trang",
                    AuthorEmail = "methutrang@example.com",
                    Content = "Mình có sữa mẹ dư thừa muốn hiến tặng cho các bé cần...",
                    Excerpt = "Mình có sữa mẹ dư thừa muốn hiến tặng cho các bé cần...",
                    CreatedAt = DateTime.Now.AddDays(-4),
                    Status = "Pending",
                    LikeCount = 15,
                    CommentCount = 6,
                    ViewCount = 90,
                    ImageUrl = "/images/milk1.jpg",
                    Tags = new List<string> { "hiến tặng", "sữa mẹ" },
                    Location = "Cần Thơ",
                    IsHealthVerified = true
                }
            };
        }

        private List<PostItem> ApplyPostFilters(List<PostItem> posts, string search, string typeFilter, 
            string statusFilter, string authorSearch, string titleSearch, string contentSearch,
            DateTime? createdFrom, DateTime? createdTo, bool caseSensitive, bool exactMatch)
        {
            // Basic search
            if (!string.IsNullOrEmpty(search))
            {
                posts = posts.Where(p => 
                    ContainsText(p.Title, search, caseSensitive, exactMatch) || 
                    ContainsText(p.AuthorName, search, caseSensitive, exactMatch) ||
                    ContainsText(p.Content, search, caseSensitive, exactMatch)
                ).ToList();
            }

            // Type filter
            if (!string.IsNullOrEmpty(typeFilter) && typeFilter != "all")
            {
                posts = posts.Where(p => p.Type == typeFilter).ToList();
            }

            // Status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                posts = posts.Where(p => p.Status == statusFilter).ToList();
            }

            // Advanced search filters
            if (!string.IsNullOrEmpty(authorSearch))
            {
                posts = posts.Where(p => ContainsText(p.AuthorName, authorSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(titleSearch))
            {
                posts = posts.Where(p => ContainsText(p.Title, titleSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(contentSearch))
            {
                posts = posts.Where(p => ContainsText(p.Content, contentSearch, caseSensitive, exactMatch)).ToList();
            }

            // Date range filter
            if (createdFrom.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt >= createdFrom.Value).ToList();
            }

            if (createdTo.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt <= createdTo.Value.AddDays(1).AddTicks(-1)).ToList();
            }

            return posts;
        }

        private List<PostItem> ApplyPostSorting(List<PostItem> posts, string sortBy, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "created";
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "desc";
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            switch (sortBy.ToLower())
            {
                case "title":
                    return isAscending 
                        ? posts.OrderBy(p => p.Title).ToList()
                        : posts.OrderByDescending(p => p.Title).ToList();
                
                case "author":
                    return isAscending 
                        ? posts.OrderBy(p => p.AuthorName).ToList()
                        : posts.OrderByDescending(p => p.AuthorName).ToList();
                
                case "type":
                    return isAscending 
                        ? posts.OrderBy(p => p.Type).ToList()
                        : posts.OrderByDescending(p => p.Type).ToList();
                
                case "status":
                    return isAscending 
                        ? posts.OrderBy(p => p.Status).ToList()
                        : posts.OrderByDescending(p => p.Status).ToList();
                
                case "created":
                default:
                    return isAscending 
                        ? posts.OrderBy(p => p.CreatedAt).ToList()
                        : posts.OrderByDescending(p => p.CreatedAt).ToList();
            }
        }

        private PostItem GetPostById(int id, string type)
        {
            var allPosts = GetAllPosts();
            return allPosts.FirstOrDefault(p => p.Id == id && p.Type == type);
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

        // Helper methods for advanced search
        private List<User> ApplyAdvancedSearchFilters(List<User> users, string search, string roleFilter,
            string emailSearch, string usernameSearch, string fullNameSearch, string phoneSearch,
            string addressSearch, string statusFilter, DateTime? createdFrom, DateTime? createdTo,
            bool caseSensitive, bool exactMatch)
        {
            // Basic search (backward compatibility)
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => 
                    ContainsText(u.Email, search, caseSensitive, exactMatch) || 
                    ContainsText(u.UserName, search, caseSensitive, exactMatch) ||
                    (u.UserDetails != null && ContainsText(u.UserDetails.FullName, search, caseSensitive, exactMatch))
                ).ToList();
            }

            // Role filter
            if (!string.IsNullOrEmpty(roleFilter) && int.TryParse(roleFilter, out int role))
            {
                users = users.Where(u => u.Role == role).ToList();
            }

            // Advanced search filters
            if (!string.IsNullOrEmpty(emailSearch))
            {
                users = users.Where(u => ContainsText(u.Email, emailSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(usernameSearch))
            {
                users = users.Where(u => ContainsText(u.UserName, usernameSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(fullNameSearch))
            {
                users = users.Where(u => u.UserDetails != null && 
                    ContainsText(u.UserDetails.FullName, fullNameSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(phoneSearch))
            {
                users = users.Where(u => ContainsText(u.PhoneNumber, phoneSearch, caseSensitive, exactMatch)).ToList();
            }

            if (!string.IsNullOrEmpty(addressSearch))
            {
                users = users.Where(u => u.UserDetails != null && 
                    ContainsText(u.UserDetails.Address, addressSearch, caseSensitive, exactMatch)).ToList();
            }

            // Status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                bool isActive = statusFilter == "active";
                users = users.Where(u => u.IsActive == isActive).ToList();
            }

            // Date range filter
            if (createdFrom.HasValue)
            {
                users = users.Where(u => u.CreatedAt >= createdFrom.Value).ToList();
            }

            if (createdTo.HasValue)
            {
                users = users.Where(u => u.CreatedAt <= createdTo.Value.AddDays(1).AddTicks(-1)).ToList();
            }

            return users;
        }

        private List<User> ApplySorting(List<User> users, string sortBy, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "created";
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "desc";
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            switch (sortBy.ToLower())
            {
                case "name":
                    return isAscending 
                        ? users.OrderBy(u => u.UserDetails?.FullName ?? "").ToList()
                        : users.OrderByDescending(u => u.UserDetails?.FullName ?? "").ToList();
                
                case "email":
                    return isAscending 
                        ? users.OrderBy(u => u.Email).ToList()
                        : users.OrderByDescending(u => u.Email).ToList();
                
                case "role":
                    return isAscending 
                        ? users.OrderBy(u => u.Role).ToList()
                        : users.OrderByDescending(u => u.Role).ToList();
                
                case "status":
                    return isAscending 
                        ? users.OrderBy(u => u.IsActive).ToList()
                        : users.OrderByDescending(u => u.IsActive).ToList();
                
                case "created":
                default:
                    return isAscending 
                        ? users.OrderBy(u => u.CreatedAt).ToList()
                        : users.OrderByDescending(u => u.CreatedAt).ToList();
            }
        }

        private bool ContainsText(string text, string searchTerm, bool caseSensitive, bool exactMatch)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchTerm))
                return false;

            if (exactMatch)
            {
                return caseSensitive 
                    ? text.Equals(searchTerm, StringComparison.Ordinal)
                    : text.Equals(searchTerm, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return caseSensitive 
                    ? text.Contains(searchTerm)
                    : text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        // GET: Admin/CreateUser
        public ActionResult CreateUser()
        {
            try
            {
                var viewModel = new B_M.Models.AdminCreateUserViewModel
                {
                    Role = 2, // Default to Mom
                    IsActive = true,
                    SendEmailNotification = false,
                    GenerateRandomPassword = false
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải trang tạo người dùng: " + ex.Message;
                return RedirectToAction("Users");
            }
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(B_M.Models.AdminCreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Check for duplicate email
                if (userRepository.EmailExists(model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                // Check for duplicate username (if provided)
                if (!string.IsNullOrEmpty(model.UserName) && userRepository.UsernameExists(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập này đã được sử dụng.");
                    return View(model);
                }

                // Generate password if requested
                string password = model.Password;
                if (model.GenerateRandomPassword)
                {
                    password = B_M.Helpers.PasswordGenerator.GeneratePassword();
                    model.GeneratedPassword = password;
                }

                // Create user
                var user = new User
                {
                    Email = model.Email,
                    UserName = string.IsNullOrEmpty(model.UserName) ? null : model.UserName,
                    PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? null : model.PhoneNumber,
                    PasswordHash = PasswordHelper.HashPassword(password),
                    Role = model.Role,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now
                };

                // Create user details
                var userDetails = new UserDetails
                {
                    FullName = model.FullName,
                    Address = string.IsNullOrEmpty(model.Address) ? null : model.Address,
                    ReputationScore = 0
                };

                // Save to database
                bool success = userRepository.CreateUser(user, userDetails);

                if (success)
                {
                    TempData["SuccessMessage"] = $"Đã tạo tài khoản thành công cho {model.FullName} ({model.Email})";
                    
                    if (model.GenerateRandomPassword)
                    {
                        TempData["GeneratedPassword"] = password;
                        TempData["ShowPassword"] = true;
                    }

                    return RedirectToAction("Users");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo tài khoản. Vui lòng thử lại.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(model);
            }
        }

        // GET: Admin/ImportUsers
        public ActionResult ImportUsers()
        {
            try
            {
                var viewModel = new B_M.Models.AdminImportUsersViewModel
                {
                    DefaultRole = 2, // Default to Mom
                    IsActive = true,
                    SendEmailNotification = false,
                    GenerateRandomPassword = true,
                    SkipDuplicateEmails = true,
                    SkipDuplicateUsernames = true
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải trang import người dùng: " + ex.Message;
                return RedirectToAction("Users");
            }
        }

        // POST: Admin/ImportUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportUsers(B_M.Models.AdminImportUsersViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.ExcelFile == null || model.ExcelFile.ContentLength == 0)
                {
                    ModelState.AddModelError("ExcelFile", "Vui lòng chọn file Excel.");
                    return View(model);
                }

                // Validate file type
                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var fileExtension = System.IO.Path.GetExtension(model.ExcelFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ExcelFile", "Chỉ chấp nhận file Excel (.xlsx, .xls).");
                    return View(model);
                }

                // Validate file size (max 10MB)
                if (model.ExcelFile.ContentLength > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("ExcelFile", "File quá lớn. Kích thước tối đa là 10MB.");
                    return View(model);
                }

                // Process Excel file
                var result = B_M.Helpers.ExcelHelper.ProcessExcelFile(model.ExcelFile, model, userRepository);

                // Store result in TempData for display
                TempData["ImportResult"] = result;

                if (result.SuccessCount > 0)
                {
                    TempData["SuccessMessage"] = $"Import thành công {result.SuccessCount} người dùng.";
                }

                if (result.ErrorCount > 0)
                {
                    TempData["ErrorMessage"] = $"Có {result.ErrorCount} lỗi trong quá trình import.";
                }

                return RedirectToAction("ImportResult");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi import file: " + ex.Message);
                return View(model);
            }
        }

        // GET: Admin/ImportResult
        public ActionResult ImportResult()
        {
            try
            {
                var result = TempData["ImportResult"] as B_M.Models.AdminImportResultViewModel;
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Không có kết quả import để hiển thị.";
                    return RedirectToAction("ImportUsers");
                }

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi hiển thị kết quả import: " + ex.Message;
                return RedirectToAction("Users");
            }
        }

        // POST: Admin/ApprovePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovePost(int postId, string postType)
        {
            try
            {
                // TODO: Implement actual post approval logic
                // For now, simulate approval
                return Json(new { 
                    success = true, 
                    message = "Đã duyệt bài viết thành công.",
                    data = new {
                        postId = postId,
                        status = "Active",
                        statusClass = "badge-success"
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/RejectPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectPost(int postId, string postType, string reason)
        {
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do từ chối." });
                }

                // TODO: Implement actual post rejection logic
                // For now, simulate rejection
                return Json(new { 
                    success = true, 
                    message = "Đã từ chối bài viết thành công.",
                    data = new {
                        postId = postId,
                        status = "Rejected",
                        statusClass = "badge-danger",
                        rejectionReason = reason
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Admin/DeletePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int postId, string postType)
        {
            try
            {
                // TODO: Implement actual post deletion logic
                // For now, simulate deletion
                return Json(new { 
                    success = true, 
                    message = "Đã xóa bài viết thành công."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Admin/PostDetails
        public ActionResult PostDetails(int id, string type)
        {
            try
            {
                // TODO: Implement post details view
                // For now, return a simple view
                var post = GetPostById(id, type);
                if (post == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bài viết.";
                    return RedirectToAction("Posts");
                }

                return View(post);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải chi tiết bài viết: " + ex.Message;
                return RedirectToAction("Posts");
            }
        }

        // GET: Admin/DownloadTemplate
        public ActionResult DownloadTemplate()
        {
            try
            {
                var templateBytes = B_M.Helpers.ExcelHelper.CreateExcelTemplate();
                var fileName = $"UserImportTemplate_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo template: " + ex.Message;
                return RedirectToAction("ImportUsers");
            }
        }
    }
}
