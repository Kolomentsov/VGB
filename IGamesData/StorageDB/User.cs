using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGamesData.StorageDB
{
    public class User
    {
        [Key()]
        public Guid ID { get; set; }

        [Required()]
        [Index("IX_UserID", IsUnique = true)]
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string FamilyName { get; set; }

        [Required()]
        public string EMail { get; set; }

        public string PhoneNumber { get; set; }
    }
}
