using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace app.Controllers.API
{
    public class VideosController : ApiController
    {
        private ApplicationDbContext _context;

        public VideosController()
        {
            _context = new ApplicationDbContext();
        }

        //GET /api/videos
        [HttpGet]
        public IEnumerable<Video> GetVideos()
        {
            return _context.Videos.ToList();
        }

        //GET /api/videos/1
        [HttpGet]
        public Video GetVideo(int id)
        {
            var video = _context.Videos.SingleOrDefault(v => v.Id == id);

            if(video == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return video;
        }

        // POST /api/videos
        [HttpPost]
        public Video CreateVideo(Video video)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            _context.Videos.Add(video);
            _context.SaveChanges();

            return video;
        }

        // PUT /api/videos/1
        [HttpPut]
        public void UpdateVideo(int id, Video video)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var videodb = _context.Videos.SingleOrDefault(v => v.Id == id);

            if (video == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            videodb.Title = video.Title;
            videodb.Description = video.Description;
            videodb.Genre = video.Genre;
            _context.SaveChanges();
        }

        //delete /api/delete/1
        [HttpDelete]
        public void DeleteVideo(int id)
        {
            var video = _context.Videos.SingleOrDefault(v => v.Id == id);

            if (video == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Videos.Remove(video);
            _context.SaveChanges();
        }
    }
}