using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternInCSharp.AbstractFactory_DesignPattern
{
    // Concrete Factory - JSON
    public class JsonFileFactory : IFileHandlerFactory
    {
        public IFileHandler CreateHandler(string path) => new JsonFileHandler(path);
    }
}
