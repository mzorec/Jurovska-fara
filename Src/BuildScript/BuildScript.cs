using System;
using System.IO;
using Flubu;
using Flubu.Builds;
using Flubu.Builds.Tasks;
using Flubu.Builds.VSSolutionBrowsing;
using Flubu.Packaging;
using Flubu.Targeting;
using Flubu.Tasks.Iis;
using Flubu.Tasks.Iis.Iis6;

//css_ref System.Core;
//css_ref ..\packages\Flubu.1.0.4.0\lib\Flubu.dll;
//css_ref ..\packages\Flubu.1.0.4.0\lib\Flubu.Contrib.dll;
//css_imp BuildHelper.cs;
using Flubu.Tasks.Processes;

namespace BuildScript
{
    public class BuildScript
    {
        public static int Main(string[] args)
        {
            TargetTree targetTree = new TargetTree();
            BuildTargets.FillBuildTargets(targetTree);

            targetTree.AddTarget("update.version")
                .SetDescription("Fetch the build version")
                .Do(TargetFetchBuildVersion);

            targetTree.AddTarget("set.debug")
                .SetAsHidden()
                .Do(x => x.Properties.Set(BuildProps.BuildConfiguration, "Debug"));

            targetTree.AddTarget("rebuild")
                .SetAsDefault()
                .SetDescription("Rebuilds the whole project")
                .DependsOn("update.version", "compile", "unit.tests");

            targetTree.AddTarget("unit.tests")
                .SetDescription("Runs unit tests on the project")
                .Do(TargetUnitTest).DependsOn("load.solution");

            using (TaskSession session = new TaskSession(new SimpleTaskContextProperties(), args, targetTree))
            {
                BuildTargets.FillDefaultProperties(session);
                session.Start(BuildTargets.OnBuildFinished);

                session.AddLogger(new MulticoloredConsoleLogger(Console.Out));

                session.Properties.Set(BuildProps.CompanyName, "Comtrade d.o.o.");
                session.Properties.Set(BuildProps.CompanyCopyright, "Copyright (C) 2014 Comtrade d.o.o.");
                session.Properties.Set(BuildProps.ProductId, "Vortex");
                session.Properties.Set(BuildProps.ProductName, "Vortex");
                session.Properties.Set(BuildProps.SolutionFileName, @"Vortex.sln");
                session.Properties.Set(BuildProps.VersionControlSystem, VersionControlSystem.Subversion);
                session.Properties.Set(BuildProps.ProductRootDir, ".");
                session.Properties.Set(BuildProps.BuildConfiguration, "Release");
                session.Properties.Set(BuildProps.TargetDotNetVersion, "v4.0.30319");
                session.Properties.Set(BuildProps.NUnitConsolePath,
                    @"packages\NUnit.Runners.2.6.3\tools\nunit-console.exe");
                try
                {
                    // actual run);
                    if (args.Length == 0)
                        targetTree.RunTarget(session, targetTree.DefaultTarget.TargetName);
                    else
                    {
                        string targetName = args[0];
                        if (false == targetTree.HasTarget(targetName))
                        {
                            session.WriteError("ERROR: The target '{0}' does not exist", targetName);
                            targetTree.RunTarget(session, "help");
                            return 2;
                        }

                        targetTree.RunTarget(session, args[0]);
                    }

                    session
                        .Complete();

                    return 0;
                }
                catch (TaskExecutionException)
                {
                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return 1;
                }
            }
        }

        public static void TargetFetchBuildVersion(ITaskContext context)
        {
            string productRootDir = context.Properties.Get(BuildProps.ProductRootDir, ".");
            string productId = context.Properties.Get<string>(BuildProps.ProductId);

            VersionControlSystem versionControlSystem = context.Properties.Get<VersionControlSystem>(
                BuildProps.VersionControlSystem);

            IFetchBuildVersionTask task;
            if (HudsonHelper.IsRunningUnderHudson)
                task = new FetchBuildVersionFromHudsonTask(
                    productRootDir,
                    productId,
                    v =>
                    {
                        int hudsonBuildNumber = HudsonHelper.BuildNumber;
                        int revisionNumber;

                        switch (versionControlSystem)
                        {
                            case VersionControlSystem.Subversion:
                                revisionNumber = HudsonHelper.SvnRevision;
                                break;
                            case VersionControlSystem.Mercurial:
                                revisionNumber = 0;
                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        return new Version(
                            v.Major,
                            v.Minor,
                            v.Build,
                            revisionNumber);
                    });
            else
                task = new FetchBuildVersionFromFileTask(productRootDir, productId);

            task.Execute(context);

            context.Properties.Set(BuildProps.BuildVersion, task.BuildVersion);
        }

        private static void TargetUnitTest(ITaskContext context)
        {
            BuildTargets.TargetRunTestsNUnit(
                context,
                "Vortex.Tests");
        }

     
    }
}
