using System;
using System.Collections.Generic;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ClassViewModel(
    string Id,
    string Title,
    School School,
    TeacherShortInfo Teacher,
    DateOnly InitialDate,
    IEnumerable<ScheduleEvent> Events);

public record ScheduleEvent(
    int Id,
    string Title,
    string Start,
    string End,
    string EventDescriptionLabel,
    string ClassName = "fullcalendar-custom-event-tasks",
    string EventLocationLabel = "Online",
    string RepeatField = "never");
    
// className: "fullcalendar-custom-event-reminders",
// eventDescriptionLabel: "",
// eventLocationLabel: "Online",
// repeatField: "everyday",
// guestsField: [],
// image: '/assets/svg/brands/pdf-icon.svg'