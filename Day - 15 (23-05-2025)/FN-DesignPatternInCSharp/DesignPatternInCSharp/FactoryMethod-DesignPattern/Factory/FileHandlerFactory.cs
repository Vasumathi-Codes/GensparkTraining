namespace DesignPatternInCSharp.FactoryMethod_DesignPattern
{
    public static class FileHandlerFactory
    {
        public static IFileHandler CreateFileHandler(string filePath)
        {
            return new TextFileHandler(filePath);
        }
    }
}
