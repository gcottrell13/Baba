using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Events
{
    public struct TextInput
    {
        public char Character;

        public void Deconstruct(out char c) => c = Character;
    }
}
