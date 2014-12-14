//css_ref System.Data;
//css_ref Flubu.dll;
//css_ref Flubu.Contrib.dll;
//css_ref BuildScript.dll

using System;
using BuildScript.Yolped;
using Flubu;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using Build.Yolped;
using Flubu.Tasks.Text;
using log4net;

namespace BuildScript
{
    public class DeployScript{
        public static int Main(params string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            DeployScript script = new DeployScript();
            try
            {
                script.Run(new List<String>(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return 0;
        }

        public void Run(IEnumerable<String> args)
        {
            IDeploymentBuilder builder = new DeploymentBuilder2();

            DeploymentStep initStep = builder
                .Step(new SetSourceDirAction(".."))
                .Then(new ReadConfigFileAction(@"Install\DeploymentConfig\Installation.config.xml"))
                .Then(new CopyParameterValueAction("Loyalty/HomeDir", StandardPipedParameters.Directory))
                .Then(new CreateDirAction())
                .Then(new SetHomeDirAction());

            ////DeploymentStep appPoolStep = builder.Step(new PrepareApplicationPoolAction("Loyalty"));

            builder.Step(new CopySubdirectoryToHomeAction("Loyalty.Console"));

            builder.Step(new CopySubdirectoryToHomeAction("Loyalty.Server"))
               .DependsOn(initStep)
               .Then(new CustomDeploymentAction(ConfigureWebServiceWebConfigAction))
               ////.Then(new PrepareWebApplicationAction("Loyalty"))
               ////.DependsOn(appPoolStep, 10)
               .Then(new CreateDirAction("logs", "Network Service", FileSystemRights.Read | FileSystemRights.Write, AccessControlType.Allow));

            builder.Step(new CopySubdirectoryToHomeAction("Loyalty.UssdWs"))
              .DependsOn(initStep)
              .Then(new CustomDeploymentAction(ConfigureWebServiceWebConfigAction))
             //// .Then(new PrepareWebApplicationAction("Loyalty.UssdWs"))
             //// .DependsOn(appPoolStep, 10)
              .Then(new CreateDirAction("logs", "Network Service", FileSystemRights.Read | FileSystemRights.Write, AccessControlType.Allow));

            builder
                .Step(new CopySubdirectoryToHomeAction("Loyalty.Service"))
                .DependsOn(initStep)
                .Then(new CustomDeploymentAction(ConfigureTaskServiceConfigAction))
                .Then(new CustomDeploymentAction(ConfigureWindsorConfigAction))
                ////.Then(new PrepareWinServiceAction(
                ////    "LoyaltyProgram.Service.exe",
                ////    "LoyaltyService",
                ////    false).SkipReinstallationIfExists("Loyalty/TaskService/DoNotReinstall", true))
                .Then(new CreateDirAction("logs", "Network Service", FileSystemRights.Read | FileSystemRights.Write, AccessControlType.Allow));

            MemoryDeploymentAudit audit = new MemoryDeploymentAudit();

            using (DeploymentContext context = new DeploymentContext(new SimpleTaskContextProperties(), args))
            {
                context.SimulationMode = false;
                context.Log = log;
                context.Audit = audit;

                ((IDeploymentRunner)builder).Run(context);

                Console.Out.WriteLine();
                Console.Out.WriteLine("INSTALLATION STEPS EXECUTED:");
                Console.Out.WriteLine("----------------------------");
                Console.Out.WriteLine(context.Audit);

                context.Complete();
            }
        }

        private static void ConfigureWebServiceWebConfigAction(DeploymentContext context, Pipeline pipeline)
        {
            string fullWebConfigPath = Path.Combine(
                (string)pipeline.GetParameter(StandardPipedParameters.Directory),
                @"Web.config");
            context.Audit.RecordAction("configure|{0}", fullWebConfigPath);

            UpdateXmlFileTask updateConfigTask = new UpdateXmlFileTask(fullWebConfigPath);

            updateConfigTask.DeletePath(
                    "/configuration/appSettings/add[@key='DevelopmentMode']");

            updateConfigTask.UpdatePath(
                "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseServer", (string)pipeline.GetParameter("Loyalty/DB/Server"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseName", (string)pipeline.GetParameter("Loyalty/DB/DatabaseName"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseUserName", (string)pipeline.GetParameter("Loyalty/DB/UserName"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabasePassword", (string)pipeline.GetParameter("Loyalty/DB/Password"));
            updateConfigTask.Execute(context);
                
            updateConfigTask.Execute(context);
        }

        private static void ConfigureTaskServiceConfigAction(DeploymentContext context, Pipeline pipeline)
        {
            string fullAppConfigPath = Path.Combine(
               (string)pipeline.GetParameter(StandardPipedParameters.Directory),
               @"LoyaltyProgram.Service.exe.config");
            context.Audit.RecordAction("configure|{0}", fullAppConfigPath);

            UpdateXmlFileTask updateConfigTask = new UpdateXmlFileTask(fullAppConfigPath);

            updateConfigTask.UpdatePath(
                "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseServer", (string)pipeline.GetParameter("Loyalty/DB/Server"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseName", (string)pipeline.GetParameter("Loyalty/DB/DatabaseName"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabaseUserName", (string)pipeline.GetParameter("Loyalty/DB/UserName"));
            updateConfigTask.UpdatePath(
               "/configuration/castle/components/component['LoyaltyProgram.Core.Configuration.CoreConfiguration']/parameters/DatabasePassword", (string)pipeline.GetParameter("Loyalty/DB/Password"));
            updateConfigTask.Execute(context);
        }

        private static void ConfigureWindsorConfigAction(DeploymentContext context, Pipeline pipeline)
        {
            ////string fullWinsorConfigPath = Path.Combine(
            ////   (string)pipeline.GetParameter(StandardPipedParameters.Directory),
            ////   @"windsor.config");
            ////context.Audit.RecordAction("configure|{0}", fullWinsorConfigPath);

            ////UpdateXmlFileTask updateConfigTask = new UpdateXmlFileTask(fullWinsorConfigPath);
            ////if (pipeline.HasParameter("Bap0/CustomValidators/Gbkr/ZbsValidateReference"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='HermesSoftLab.BankingCore.Bap0ServiceController.Processors.GbkrCustomValidator']/parameters/ZbsValidateReference",
            ////        (string)pipeline.GetParameter("Bap0/CustomValidators/Gbkr/ZbsValidateReference"));

            ////if (pipeline.HasParameter("Bap0/CustomValidators/Gbkr/ZbsValidateNrcReference"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='HermesSoftLab.BankingCore.Bap0ServiceController.Processors.GbkrCustomValidator']/parameters/ZbsValidateNrcReference",
            ////        (string)pipeline.GetParameter("Bap0/CustomValidators/Gbkr/ZbsValidateNrcReference"));

            ////if (pipeline.HasParameter("Bap0/CustomValidators/Gbkr/Ba1ValidateStatistics"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='HermesSoftLab.BankingCore.Bap0ServiceController.Processors.GbkrCustomValidator']/parameters/Ba1ValidateStatistics",
            ////        (string)pipeline.GetParameter("Bap0/CustomValidators/Gbkr/Ba1ValidateStatistics"));

            ////if (pipeline.HasParameter("Bap0/ValidationConfiguration/MaxForeignUpnAmount"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='ValidationConfigurationProvider']/parameters/MaxForeignAmount",
            ////        (string)pipeline.GetParameter("Bap0/ValidationConfiguration/MaxForeignUpnAmount"));

            ////if (pipeline.HasParameter("Bap0/ValidationConfiguration/MaxForeignAmountWithoutStatistics"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='ValidationConfigurationProvider']/parameters/MaxForeignAmountWithoutStatistics",
            ////        (string)pipeline.GetParameter("Bap0/ValidationConfiguration/MaxForeignAmountWithoutStatistics"));

            ////if (pipeline.HasParameter("Bap0/ValidationConfiguration/MaxDomesticAmountWithoutStatistics"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='ValidationConfigurationProvider']/parameters/MaxDomesticAmountWithoutStatistics",
            ////        (string)pipeline.GetParameter("Bap0/ValidationConfiguration/MaxDomesticAmountWithoutStatistics"));

            ////if (pipeline.HasParameter("Bap0/ValidationConfiguration/RequireUpnStatistics"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='ValidationConfigurationProvider']/parameters/RequireUpnStatistics",
            ////        (string)pipeline.GetParameter("Bap0/ValidationConfiguration/RequireUpnStatistics"));

            ////if (pipeline.HasParameter("Bap0/ValidationConfiguration/BankId"))
            ////    updateConfigTask.UpdatePath(
            ////        "/configuration/components/component[@id='ValidationConfigurationProvider']/parameters/BankId",
            ////        (string)pipeline.GetParameter("Bap0/ValidationConfiguration/BankId"));

            ////updateConfigTask.Execute(context);
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(DeployScript));
    }
}