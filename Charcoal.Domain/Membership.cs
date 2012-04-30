namespace Charcoal.Core {
    public class Membership : BaseEntity {
        public Person Person { get; set; }
        public string Role { get; set; }
        //  <memberships type="array">
        //    <membership>
        //      <id>1006</id>
        //      <person>
        //        <email>kirkybaby@earth.ufp</email>
        //        <name>James T. Kirk</name>
        //        <initials>JTK</initials>
        //      </person>
        //      <role>Owner</role>
        //    </membership>
        //  </memberships>
    }
}
