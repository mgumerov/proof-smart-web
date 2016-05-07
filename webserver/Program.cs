using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Nancy.Hosting.Self;
using Nancy;
using Nancy.Conventions;
using Nancy.ViewEngines.Razor;

namespace webserver
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            HostConfiguration cfg = new HostConfiguration();
            cfg.RewriteLocalhost = false;
            using (var host = new NancyHost(new ApplicationBootstrapper(/*args.Length == 1 ? args[0] : */AppDomain.CurrentDomain.BaseDirectory),
                cfg, new Uri("http://localhost:8080")))
            {
                host.Start();
                Application.Run(new Form());
            }
        }
    }

    public class OilBalancesParams
    {
        public float square;
        public float initialSaturatedThickness;
    }

    public class CalcParams
    {
        public OilBalancesParams oilBalances;
        public float waterCompressibility;
    }

    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "HyRes.Models";
            yield return "HyRes.Website";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "HyRes.Models";
            yield return "HyRes.Website.Infrastructure.Helpers";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }

    public class ApplicationBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IRootPathProvider rootPathProvider;

        public ApplicationBootstrapper(string rootpath)
        {
            this.rootPathProvider = new SelfHostRootPathProvider(rootpath);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static", @"Static"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override IRootPathProvider RootPathProvider
        {
            get { return rootPathProvider; }
        }
    }
    
    public class SelfHostRootPathProvider : IRootPathProvider
    {
        private readonly string path;

        public SelfHostRootPathProvider(string path)
        {
            this.path = path;
        }

        public string GetRootPath()
        {
            return path;
        }
    }
}
