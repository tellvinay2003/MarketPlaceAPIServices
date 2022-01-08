using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            UserAccountRefreshToken = new HashSet<UserAccountRefreshToken>();
            UserAccountRole = new HashSet<UserAccountRole>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<UserAccountRefreshToken> UserAccountRefreshToken { get; set; }
        public virtual ICollection<UserAccountRole> UserAccountRole { get; set; }
    }
}
