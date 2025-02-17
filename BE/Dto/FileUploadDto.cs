using Microsoft.AspNetCore.Http;

namespace BE.Dto
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
}
}
