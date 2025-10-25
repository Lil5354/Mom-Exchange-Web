using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OfficeOpenXml;
using B_M.Models;

namespace B_M.Helpers
{
    public static class ExcelHelper
    {
        public static B_M.Models.AdminImportResultViewModel ProcessExcelFile(HttpPostedFileBase file, B_M.Models.AdminImportUsersViewModel model, UserRepository userRepository)
        {
            var result = new B_M.Models.AdminImportResultViewModel
            {
                FileName = file.FileName,
                ImportTime = DateTime.Now
            };

            try
            {
                // Set EPPlus license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        result.Errors.Add(new B_M.Models.ImportUserError
                        {
                            RowNumber = 0,
                            ErrorMessage = "File Excel không có worksheet nào"
                        });
                        return result;
                    }

                    var rowCount = worksheet.Dimension?.Rows ?? 0;
                    if (rowCount < 2) // At least header + 1 data row
                    {
                        result.Errors.Add(new B_M.Models.ImportUserError
                        {
                            RowNumber = 0,
                            ErrorMessage = "File Excel không có dữ liệu"
                        });
                        return result;
                    }

                    result.TotalRows = rowCount - 1; // Exclude header

                    // Process each row
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var userData = ExtractUserDataFromRow(worksheet, row);
                            var validationResult = ValidateUserData(userData, userRepository, model);

