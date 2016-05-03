using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Nancy.Hosting.Self;
using Nancy;
using Nancy.Conventions;
using Nancy.ModelBinding;
using System.IO;
using Nancy.Responses;

namespace webserver
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            HostConfiguration cfg = new HostConfiguration();
            cfg.RewriteLocalhost = false;
            using (var host = new NancyHost(new ApplicationBootstrapper(args.Length == 1 ? args[0] : AppDomain.CurrentDomain.BaseDirectory),
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

    public class Sucaba : NancyModule
    {
        public Sucaba(IRootPathProvider pathProvider)
        {
            Post["/chart"] = x =>
            {
                //Вытащим параметры расчета из запроса. Но использовать не будем, отдадим муляж.
                CalcParams calcparams = this.Bind<CalcParams>();

                var ss = new MemoryStream();
                StreamWriter sw = new StreamWriter(ss);
                sw.WriteLine("Date,V1,V2");
                DateTime dt = DateTime.Today;
                Random r = new Random();
                for (int i = 0; i < 100; i++)
                {
                    sw.Write(string.Format("{0:yyyy}{0:MM}{0:dd}", dt.AddDays(i)));
                    sw.Write(",");
                    sw.Write(r.Next(30));
                    sw.Write(",");
                    sw.Write(r.Next(30));
                    sw.WriteLine();
                }
                sw.Flush();
                ss.Position = 0;

                var response = new StreamResponse(() => ss, MimeTypes.GetMimeType("text/csv"));
                  
                return response.AsAttachment("chart.csv");
            };
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
