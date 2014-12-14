namespace BuildScript
{
    using Flubu;
    using Flubu.Builds;
    using Flubu.Builds.VSSolutionBrowsing;

    public class BuildHelper
    {
        public static FullPath GetBuildPackagesDir(ITaskContext context)
        {
            return new FullPath(context.Properties[BuildProps.BuildDir])
                .CombineWith("packages");
        }

        public static FullPath GetProjectOutputDir(string projectName, ITaskContext context)
        {
            VSSolution solution = context.Properties.Get<VSSolution>(BuildProps.Solution);
            VSProjectWithFileInfo project = (VSProjectWithFileInfo)solution.FindProjectByName(projectName);

            return new FullPath(projectName)
                .CombineWith(project.GetProjectOutputPath(context.Properties.Get<string>(BuildProps.BuildConfiguration)));
        }
    }
}