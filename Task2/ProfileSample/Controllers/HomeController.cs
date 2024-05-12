using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;
using static StackExchange.Profiling.Helpers.Dapper.SqlMapper;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new List<ImageModel>();
            
            using (var context = new ProfileSampleEntities())
            {
                model = await context.ImgSources.Take(20).Select(x => new ImageModel { Name = x.Name, Data = x.Data }).ToListAsync();
            }

            return View(model);
        }

        public async Task<ActionResult> Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");
            var model = new List<ImgSource>();
            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        await stream.ReadAsync(buff, 0, (int) stream.Length);

                        var entity = new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };

                        model.Add(entity);
                    }
                }
                context.ImgSources.AddRange(model);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}