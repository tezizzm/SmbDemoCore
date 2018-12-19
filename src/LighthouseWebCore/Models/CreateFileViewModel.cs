using Microsoft.AspNetCore.Http;

namespace LighthouseUiCore.Models
{
    public class CreateFileViewModel
    {
        public IFormFile File { get; set; }
    }
}