using Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Events
{
    static class EventChannels
    {
        public static EventChannel<KeyEvent> KeyPress = EventManager.CreateChannel<KeyEvent>();
        public static EventChannel<MapChange> MapChange = EventManager.CreateChannel<MapChange>();
        public static EventChannel<CharacterControl> CharacterControl = EventManager.CreateChannel<CharacterControl>();
        public static EventChannel<MusicPlay> MusicPlay = EventManager.CreateChannel<MusicPlay>();
    }
}
