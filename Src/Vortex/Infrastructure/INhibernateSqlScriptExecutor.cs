namespace Vortex.Infrastructure
{
    public interface INhibernateSqlScriptExecutor
    {
        void ExecuteSQLFile(string sqlFilePath);

        void ExecuteSQLStatment(string sqlstatment, bool allowErrors = false);
    }
}
