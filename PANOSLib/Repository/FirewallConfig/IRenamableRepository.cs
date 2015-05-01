namespace PANOS
{
    public interface IRenamableRepository
    {
        void Rename(string schemaName, string oldName, string newName);
    }
}
