namespace Charcoal.Common.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        //TODO: add to the database
        public int Velocity { get; set; }
        //TODO: uncomment when needed 
        //public List<User> Users { get; set; }
    }
}