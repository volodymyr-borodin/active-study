using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Scheduler;

public record CreateEventInputModel(
    [Required] string SchoolId,
    [Required] string SubjectId,
    [Required] string TeacherId,
    [Required] string ClassId,
    [DataType(DataType.Date)] DateOnly Date,
    [DataType(DataType.Time)] TimeOnly From,
    [DataType(DataType.Time)] TimeOnly To,
    string Description);
