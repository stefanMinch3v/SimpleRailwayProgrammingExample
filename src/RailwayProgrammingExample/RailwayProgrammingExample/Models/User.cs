namespace RailwayProgrammingExample.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Membership Membership { get; set; }

        public bool CanChangeMembership => this.Membership != Membership.Enterprise;

        public override string ToString()
        {
            return $"User: \n--Id:{this.Id}\n--Name:{this.Name}\n--Age:{this.Age}\n--Membership:{this.Membership}";
        }
    }
    
    public enum Membership
    {
        Basic = 0,
        Extended = 1,
        Gold = 2,
        Diamond = 3,
        Enterprise = 4
    }
}
