using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;
using Vortex.Database.Models;
using Vortex.Database.Repository;
using Vortex.Infrastructure;

namespace Vortex.Tests
{
    public class TestBaseUsingDb
    {
        protected IRepositoryFactory RepositoryFactory { get; set; }

        public ISessionFactory CreateSessionFactory()
        {
            string dbServer, dbName, dbUsername, dbPassword;
            GetDBConnectionDataFromFile(out dbServer, out dbName, out dbUsername, out dbPassword);
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.Server(dbServer)
                        .Database(dbName)
                        .Username(dbUsername)
                        .Password(dbPassword)))
                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<User>())
                .BuildSessionFactory();
        }

        protected static void GetDBConnectionDataFromFile(
            out string dbServer,
            out string dbName,
            out string dbUsername,
            out string dbPassword)
        {
            const string DataFileName = @"..\..\..\Vortex\DBConnectionData.txt";

            if (File.Exists(DataFileName))
            {
                string[] data = File.ReadAllLines(DataFileName);
                dbServer = data[1].Substring(data[1].IndexOf(':') + 1).Trim();
                dbName = data[2].Substring(data[2].IndexOf(':') + 1).Trim();
                dbUsername = data[3].Substring(data[3].IndexOf(':') + 1).Trim();
                dbPassword = data[4].Substring(data[4].IndexOf(':') + 1).Trim();
            }
            else
            {
                throw new FileNotFoundException("file not found", DataFileName);
            }
        }

        [TestFixtureSetUp]
        protected virtual void FixtureSetup()
        {
            ISessionFactory sessionFactory = CreateSessionFactory();
            RepositoryFactory = new RepositoryFactory(sessionFactory, new TimeProvider());
        }

        [SetUp]
        protected virtual void Setup()
        {
            using (ISession session = RepositoryFactory.OpenSession())
            {
                INhibernateSqlScriptExecutor scriptExecutor = new NhibernateSqlScriptExecutor(session);
            }
        }

        [TestFixtureTearDown]
        protected virtual void TestFixtureTearDown()
        {
            RepositoryFactory.Dispose();
        }
    }
}
