using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class UserAccountRole
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Guid? Key { get; set; }
        public DateTime Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
