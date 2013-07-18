using System;
using System.Diagnostics;

namespace Jarvis.Core.Infrastructure
{
    public class ProcessStarter
    {
        private readonly string _input;
        readonly bool _asAdmin;

        public ProcessStarter(string input)
        : this(input, false){}

        public ProcessStarter(string input, bool asAdmin) {
            _input = input;
            _asAdmin = asAdmin;
        }

        public void Execute() {
            var process = new Process {
                StartInfo = {
                    FileName = _input,
                    Verb = _asAdmin ? "runas" : null
                }
            };

            process.Start();
        }
    }
}