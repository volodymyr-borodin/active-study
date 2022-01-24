using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record ClassScheduleTemplateInputModel(
    [DataType(DataType.Date), Required] DateOnly EffectiveFrom,
    [DataType(DataType.Date), Required] DateOnly EffectiveTo,
    List<ScheduleTemplateEventPeriodInputModel> Periods);
