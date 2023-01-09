﻿using System.ComponentModel.DataAnnotations;
using System;

namespace Restorantt.Models
{
    public class Blog
    {
        [Key] //id benzersiz olacak
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Image { get; set; }
        public bool Onay { get; set; }
        [Required]
        public string Mesaj { get; set; }
        public DateTime Tarih { get; set; }

    }
}

