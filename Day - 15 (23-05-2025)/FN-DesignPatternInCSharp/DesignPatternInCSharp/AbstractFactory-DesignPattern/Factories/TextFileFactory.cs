using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternInCSharp.AbstractFactory_DesignPattern
{
    public class TextFileFactory : IFileHandlerFactory
    {
        public IFileHandler CreateHandler(string path) => new TextFileHandler(path);
    }

   
}
