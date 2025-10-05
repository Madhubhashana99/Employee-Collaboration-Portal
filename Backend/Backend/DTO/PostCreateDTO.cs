﻿using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class PostCreateDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
