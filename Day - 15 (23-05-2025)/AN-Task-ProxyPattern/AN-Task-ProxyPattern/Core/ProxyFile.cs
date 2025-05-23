using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AN_Task_ProxyPattern.Interfaces;
using AN_Task_ProxyPattern.Models;

namespace AN_Task_ProxyPattern.Core
{
    public class ProxyFile : IFile
    {
        private File _realFile;
        private string _filePath;
        private User _user;

        public ProxyFile(string filePath, User user)
        {
            _filePath = filePath;
            _realFile = new File(filePath);
            _user = user;
        }

        public void Read()
        {
            switch (_user.Role.ToLower())
            {
                case "admin":
                    _realFile.Read();
                    break;

                case "user":
                    ShowFileMetadata(_filePath);
                    break;

                case "guest":
                default:
                    Console.WriteLine("[Access Denied for GUEST user] You do not have permission to read this file.");
                    break;
            }
        }

        public void ShowFileMetadata(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                {
                    Console.WriteLine($"[Error] File '{fileInfo.Name}' does not exist.");
                    return;
                }

                Console.WriteLine("[Limited Access] File Metadata:");
                Console.WriteLine($"File Name: {fileInfo.Name}");
                Console.WriteLine($"Size: {fileInfo.Length / 1024.0:F2} KB");
                Console.WriteLine($"Created On: {fileInfo.CreationTime}");
                Console.WriteLine($"Last Modified: {fileInfo.LastWriteTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Unable to read file metadata: {ex.Message}");
            }
        }

    }

}
