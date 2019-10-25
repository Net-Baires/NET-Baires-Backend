using System.IO;
using System.Threading.Tasks;

namespace NetBaires.Api.Services
{
    public interface IFilesServices
    {
        Task<FileDetail> UploadAsync(Stream file, string fileName, Container container);
        Task<bool> DeleteAsync(string fileName, Container container);
        Task<Stream> GetAsync(string fileName, Container container);
    }
}
