namespace Diddi.Helpers
{
    public class FileHelper
    {
        public static async Task<byte[]?> ConvertToByteArrayAsync(IFormFile? file)
        {
            if (file == null) return null;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
