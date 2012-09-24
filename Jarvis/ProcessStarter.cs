using System.Diagnostics;

namespace Jarvis
{
    internal class ProcessStarter
    {
        private readonly string _input;

        public ProcessStarter(string input) {
            _input = input;
        }

        public void Execute() {
            var process = new Process {
                StartInfo = {
                    FileName = _input
                }
            };

            process.Start();
        }
    }
}