                            if (validationResult.IsValid)
                            {
                                var user = CreateUserFromData(userData, model);
                                var userDetails = CreateUserDetailsFromData(userData);

                                if (userRepository.CreateUser(user, userDetails))
                                {
                                    result.SuccessCount++;
                                    result.SuccessUsers.Add(new B_M.Models.ImportUserSuccess
                                    {
                                        RowNumber = row,
                                        Email = user.Email,
                                        FullName = userDetails.FullName,
                                        UserName = user.UserName,
                                        Role = user.Role,
                                        RoleName = GetRoleName(user.Role),
                                        GeneratedPassword = model.GenerateRandomPassword ? userData.GeneratedPassword : null
                                    });
                                }
                                else
                                {
                                    result.ErrorCount++;
                                    result.Errors.Add(new B_M.Models.ImportUserError
                                    {
                                        RowNumber = row,
                                        Email = userData.Email,
                                        ErrorMessage = "Không thể tạo tài khoản trong database"
                                    });
                                }
                            }
                            else
                            {
                                result.ErrorCount++;
                                result.Errors.AddRange(validationResult.Errors.Select(e => new B_M.Models.ImportUserError
                                {
                                    RowNumber = row,
                                    Email = userData.Email,
                                    ErrorMessage = e,
                                    FieldName = "Validation"
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            result.ErrorCount++;
                            result.Errors.Add(new B_M.Models.ImportUserError
                            {
                                RowNumber = row,
                                ErrorMessage = $"Lỗi xử lý dòng: {ex.Message}"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new B_M.Models.ImportUserError
                {
                    RowNumber = 0,
                    ErrorMessage = $"Lỗi đọc file Excel: {ex.Message}"
                });
            }

            return result;
        }

        private static ExcelUserData ExtractUserDataFromRow(ExcelWorksheet worksheet, int row)
        {
            return new ExcelUserData
            {
                Email = GetCellValue(worksheet, row, 1), // Column A
                UserName = GetCellValue(worksheet, row, 2), // Column B
                FullName = GetCellValue(worksheet, row, 3), // Column C
                PhoneNumber = GetCellValue(worksheet, row, 4), // Column D
                Address = GetCellValue(worksheet, row, 5), // Column E
                Role = GetCellValue(worksheet, row, 6), // Column F
                GeneratedPassword = PasswordGenerator.GeneratePassword()
            };
        }

        private static string GetCellValue(ExcelWorksheet worksheet, int row, int col)
        {
            var cellValue = worksheet.Cells[row, col].Value;
            return cellValue?.ToString()?.Trim() ?? string.Empty;
        }

        private static ValidationResult ValidateUserData(ExcelUserData userData, UserRepository userRepository, B_M.Models.AdminImportUsersViewModel model)
        {
            var result = new ValidationResult();

            // Validate email
            if (string.IsNullOrEmpty(userData.Email))
            {
                result.Errors.Add("Email là bắt buộc");
            }
            else if (!IsValidEmail(userData.Email))
            {
                result.Errors.Add("Email không hợp lệ");
            }
            else if (userRepository.EmailExists(userData.Email))
            {
                if (!model.SkipDuplicateEmails)
                {
                    result.Errors.Add("Email đã tồn tại");
                }
                else
                {
                    result.IsSkipped = true;
                    return result;
                }
            }

            // Validate username (optional)
            if (!string.IsNullOrEmpty(userData.UserName))
            {
                if (userRepository.UsernameExists(userData.UserName))
                {
                    if (!model.SkipDuplicateUsernames)
                    {
                        result.Errors.Add("Tên đăng nhập đã tồn tại");
                    }
                    else
                    {
                        userData.UserName = null; // Clear username if duplicate
                    }
                }
            }

            // Validate full name
            if (string.IsNullOrEmpty(userData.FullName))
            {
                result.Errors.Add("Họ và tên là bắt buộc");
            }

            // Validate phone number
            if (!string.IsNullOrEmpty(userData.PhoneNumber) && !IsValidPhoneNumber(userData.PhoneNumber))
            {
                result.Errors.Add("Số điện thoại không hợp lệ");
            }

            // Validate role
            if (!string.IsNullOrEmpty(userData.Role))
            {
                if (!byte.TryParse(userData.Role, out byte role) || role < 1 || role > 3)
                {
                    result.Errors.Add("Vai trò không hợp lệ (1=Admin, 2=Mom, 3=Brand)");
                }
                else
                {
                    userData.ParsedRole = role;
                }
            }
            else
            {
                userData.ParsedRole = model.DefaultRole;
            }

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        private static User CreateUserFromData(ExcelUserData userData, B_M.Models.AdminImportUsersViewModel model)
        {
            return new User
            {
                Email = userData.Email,
                UserName = string.IsNullOrEmpty(userData.UserName) ? null : userData.UserName,
                PhoneNumber = string.IsNullOrEmpty(userData.PhoneNumber) ? null : userData.PhoneNumber,
                PasswordHash = PasswordHelper.HashPassword(userData.GeneratedPassword),
                Role = userData.ParsedRole,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now
            };
        }

        private static UserDetails CreateUserDetailsFromData(ExcelUserData userData)
        {
            return new UserDetails
            {
                FullName = userData.FullName,
                Address = string.IsNullOrEmpty(userData.Address) ? null : userData.Address,
                ReputationScore = 0
            };
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^[0-9+\-\s()]*$");
        }

        private static string GetRoleName(byte role)
        {
            switch (role)
            {
                case 1: return "Quản trị viên";
                case 2: return "Mẹ bỉm";
                case 3: return "Nhãn hàng";
                default: return "Không xác định";
            }
        }

        public static byte[] CreateExcelTemplate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                // Headers
                worksheet.Cells[1, 1].Value = "Email";
                worksheet.Cells[1, 2].Value = "UserName";
                worksheet.Cells[1, 3].Value = "FullName";
                worksheet.Cells[1, 4].Value = "PhoneNumber";
                worksheet.Cells[1, 5].Value = "Address";
                worksheet.Cells[1, 6].Value = "Role";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                }

                // Add sample data
                worksheet.Cells[2, 1].Value = "admin@example.com";
                worksheet.Cells[2, 2].Value = "admin";
                worksheet.Cells[2, 3].Value = "Nguyễn Văn Admin";
                worksheet.Cells[2, 4].Value = "0123456789";
                worksheet.Cells[2, 5].Value = "Hà Nội";
                worksheet.Cells[2, 6].Value = "1";

                worksheet.Cells[3, 1].Value = "mom@example.com";
                worksheet.Cells[3, 2].Value = "mom_user";
                worksheet.Cells[3, 3].Value = "Trần Thị Mẹ";
                worksheet.Cells[3, 4].Value = "0987654321";
                worksheet.Cells[3, 5].Value = "TP.HCM";
                worksheet.Cells[3, 6].Value = "2";

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }

    public class ExcelUserData
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public byte ParsedRole { get; set; }
        public string GeneratedPassword { get; set; }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public bool IsSkipped { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
