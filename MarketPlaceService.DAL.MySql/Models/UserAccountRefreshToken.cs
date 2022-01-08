using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class UserAccountRefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public Guid? UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
