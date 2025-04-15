using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;

namespace BlazorApp.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        public event Action OnAuthStateChanged;

        public AuthService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Session?> SignUp(string email, string password)
        {
            try
            {
                // Check if the email already exists
                var userExists = await DoesUserExist(email);
                if (userExists)
                {
                    throw new Exception("An account with this email already exists.");
                }

                // If the email does not exist, proceed with signup
                var session = await _supabase.Auth.SignUp(email, password);
                OnAuthStateChanged?.Invoke();
                return session;
            }
            catch (GotrueException ex)
            {
                if (ex.Message.Contains("Password does not match"))
                {
                    throw new Exception("Incorrect password for this email.");
                }
                throw new Exception("Sign up failed. Please try again.");
            }
        }

        public async Task<Session?> SignIn(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(email, password);
                OnAuthStateChanged?.Invoke();
                return session;
            }
            catch (GotrueException ex)
            {
                if (ex.Message.Contains("Invalid login credentials"))
                {
                    throw new Exception("Incorrect MailID or password. Please try again.");
                }
                if (ex.Message.Contains("Email not confirmed"))
                {
                    throw new Exception("No account found with this email. Please sign up first.");
                }
                throw new Exception("Login failed. Please try again.");
            }
        }

        public async Task SignOut()
        {
            await _supabase.Auth.SignOut();
            OnAuthStateChanged?.Invoke(); // Notify subscribers
        }

        public async Task<User?> GetUser()
        {
            var user = _supabase.Auth.CurrentUser;
            return user;
        }
        public async Task<bool> DoesUserExist(string email)
        {
            try
            {
                // Attempt to fetch the user by email
                var user = await _supabase.Auth.GetUser(email);
                return user != null;
            }
            catch (GotrueException ex)
            {
                // If the user does not exist, Gotrue will throw an exception
                return false;
            }
        }
    }
}