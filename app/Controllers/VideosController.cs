using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Add(Video video)
        {
            _dbContext.Videos.Add(video);
            _dbContext.SaveChanges();
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
        public ActionResult Update(Video video)
        {
            if (Request.IsAuthenticated)
            {
                var videoInDb = _dbContext.Videos.SingleOrDefault(v => v.Id == video.Id);

                if (videoInDb == null)
                    return HttpNotFound();

                videoInDb.Title = video.Title;
                videoInDb.Description = video.Description;
                videoInDb.Genre = video.Genre;
                _dbContext.SaveChanges();

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

                if (video == null)
                    return HttpNotFound();
                _dbContext.Videos.Remove(video);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("../Account/Login");
        }

        public ActionResult Search(string search)
        {
            var videos = _dbContext.Videos.OrderByDescending(id => id.Id).Where(s => s.Title == search).ToList();
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
    }
}