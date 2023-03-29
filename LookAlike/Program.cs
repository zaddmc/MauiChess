using System;

namespace LookAlike
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] myString = Console.ReadLine().Split(' ');
            Console.Clear();
            if (myString != null)
                for (int i = 0; i < myString.Length; i++)
                    if (myString[i] == "men" || myString[i] == "der" || myString[i] == "som")
                        Console.Write(", " + myString[i]);
                    else if (i != 0)
                        Console.Write(" " + myString[i]);
                    else
                        Console.Write(myString[i]);
        }
    }
}