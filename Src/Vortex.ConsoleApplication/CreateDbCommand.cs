using System.Collections.Generic;
using LoyaltyProgram.Console;
using NHibernate;
using Vortex.Infrastructure;

namespace Vortex.ConsoleApplication
{
   public class CreateDbCommand : DbCommandBase
    {
       public override string CommandDescription
       {
           get { return "Generates Loyalty Database"; }
       }

       public override string CommandName
       {
           get { return "create.db"; }
       }

       public override int Execute(IEnumerable<string> args)
       {
               VerifyArguments(args);
               var tmp = DBName;
               DBName = "Master";
               using (ISessionFactory sessionFactory = CreateSessionFactory())
               {
                   using (ISession session = sessionFactory.OpenSession())
                   {
                       DBName = tmp;
                       INhibernateSqlScriptExecutor scriptExecutor = new NhibernateSqlScriptExecutor(session);
                       scriptExecutor.ExecuteSQLStatment(string.Format("drop database {0};", DBName), true);
                       scriptExecutor.ExecuteSQLStatment(string.Format("create database {0}", DBName));
                   }
               }
          
               using (ISessionFactory sessionFactory2 = CreateSessionFactory())
               {
                   using (ISession session = sessionFactory2.OpenSession())
                   {
                       INhibernateSqlScriptExecutor scriptExecutor = new NhibernateSqlScriptExecutor(session);
                       scriptExecutor.ExecuteSQLFile(@"..\..\..\Vortex\Database\Scripts\Latest\Vortex.Create.sql");
                   }
               }

           return 0;
       }
    }
}
