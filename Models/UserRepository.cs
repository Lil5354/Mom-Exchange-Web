// File: Models/UserRepository.cs
using System;
using System.Linq;
using System.Data.Entity;

namespace MomExchange.Models
{
    public class UserRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UserRepository()
        {
            _context = new ApplicationDbContext();
        }

        public bool CreateUser(User user, UserDetails userDetails)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges(); 

                    userDetails.UserID = user.UserID;

                    _context.UserDetails.Add(userDetails);
                    _context.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine($"ERROR in CreateUser: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                        System.Diagnostics.Debug.WriteLine($"INNER STACK TRACE: {ex.InnerException.StackTrace}");
                    }
                    
                    return false;
                }
            }
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UsernameExists(string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            return _context.Users.Any(u => u.UserName == username);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users
                .Include("UserDetails")
                .FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users
                .Include("UserDetails")
                .FirstOrDefault(u => u.UserName == username);
        }

        public User GetUserByEmailOrUsername(string emailOrUsername)
        {
            return _context.Users
                .Include("UserDetails")
                .FirstOrDefault(u => u.Email == emailOrUsername || u.UserName == emailOrUsername);
        }

        public UserDetails GetUserDetails(int userId)
        {
            return _context.UserDetails.Find(userId);
        }

        public bool UpdateUser(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in UpdateUser: {ex.Message}");
                return false;
            }
        }

        public bool UpdateUserDetails(UserDetails userDetails)
        {
            try
            {
                _context.Entry(userDetails).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in UpdateUserDetails: {ex.Message}");
                return false;
            }
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
