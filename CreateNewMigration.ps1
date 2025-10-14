# Script để tạo migration mới và update database
# Chạy trong Package Manager Console của Visual Studio

Write-Host "=== CREATING NEW MIGRATION ===" -ForegroundColor Green

# Xóa migration cũ nếu có vấn đề
Write-Host "Removing old migration..." -ForegroundColor Yellow
Remove-Migration -Force

Write-Host "`nCreating new migration..." -ForegroundColor Yellow
Add-Migration InitialCreateWithUserDetails

Write-Host "`nUpdating database..." -ForegroundColor Yellow
Update-Database -Verbose

Write-Host "`n=== MIGRATION COMPLETED ===" -ForegroundColor Green
Write-Host "Database should now have correct structure!" -ForegroundColor Cyan
