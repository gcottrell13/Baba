using Core.Content;
using Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.Events;

static class EventChannels
{
    public static EventChannel<MapChange> MapChange = EventManager.CreateChannel<MapChange>();
    public static EventChannel<MusicPlay> MusicPlay = EventManager.CreateChannel<MusicPlay>();
    public static EventChannel<MusicPlay> SoundPlay = EventManager.CreateChannel<MusicPlay>();
    public static EventChannel<int> SaveGame = EventManager.CreateChannel<int>();
    public static EventChannel<bool> CharacterControl = EventManager.CreateChannel<bool>();
    public static EventChannel<(ObjectTypeId obj, int count)> AddItemsToInventory = EventManager.CreateChannel<(ObjectTypeId obj, int count)>();
}
