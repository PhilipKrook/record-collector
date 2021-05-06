using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Krompaco.RecordCollector.Web.Models
{
    public class UiWebRootModel
    {
        private static object lockToOnce;

        public string GetLatestFromDist(string path, string fileNamePattern)
        {
            if (lockToOnce == null)
            {
                // This is a way to get webpack to build a new purged CSS file when app has restarted
                var diWebRoot = new DirectoryInfo(path);
                var src = diWebRoot.Parent?.Parent;

                if (src != null)
                {
                    var fullName = Path.Combine(src.FullName, "styles.css");

                    try
                    {
                        using var w = File.AppendText(fullName);
                        w.Write($"\r\n/* {DateTime.Now:yyyy-MM-dd HH:mm:ss} */");
                    }
                    catch (Exception)
                    {
                    }
                }

                lockToOnce = new object();
            }

            var di = new DirectoryInfo(Path.Combine(path, "dist"));
            var list = di.EnumerateFiles(fileNamePattern, SearchOption.TopDirectoryOnly).ToList();

            if (list.Count <= 1)
            {
                return list.Count == 1 ? "/dist/" + list[0].Name : string.Empty;
            }

            var latest = list.OrderByDescending(x => x.CreationTime).First().Name;

            foreach (var fi in list)
            {
                if (fi.Name.Equals(latest, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    File.Delete(fi.FullName);
                }
                catch (Exception)
                {
                }
            }

            return "/dist/" + latest;
        }
    }
}
