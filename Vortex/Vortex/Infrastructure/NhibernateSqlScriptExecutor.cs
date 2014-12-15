using System;
using System.IO;
using System.Text.RegularExpressions;
using NHibernate;

namespace Vortex.Infrastructure
{
    public class NhibernateSqlScriptExecutor : INhibernateSqlScriptExecutor
    {
        private readonly ISession session;

        public NhibernateSqlScriptExecutor(ISession session)
        {
            this.session = session;
        }

        public void ExecuteSQLFile(string sqlFilePath)
        {
            string sql;
            using (FileStream strm = File.OpenRead(sqlFilePath))
            {
                var reader = new StreamReader(strm);
                sql = reader.ReadToEnd();
            }

            var regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] lines = regex.Split(sql);

            foreach (string line in lines)
            {
                IQuery query = session.CreateSQLQuery(line);
                query.ExecuteUpdate();
            }
        }

        public void ExecuteSQLStatment(string sqlstatment, bool allowErrors)
        {
            try
            {
                IQuery query = session.CreateSQLQuery(sqlstatment);
                query.ExecuteUpdate();
            }
            catch (Exception)
            {
                if (!allowErrors)
                {
                    throw;
                }
            }
        }
    }
}