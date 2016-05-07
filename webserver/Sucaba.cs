using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Nancy.Responses;
using Nancy.ModelBinding;

namespace webserver
{
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

            Get["/"] = x =>
            {
                return View["form", null /*CalcParams*/];
            };
        }
    }
}
