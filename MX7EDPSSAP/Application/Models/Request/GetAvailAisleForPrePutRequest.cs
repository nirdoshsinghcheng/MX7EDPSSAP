using MX7EDPSSAP.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace MX7EDPSSAP.Application.Models.Request
{
    public class GetAvailAisleForPrePutRequest
    {
        [Required]
        [StringLength(15)]
        [CustomizedModel(Name = "[SKU Code]")]
        public string SkuCode { get; set; }
    }
}