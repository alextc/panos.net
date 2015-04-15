namespace PANOS
{
    using System.Text;

    public abstract class FirewallObject
    {
        protected FirewallObject(string name, string schemName, string description)
        {
            Name = name;
            SchemaName = schemName;
            Description = description;
        }

        public string Name { get; set; }

        public string SchemaName { get; set; }

        public string Description { get; set; }

        public abstract string ToXml();

        // Only required for Ps Unit Tests
        public abstract string ToPsScript();

        // Only required for Ps Unit Tests
        // The objective of this method is to change the state of the object to the point that
        // the equals method would fail on the versions of the object prior and after the call to Mutate
        public abstract void Mutate();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Name:{0}, Description:{1}", Name, Description);
            return sb.ToString();
        }
    }
}
