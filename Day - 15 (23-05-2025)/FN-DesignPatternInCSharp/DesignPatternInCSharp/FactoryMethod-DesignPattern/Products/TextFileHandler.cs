using System.IO;

namespace DesignPatternInCSharp.FactoryMethod_DesignPattern
{
    public class TextFileHandler : IFileHandler
    {
        private readonly FileStream _fileStream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public TextFileHandler(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _reader = new StreamReader(_fileStream);
            _writer = new StreamWriter(_fileStream) { AutoFlush = true };
        }

        public void Write(string text)
        {
            _fileStream.Seek(0, SeekOrigin.End);
            _writer.Write(text);
        }

        public string ReadAll()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            return _reader.ReadToEnd();
        }

        public void Close()
        {
            _writer.Close();
            _reader.Close();
            _fileStream.Close();
        }
    }
}
