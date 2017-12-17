using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGamesData.StorageDB
{
   public class StorageUserRegustration
    {

        public Guid ID { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string FamilyName { get; set; }

        public int UserID { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
