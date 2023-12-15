using System.Drawing;

namespace BulkyBook.PL.Helpers
{
    public static class DocumetSetting
    {
        public static string UploadDocument(IFormFile file , string  FolderName)
        {
            //1- Get Located Folder Path to save in.

            //string FolderPath = "E:\\1-(EraaSoft)\\.NetCore Mvc  (.net6)\\BulkyBook\\BulkyBook.PL\\wwwroot\\Files\\Images\\";
            //string FolderPath = Directory.GetCurrentDirectory()+"\\wwwroot\\Files\\"+FolderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            // 2- get file name and make it unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3- get File path
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4- Save file As Stream (Stream => is A data per time)
            using var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);

            return FileName;


        }

        //In this example, we load an image from a file named image.jpg.
        //We then convert the image to a byte array and create a new MemoryStream from the byte array.
        //Finally, we create a new FormFile from the MemoryStream and return it as an IFormFile.

        public static IFormFile GetFile(string FolderName , string FileName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName,FileName);
            // Load the image from file or database
            Image image = Image.FromFile(FilePath);

            // Convert the image to a byte array
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }

            // Create a new MemoryStream from the byte array
            MemoryStream stream = new MemoryStream(bytes);

            // Create a new FormFile from the MemoryStream
            IFormFile file = new FormFile(stream, 0, bytes.Length,FileName, FileName);

            return file;


        }


        public static string ImagePath(string fileName, string FolderNAme)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", FolderNAme, fileName);
        }

        public static void DeleteFile(string fileName, String FolderNAme)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", FolderNAme, fileName);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
