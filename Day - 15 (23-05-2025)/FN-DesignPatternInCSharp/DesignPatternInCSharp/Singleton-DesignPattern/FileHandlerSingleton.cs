using System;
using System.IO;

namespace DesignPatternInCSharp.Singleton_DesignPattern
{
    internal sealed class FileHandlerSingleton
    {
        //single instance of the class (singleton).
        private static FileHandlerSingleton? _instance;
        private FileStream _fileStream;
        private StreamReader _reader;
        private StreamWriter _writer;

        // Private constructor
        //This is private to prevent creating objects from outside.
        private FileHandlerSingleton(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _reader = new StreamReader(_fileStream);
            _writer = new StreamWriter(_fileStream);
            _writer.AutoFlush = true;
        }

        //A static method returns the single instance.
        public static FileHandlerSingleton GetInstance(string filePath)
        {
            if (_instance == null)
            {
                _instance = new FileHandlerSingleton(filePath);
            }
            return _instance;
        }

        public void Write(string text)
        {
            _fileStream.Seek(0, SeekOrigin.End);  // Move to end to append
            _writer.Write(text);
        }

        public string ReadAll()
        {
            _fileStream.Seek(0, SeekOrigin.Begin); // Move to start to read whole file
            return _reader.ReadToEnd();
        }

        public void Close()
        {
            _writer.Close();
            _reader.Close();
            _fileStream.Close();
            _instance = null; // Reset instance
        }
    }
}
