using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Scheduler
{
    public class CreateEventInputModel
    {
        [Required]
        public string SchoolId { get; set; }
        
        [Required]
        public string SubjectId { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string ClassId { get; set; }
        
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        
        [DataType(DataType.Time)]
        public TimeOnly From { get; set; }
        
        [DataType(DataType.Time)]
        public TimeOnly To { get; set; }

        public string Description { get; set; }
    }

    public class CreateEventPageModel : CreateEventInputModel
    {
        public CreateEventPageModel(
            School school,
            IEnumerable<Subject> subjects,
            IEnumerable<Teacher> teachers,
            IEnumerable<Class> classes)
        {
            School = school;
            Subjects = subjects.Select(s => new SelectListItem(s.Title, s.Id));
            Teachers = teachers.Select(s => new SelectListItem(s.FullName, s.Id));
            Classes = classes.OrderBy(c => c.Title).Select(s => new SelectListItem(s.Title, s.Id));
        }

        public School School { get; }
        public IEnumerable<SelectListItem> Subjects { get; }
        public IEnumerable<SelectListItem> Teachers { get; }
        public IEnumerable<SelectListItem> Classes { get; }
        
        public bool DateLocked { get; set; }
        public bool TimeLocked { get; set; }
    }
}