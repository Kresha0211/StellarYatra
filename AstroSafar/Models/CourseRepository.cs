using System;
using System.Collections.Generic;
using AstroSafar.Models;

namespace AstroSafar.Data
{
    public static class CourseRepository
    {
        public static List<Course> GetInitialCourses()
        {
            return new List<Course>
            {
                new Course { CourseID = 1, Title = "Introduction to Space", Description = "Basics of space and astronomy.", Duration = "4 hours", PublishedDate = DateTime.UtcNow, IsPublished = true, ImageURL = "/images/Space.jpg" },
                new Course { CourseID = 2, Title = "Rocket Science 101", Description = "Fundamentals of rocket propulsion.", Duration = "6 hours", PublishedDate = DateTime.UtcNow, IsPublished = true, ImageURL = "/images/Rocket.jpg" },
                new Course { CourseID = 3, Title = "Black Holes Explained", Description = "Dive into black hole mysteries.", Duration = "5 hours", PublishedDate = DateTime.UtcNow, IsPublished = true, ImageURL = "/images/Blackhole2.jpg" }
            };
        }

        public static List<Course> GetMoreCourses()
        {
            return new List<Course>
            {
                new Course { CourseID = 4, Title = "Astrobiology",
                                           Description = "Study of life in the universe.",
                                           Duration = "3 hours",
                                           PublishedDate = DateTime.UtcNow, 
                                           IsPublished = true,
                                           ImageURL = "/images/Astrobiology.jpg" },
                new Course { CourseID = 5, Title = "Mars Exploration", Description = "History and future of Mars missions.", Duration = "4.5 hours", PublishedDate = DateTime.UtcNow, IsPublished = true, ImageURL = "/images/mars.jpg" },
                new Course { CourseID = 6, Title = "Exoplanets", Description = "Discover planets beyond our solar system.", Duration = "5 hours", PublishedDate = DateTime.UtcNow, IsPublished = true, ImageURL = "/images/explanets.jpg" }
            };
        }

    }
}

