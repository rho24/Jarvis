using System;

namespace Jarvis
{
    internal class Program
    {
        private static void Main(string[] args) {
            string input;
            while ((input = Console.ReadLine()) != "") {
                try {
                    var cmdStarter = new ProcessStarter(input);
                    cmdStarter.Execute();
                }
                catch (Exception ex) {
                    Console.WriteLine("Exception: {0}", ex);
                }
            }
        }
    }
}