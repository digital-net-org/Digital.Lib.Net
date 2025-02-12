using System.ComponentModel.DataAnnotations;

namespace Digital.Lib.Net.Authentication.Models.Events;

public enum ApiEventState
{
    [Display(Name = "Failed")]
    Failed,
    [Display(Name = "Success")]
    Success,
    [Display(Name = "Pending")]
    Pending
}