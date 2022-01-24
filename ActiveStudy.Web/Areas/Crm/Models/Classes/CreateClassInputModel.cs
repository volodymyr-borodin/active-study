using System.ComponentModel.DataAnnotations;

namespace ActiveStudy.Web.Areas.Crm.Models.Classes;

public record CreateClassInputModel(
    [Required] string Label,
    int? Grade,
    string TeacherId);
