namespace PANOS
{
    using System;

    public class RenamableRepository : IRenamableRepository
    {
         private readonly IConfigCommandFactory commandFactory;

         public RenamableRepository(IConfigCommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public void Rename(string schemaName, string oldName, string newName)
        {
            var renameCommand = commandFactory.CreateRename(schemaName, oldName, newName);
            var response = renameCommand.Execute();
            // What is the status of an attempt to rename an non-existing object
            if (response.Status.Equals("success"))
            {
                return;
            }

            throw new Exception(string.Format("Rename Method failed. PANOS error code {0}", response.Status));
        }
    }
}
