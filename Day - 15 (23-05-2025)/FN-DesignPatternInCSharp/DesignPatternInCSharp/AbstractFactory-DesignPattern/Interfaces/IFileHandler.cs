public interface IFileHandler
{
    void Write(string content);
    string Read();
    void Close();
}