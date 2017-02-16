using app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace app.Controllers
{
    public class VideosController : Controller
    {

        private ApplicationDbContext _dbContext;

        public VideosController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Videos
        public ActionResult Index()
        {
            var videos = _dbContext.Videos.OrderByDescending(id => id.Id).ToList();

            return View(videos);
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Add(Video video, HttpPostedFileBase image)
        {
            if(image == null)
            {
                TempData["type"] = "danger";
                TempData["info"] = "Image is required";
                return RedirectToAction("New");
            }
            var imagename = Path.GetFileName(image.FileName);
            var allowedExtansions = new[] { ".jpeg", ".jpg", ".png" };
            var checkextension = Path.GetExtension(image.FileName).ToLower();
            var size = image.ContentLength;
           

            foreach (string x in allowedExtansions)
            {
                if (!allowedExtansions.Contains(checkextension) || size > 360000)
                {
                    TempData["type"] = "danger";
                    TempData["info"] = "This image type is not allowed or is more than 36MB please try again";
                    return RedirectToAction("index");
                }
            }

            var randomstring = stringGenerateRandomString(10);
            imagename = randomstring + checkextension;
            var fullPath = "~/UploadedImages/" + imagename;
            WebImage img = new WebImage(image.InputStream);

            img.Resize(300,300);

            //image.SaveAs(Server.MapPath(fullPath));

            img.Save(fullPath);
            var videoInDb = _dbContext.Videos.ToList();
            video.VideoImg = imagename;

            _dbContext.Videos.Add(video);
            _dbContext.SaveChanges();

            TempData["type"] = "info";
            TempData["info"] = "The Video has been saved!";
           
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                var video = _dbContext.Videos.SingleOrDefault(v => v.Id == id);

                if (video == null)
                    return HttpNotFound();

                return View(video);
            }
            return RedirectToAction("../Account/Login");
        }

        [HttpPost]
        public ActionResult Update(Video video, HttpPostedFileBase image)
        {
            if (Request.IsAuthenticated)
            {
                var videoInDb = _dbContext.Videos.SingleOrDefault(v => v.Id == video.Id);

                if (videoInDb == null)
                    return HttpNotFound();

                var imagetodelete = videoInDb.VideoImg;

                var imageupload = "~/UploadedImages/"+ image.FileName;
                
                string fullPath = Request.MapPath("~/UploadedImages/" + imagetodelete);

                var allowedExtansions = new[] { ".jpeg", ".jpg", ".png" };
                var checkextension = Path.GetExtension(image.FileName).ToLower();
                var size = image.ContentLength;

                foreach (string x in allowedExtansions)
                {
                    if (!allowedExtansions.Contains(checkextension) || size > 360000)
                    {
                        TempData["type"] = "danger";
                        TempData["info"] = "This image is not allowed or is more than 36MB please try again";

                        return RedirectToAction("Edit/"+ video.Id);
                    }
                }

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                image.SaveAs(Server.MapPath(imageupload));

                videoInDb.Title = video.Title;
                videoInDb.Description = video.Description;
                videoInDb.Genre = video.Genre;
                videoInDb.VideoImg = image.FileName;
                _dbContext.SaveChanges();

                TempData["type"] = "Info";
                TempData["info"] = "Your Video has been updated";

                return RedirectToAction("Index");
            }

            return RedirectToAction("../Account/Login");

        }

        public ActionResult Delete(int id)
        {
            if (Request.IsAuthenticated)
            {
                var video = _dbContext.Videos.SingleOrDefault(v => v.Id == id);

                if (video == null)
                    return HttpNotFound();

                return View(video);
            }

            return RedirectToAction("../Account/Login");
        }

        public ActionResult DoDelete(int id)
        {
            if (Request.IsAuthenticated)
            {
                var video = _dbContext.Videos.SingleOrDefault(v => v.Id == id);
                var imagename = video.VideoImg;
                string fullPath = Request.MapPath("~/UploadedImages/" + imagename);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                if (video == null)
                    return HttpNotFound();
                _dbContext.Videos.Remove(video);
                _dbContext.SaveChanges();

                TempData["type"] = "danger";
                TempData["info"] = "The video has been deleted!";

                return RedirectToAction("Index");
            }

            return RedirectToAction("../Account/Login");
        }

        public ActionResult Search(string search)
        {
            var videos = _dbContext.Videos.OrderByDescending(id => id.Id).Where(s => s.Title.Contains(search)).ToList();
            return View(videos);
        }

        public ActionResult Genre(Genre genre)
        {
            if (genre == 0)
            {
                return View(_dbContext.Videos.OrderByDescending(id => id.Id).ToList());
            }
            var videos = _dbContext.Videos.OrderByDescending(id => id.Id).Where(s => s.Genre == genre).ToList();
            return View(videos);
        }

        public ActionResult Details(int id)
        {
            var vid = _dbContext.Videos.SingleOrDefault(v => v.Id == id);
            return View(vid);
        }

        public ActionResult imageform()
        {
            return View();
        }

        public static string stringGenerateRandomString(int length)
        {
            string c = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            //It will generate string with combination of small,capital letters and numbers
            char[] charArr = c.ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don’t Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }
    }
}