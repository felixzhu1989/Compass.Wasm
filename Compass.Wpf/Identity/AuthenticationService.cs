using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Compass.Wasm.Shared.IdentityService;

namespace Compass.Wpf.Identity;
/* https://social.technet.microsoft.com/wiki/contents/articles/25726.wpf-implementing-custom-authentication-and-authorization.aspx
 * 
 */


public interface IAuthenticationService
{
    UserDto AuthenticateUser(string username, string password);
}
public class AuthenticationService : IAuthenticationService
{
    #region 构造测试数据
    private class InternalUserData
    {
        public InternalUserData(string username, string email, string hashedPassword, string roles)
        {
            Username = username;
            Email = email;
            HashedPassword = hashedPassword;
            Roles = roles;
        }
        public string Username { get; }
        public string Email { get; }
        public string HashedPassword { get; }
        public string Roles { get; }
    }

    private static readonly List<InternalUserData> _users = new()
    {
        new InternalUserData("Mark", "mark@company.com",
            "tUenK3/F1YclPPKnnSCFiVdI13grxLiXNrlnspQ1UfA=",   "admin"),
        new InternalUserData("John", "john@company.com",
            "hxcp70rN66koIHbUeSkf9226QRsmv2L/nUQIhKD/YFA=", string.Empty )
    };
    #endregion

    public UserDto AuthenticateUser(string username, string clearTextPassword)
    {
        InternalUserData userData = _users.FirstOrDefault(u => u.Username.Equals(username) && u.HashedPassword.Equals(CalculateHash(clearTextPassword, u.Username)));
        if (userData == null)
            throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");
        return new UserDto { UserName = userData.Username, Email = userData.Email, Roles = userData.Roles };
    }

    private string CalculateHash(string clearTextPassword, string salt)
    {
        // Convert the salted password to a byte array
        byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
        // Use the hash algorithm to calculate the hash
        HashAlgorithm algorithm = new SHA256Managed();
        byte[] hash = algorithm.ComputeHash(saltedHashBytes);
        // Return the hash as a base64 encoded string to be compared to the stored password
        return Convert.ToBase64String(hash);
    }
}