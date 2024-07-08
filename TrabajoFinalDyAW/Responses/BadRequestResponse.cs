using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalDyAW.Responses
{
    public class BadRequestResponse
    {
        [Required]
        public string error {  get; set; }
    }
}
