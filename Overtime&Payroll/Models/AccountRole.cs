﻿using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    [Table("tb_tr_account_roles")]
    public class AccountRole : BaseEntity
    {
        [Column("account_guid")]
        public Guid AccountGuid { get; set; }

        [Column("role_guid")]
        public Guid RoleGuid { get; set; }

        // Kardinalitas AccountRole
        public Role? Role { get; set; }
        public Account? Account { get; set; }


    }
}
