namespace PANOS
{
    public class ObjectNotFoundError
    {
        public FirewallObject RequestedObject { get; set; }

        public string RequestedName { get; set; }

        public ObjectNotFoundError(FirewallObject firewallObject)
        {
            RequestedObject = firewallObject;
        }

        public ObjectNotFoundError(string name)
        {
            RequestedName = name;
        }

        public override string ToString()
        {
            return 
                RequestedObject != null ? 
                    string.Format("Attempt to locate the following object {0} failed", RequestedObject) :
                    string.Format("Attempt to locate {0} failed", RequestedName);
        }
    }
}
