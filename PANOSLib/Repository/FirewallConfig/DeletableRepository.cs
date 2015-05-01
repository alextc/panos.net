namespace PANOS
{
    using System;

    public class DeletableRepository : IDeletableRepository
    {
         private readonly IConfigCommandFactory commandFactory;

         public DeletableRepository(IConfigCommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public void Delete(string schemaName, string name)
        {
            var response = commandFactory.CreateDelete(schemaName, name).Execute();
            // What is the status of an attempt to delete an non-existing object
            if (response.Status.Equals("success") && !response.Message.Equals("Object doesn't exist"))
            {
                return;
            }

            if (response.Message.Equals("Object doesn't exist"))
            {
                throw new ObjectNotFound(string.Format("Attempt to Delete a non-existing object {0}", name));
            }

            throw new Exception(string.Format("Delete Method failed. PANOS error code {0}", response.Status));
        }
    }
}
