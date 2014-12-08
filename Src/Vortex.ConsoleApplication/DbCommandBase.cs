using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Vortex.Database.Mappings;

namespace Vortex.ConsoleApplication
{
    public abstract class DbCommandBase : ConsoleCommandBase
    {
        protected DbCommandBase()
        {
            Options.Add(
                "s|server=",
                "the name of the {DB server} (if not specified readed from config file)",
                x => dbServer = x);
            Options.Add("db|database=", "the name of the {database} (optional)", x => dbName = x);
            Options.Add(
                "u|username=", "the DB {user name} (if not specified readed from config file)", x => dbUserName = x);
            Options.Add(
                "p|password=", "the DB {user password} (if not specified readed from config file)", x => dbPassword = x);
        }

        protected ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                 .Database(MsSqlConfiguration.MsSql2012
                               .ConnectionString(c => c.Server(dbServer)
                                                          .Database(dbName)
                                                          .Username(dbUserName)
                                                          .Password(dbPassword)))
                 .Mappings(m => m.FluentMappings
                                    .AddFromAssemblyOf<UserMap>())
                                    .ExposeConfiguration(BuildSchema)

                  .BuildSessionFactory();
        }

        protected virtual void VerifyArguments(IEnumerable<string> args)
        {
            List<string> unhandledArguments = Options.Parse(args);

            string arguments = string.Empty;
            foreach (string unhandledArgument in unhandledArguments)
            {
                arguments = string.Concat(arguments, " ", unhandledArgument);
            }

            if (unhandledArguments.Count > 0)
            {
                ShowHelp();
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "There are some unsupported options:{0}", arguments));
            }
        }

        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists("LatestGeneratedNHScript.sql"))
            {
                File.Delete("LatestGeneratedNHScript.sql");
            }

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .SetOutputFile("LatestGeneratedNHScript.sql").Execute(false, false, false);
        }

       protected string DBServer
       {
           get
           {
               return dbServer;
           }

           set
           {
               dbServer = value;
           }
       }

        protected string DBName
       {
           get
           {
               return dbName;
           }

           set
           {
               dbName = value;
           }
       }

       protected string DBPassword
       {
           get
           {
               return dbPassword;
           }

           set
           {
               dbPassword = value;
           }
       }

       protected string DBUserName
       {
           get
           {
               return dbUserName;
           }

           set
           {
               dbUserName = value;
           }
       }

       private string dbServer;

       private string dbName = null;

       private string dbPassword;

       private string dbUserName;
    }
}
