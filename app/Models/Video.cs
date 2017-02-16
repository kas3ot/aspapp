using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        
        public string VideoImg { get; set; }
    }

    public enum Genre
    {
        Comedy = 1,
        Horror,
        Video_tutorials,
        SciFi,
        Romance,
        Documentary,
        Kids
    }
}