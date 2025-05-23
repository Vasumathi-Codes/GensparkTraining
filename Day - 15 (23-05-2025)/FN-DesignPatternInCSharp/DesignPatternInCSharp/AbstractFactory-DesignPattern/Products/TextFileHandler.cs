using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternInCSharp.AbstractFactory_DesignPattern
{
    public class TextFileHandler : IFileHandler
    {
        private StreamWriter _writer;
        private StreamReader _reader;
        private FileStream _stream;
        private string _path;

        public TextFileHandler(string path)
        {
            _path = path;
            _stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream) { AutoFlush = true };
        }

        public void Write(string content)
        {
            _stream.Seek(0, SeekOrigin.End);
            _writer.WriteLine(content);
        }

        public string Read()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            return _reader.ReadToEnd();
        }

        public void Close()
        {
            _writer.Close();
            _reader.Close();
            _stream.Close();
        }
    }

}
