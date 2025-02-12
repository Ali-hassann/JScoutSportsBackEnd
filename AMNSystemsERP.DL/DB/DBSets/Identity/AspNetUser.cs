﻿using AMNSystemsERP.DL.DB.Base;
using System.ComponentModel.DataAnnotations;

namespace AMNSystemsERP.DL.DB.DBSets.Identity
{
    public partial class AspNetUser : Entity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string NormalizedUserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NormalizedEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string SecurityStamp { get; set; } = string.Empty;
        public string ConcurrencyStamp { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationRoleId { get; set; }
        public int AccessFailedCount { get; set; }
        public long OrganizationId { get; set; }
        public long OutletId { get; set; }
    }
}