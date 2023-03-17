namespace Md5Bruteforce
{
    //Программа не имеет серьёхного использования, перебирает наборы символов из словаря по порядку и пытается найти известный мд5
    internal class Program
    {
        static void Main(string[] args)
        {
            IService myDirector = new Md5Director();
            myDirector.Start();
            Console.ReadLine();
            myDirector.Stop();
        }
    }
}