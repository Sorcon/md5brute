namespace Md5Bruteforce;

public class Md5Director : IDirector, IService //Класс, управляющий Worker`ами, все статические данные в реальной программе нужно запрашивать у пользователя и/или хранить в конфиге
{
    private string _alphabet = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
    private int _alphabetCount;

    private static int _workersCount = 8;
    private readonly Object _numberMutex = new Object();
    private readonly Object _listMutex = new Object();
    private ulong _currentNumber = 0; //Для перебора реально больших парольных фраз придется изменять механизм учета текущей фразы, ulong тянет только 8 байт, что больше 8 символов и для демонстрационных целей достаточно
    private List<string> _foundData = new List<string>();
    private List<IService> _workers = new List<IService>();
    private uint _currentCharCount = 1;

    public Md5Director()
    {
        _alphabetCount = _alphabet.Length;
        for (int i = 0; i < _workersCount; i++)
        {
            _workers.Add(new Md5Worker(this, _alphabet));
        }
    }


    public (ulong number, uint charCount) GetCurrentNumbers()
    {
        lock(_numberMutex)
        {
            _currentNumber++;
            if (_currentNumber >= Math.Pow(_alphabetCount, _currentCharCount))
            {
                _currentNumber = 0;
                _currentCharCount++;
                Console.WriteLine($"Current number of chars: {_currentCharCount}");
            }
            return (_currentNumber, _currentCharCount);
        }
    }

    public void SaveFoundData(string data)
    {
        lock (_listMutex)
        {
            _foundData.Add(data);
            Console.WriteLine("Found data: \"" + data + "\"");
        }
    }

    public void Start()
    {
        foreach (var service in _workers)
        {
            service.Start();
        }
    }

    public void Stop()
    {
        foreach (var service in _workers)
        {
            service.Stop();
        }
    }
}