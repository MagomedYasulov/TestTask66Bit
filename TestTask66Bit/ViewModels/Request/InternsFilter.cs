using Microsoft.AspNetCore.Mvc;

namespace TestTask66Bit.ViewModels.Request
{
    public class InternsFilter
    {
        [FromQuery]
        public int? InternshipId { get; set; }

        [FromQuery]
        public int? ProjectId { get; set; }
    }
}
