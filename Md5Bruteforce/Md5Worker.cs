using System.Security.Cryptography;
using System.Text;

namespace Md5Bruteforce;

public class Md5Worker : IService //Класс работующего треда, выполняет вычисления, связывается с IDirector для получения следующей парольной фразы
{
    private IDirector _md5Director;
    private Thread? _thread;
    string _alphabet;
    static byte[] _knownMd5 = MD5.HashData(Encoding.UTF8.GetBytes("12345"));
    private uint _alphabetCount;
    private void Work()
    {
        while(true)
        {
            var (number,charCount)  = _md5Director.GetCurrentNumbers();
            var currentString = GenerateStringByNumber(number, charCount);
            var md5 = MD5.HashData(Encoding.UTF8.GetBytes(currentString));
            bool found = checkBytesArrayEquals(md5, _knownMd5);
            if (found)
                _md5Director.SaveFoundData(currentString);
        }
    }

    private bool checkBytesArrayEquals(byte[] a1, byte[] a2)
    {
        if (a1 == a2)
        {
            return true;
        }
        if ((a1 != null) && (a2 != null))
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void Start()
    {
        _thread = new Thread(Work);
        _thread.Start();
    }

    public void Stop()
    {
        _thread?.Abort();
        _thread = null;
    }

    public Md5Worker(IDirector md5Director, string alpabet)
    {
        _alphabet = alpabet;
        _md5Director = md5Director;
        _alphabetCount = (uint)_alphabet.Length;
    }

    private string GenerateStringByNumber(ulong number, uint charCount)
    {
        StringBuilder sb = new StringBuilder();
        ulong tmpNumber = number;

        for(var i = 0; i < charCount; i++)
        {
            int charCode = (int)((tmpNumber % _alphabetCount));
            sb.Append(_alphabet[charCode]);
            tmpNumber = tmpNumber / _alphabetCount;
        }
        return sb.ToString();
    }
}