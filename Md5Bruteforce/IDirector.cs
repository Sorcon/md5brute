namespace Md5Bruteforce;

public interface IDirector
{
    public (ulong number, uint charCount) GetCurrentNumbers();
    public void SaveFoundData(string data);
}