namespace PANOS
{
    public interface IDeletableRepository
    {
        void Delete(string schemaName, string name);
    }
}
