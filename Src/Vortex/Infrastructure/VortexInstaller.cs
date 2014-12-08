using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Vortex.Database.Mappings;

namespace Vortex.Infrastructure
{
    public class LoyaltyCoreWindsorInstaller : IDisposable
    {
        public LoyaltyCoreWindsorInstaller()
        {
            DbConnectionFilePath = "..\\..\\..\\DBConnectionData.txt";
        }

        public LoyaltyCoreWindsorInstaller(string dbConnectionFilePath)
        {
            DbConnectionFilePath = dbConnectionFilePath;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ////container.Register(Component.For<ICoreConfiguration>().ImplementedBy<CoreConfiguration>().Start());
            ////container.Register(Component.For<IApplicationInfo>().ImplementedBy<ApplicationInfo>());
            ////container.Register(Component.For<IKeyValueStore>().ImplementedBy<SimpleKeyValueStore>().Named("KeyValueStore"));
            container.Register(Component.For<ITimeProvider>().ImplementedBy<TimeProvider>());
          
            //container.Register(Component.For<IRepositoryFactory>().ImplementedBy<RepositoryFactory>().LifeStyle.Singleton);

            ISessionFactory nhibernateSessionFactory = CreateSessionFactory(container);
            container.Register(Component.For<ISessionFactory>().Instance(nhibernateSessionFactory));
        }

        public string DbConnectionFilePath { get; protected set; }

        public int Order
        {
            get { return 10; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                // clean native resources         
                if (disposing)
                {
                    // clean managed resources       
                }

                disposed = true;
            }
        }

        private ISessionFactory CreateSessionFactory(IWindsorContainer container)
        {
            string server, name, username, password;

            bool integratedAuth = false;

            //log.DebugFormat("development mode: {0}", IsInDevelopmentMode);

           
            GetDBConnectionDataFromFile(out server, out name, out username, out password);

            return Fluently.Configure()
                .Database(
                integratedAuth ?
                MsSqlConfiguration.MsSql2008
                              .ConnectionString(c => c.Server(server)
                                                         .Database(name)
                                                         .TrustedConnection())
                  : MsSqlConfiguration.MsSql2008
                              .ConnectionString(c => c.Server(server)
                                                         .Database(name)
                                                         .Username(username)
                                                         .Password(password)))
                .Mappings(m => m.FluentMappings
                                   .AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
        }

        private static bool IsInDevelopmentMode
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["DevelopmentMode"];
                if (string.IsNullOrWhiteSpace(setting))
                {
                    return false;
                }

                return Convert.ToBoolean(setting, CultureInfo.InvariantCulture);
            }
        }

        private void GetDBConnectionDataFromFile(
          out string databaseServer,
          out string databaseName,
          out string databaseUsername,
          out string databasePassword)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string pathToFile = Uri.UnescapeDataString(uri.Path);
            pathToFile = Path.Combine(pathToFile, DbConnectionFilePath);

            if (File.Exists(pathToFile))
            {
                string[] data = File.ReadAllLines(pathToFile);
                databaseServer = data[1].Substring(data[1].IndexOf(':') + 1).Trim();
                databaseName = data[2].Substring(data[2].IndexOf(':') + 1).Trim();
                databaseUsername = data[3].Substring(data[3].IndexOf(':') + 1).Trim();
                databasePassword = data[4].Substring(data[4].IndexOf(':') + 1).Trim();
            }
            else
            {
                throw new FileNotFoundException("file not found", pathToFile);
            }
        }

        private bool disposed;
    }
}
