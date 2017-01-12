using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MediaService.Models
{
    public class Media
    {

            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            public int Year { get; set; }
            public decimal Price { get; set; }
            public string Genre { get; set; }

            // Foreign Key
            public int AuthorId { get; set; }
            // Navigation property
            public Author Author { get; set; }
    }
}
