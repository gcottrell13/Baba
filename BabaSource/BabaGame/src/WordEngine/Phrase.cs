using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.WordEngine
{
    public class Phrase
    {
        private readonly IEnumerable<string> words;

        public Phrase(IEnumerable<string> words)
        {
            this.words = words;
        }
    }
}
