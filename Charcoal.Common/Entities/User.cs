using System.Collections.Generic;

namespace Charcoal.Common.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string APIKey { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Privilege Privileges { get; set; }
        public List<Project> Projects { get; set; }
    }
}