namespace DesignPatternInCSharp.FactoryMethod_DesignPattern
{
    public interface IFileHandler
    {
        void Write(string text);
        string ReadAll();
        void Close();
    }
}
