using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baba
{
    class CommandLineArguments
    {
        public static CommandLineArguments? Args { get; set; }

        [Option("connect")]
        public string? Connect { get; set; }

        [Option("port")]
        public int Port { get; set; }
    }
}
