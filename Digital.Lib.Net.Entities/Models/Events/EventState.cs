using System.ComponentModel.DataAnnotations;

namespace Digital.Lib.Net.Entities.Models.Events;

public enum EventState
{
    [Display(Name = "Failed")]
    Failed,

    [Display(Name = "Success")]
    Success,

    [Display(Name = "Pending")]
    Pending
}
