
namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    public class SearchesObjects : RequiresConfigRepository
    {
        [Parameter(
            ParameterSetName = "Name",
            ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string[] Name { get; set; }

        [Parameter(
            ParameterSetName = "Object",
            ValueFromPipeline = true)]
        public FirewallObject[] FirewallObject { get; set; }

        [Parameter(ParameterSetName = "Name")]
        [Parameter(ParameterSetName = "Object")]
        [Parameter(ParameterSetName = "GetAll")]
        public SwitchParameter FromCandidateConfig { get; set; }

        protected ConfigTypes ConfigType { get; set; }

        protected string SchemaName { get; set; }
        
        protected void FilterResponse<T>(Dictionary<string, T> returnedObjects) where T : FirewallObject
        {
            // nothing was returned so writing null and exiting.
            if (returnedObjects == null || returnedObjects.Count == 0)
            {
                WriteWarning("Api did not returen any data");
                WriteObject(null);
                return;
            }

            if (Name == null && FirewallObject == null)
            {
                WriteObject(returnedObjects.Values.Where(a => a.GetType() == typeof(T)).ToArray(), true);
                return;
            }
                

            switch (ParameterSetName)
            {
                case "Name":
                    if (Name != null)
                    {
                        foreach (var name in Name)
                        {
                            if (returnedObjects.ContainsKey(name))
                            {
                                WriteObject(returnedObjects[name]);
                            }
                            else
                            {
                                WriteObject(new ObjectNotFoundError(name));
                            }
                        }
                    }
                    break;

                case "Object":
                    foreach (var o in FirewallObject)
                    {
                        var firewallObject = (T)o;
                        if (returnedObjects.ContainsValue(firewallObject))
                        {
                            WriteObject(returnedObjects[firewallObject.Name]);
                        }
                        else
                        {
                            WriteObject(new ObjectNotFoundError(firewallObject));
                        }
                    }
                    break;
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ConfigType = FromCandidateConfig ? ConfigTypes.Candidate : ConfigTypes.Running;
        }
    }
}

