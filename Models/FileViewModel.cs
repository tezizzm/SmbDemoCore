using System.ComponentModel;

namespace LighthouseUiCore.Models
{
    public class FileViewModel
    {
        public string Directory { get; set; }

        [DisplayName("KB")]
        public long Length { get; set; }

        public string Name  { get; set; }
    }
}