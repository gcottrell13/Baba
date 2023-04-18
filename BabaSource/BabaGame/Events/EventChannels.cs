using Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.Events;

static class EventChannels
{
    public static EventChannel<MapChange> MapChange = EventManager.CreateChannel<MapChange>();
    public static EventChannel<MusicPlay> MusicPlay = EventManager.CreateChannel<MusicPlay>();
}
