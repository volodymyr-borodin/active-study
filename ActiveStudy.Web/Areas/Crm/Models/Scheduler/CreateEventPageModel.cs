using System;
using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.Crm.Models.Scheduler;

public record CreateEventPageModel(
    School School,
    IEnumerable<SelectListItem> Subjects,
    IEnumerable<SelectListItem> Teachers,
    IEnumerable<SelectListItem> Classes,
    bool DateLocked,
    bool TimeLocked,
    string SchoolId,
    string SubjectId,
    string TeacherId,
    string ClassId,
    DateOnly Date,
    TimeOnly From,
    TimeOnly To,
    string Description) : CreateEventInputModel(SchoolId, SubjectId, TeacherId, ClassId, Date, From, To, Description);
