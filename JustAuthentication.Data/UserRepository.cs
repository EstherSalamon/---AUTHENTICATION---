using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JustAuthentication.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connection)
        {
            _connectionString = connection;
        }

        public void AddUser(User user, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            user.Password = hashedPassword;
            using UsersDataContext context = new UsersDataContext(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User LogIn(string email, string password)
        {
            User user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if(!isValidPassword)
            {
                return null;
            }

            return user;
        }

        public User GetByEmail(string email)
        {
            using UsersDataContext context = new UsersDataContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailExists(string email)
        {
            using UsersDataContext context = new UsersDataContext(_connectionString);
            return context.Users.Any(u => u.Email == email);
        }
    }
}
