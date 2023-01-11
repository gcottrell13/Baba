using Core.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content;

public static class PlaySound
{
    private const string soundsDirectory = ContentDirectory.contentDirectory + "/sounds/";

    private static Dictionary<string, string> files = Directory.EnumerateFiles(soundsDirectory).ToDictionary(f => f.Split('\\', '/').Last().Split('.').First());

    private static Dictionary<string, SoundEffect> sounds = new();

    public static IEnumerable<string> musicNames => files.Keys;

    private static IEnumerable<byte[]> LoadOgg(VorbisReader vorbis)
    {
        float[] buffer = new float[vorbis.SampleRate / 10 * vorbis.Channels];
        var bytes = new byte[buffer.Length * 2];
        int count;
        while ((count = vorbis.ReadSamples(buffer, 0, buffer.Length)) > 0)
        {
            var index = 0;
            for (int i = 0; i < count; i++)
            {
                int temp = (int)(short.MaxValue * buffer[i]);
                if (temp > short.MaxValue)
                {
                    temp = short.MaxValue;
                }
                else if (temp < short.MinValue)
                {
                    temp = short.MinValue;
                }
                short tempBytes = (short)temp;
                byte byte1 = (byte)((tempBytes >> 8) & 0x00FF);
                byte byte2 = (byte)((tempBytes >> 0) & 0x00FF);

                try 
                {
                    // Little endian
                    bytes[index++] = byte2;
                    bytes[index++] = byte1;
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }
            yield return bytes;
        }
    }

    private static void WriteWave(BinaryWriter writer, int channels, int rate, int dataLength)
    {
        writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
        writer.Write((int)(36 + dataLength));
        writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[4] { 'f', 'm', 't', ' ' });
        writer.Write((int)16);
        writer.Write((short)1);
        writer.Write((short)channels);
        writer.Write((int)rate);
        writer.Write((int)(rate * ((16 * channels) / 8)));
        writer.Write((short)((16 * channels) / 8));
        writer.Write((short)16);

        writer.Write(new char[4] { 'd', 'a', 't', 'a' });
        writer.Write((int)dataLength);
    }

    internal static SoundEffect LoadSound(string path)
    {
        if (path.EndsWith(".ogg"))
        {
            using var vorbis = new VorbisReader(soundsDirectory + path);
            var ogg = LoadOgg(vorbis);

            var dataLength = vorbis.SampleRate * vorbis.Channels * 2 * (int)vorbis.TotalTime.TotalSeconds;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            WriteWave(writer, vorbis.Channels, vorbis.SampleRate, dataLength);
            stream.Position = 0;
            var bytes = stream.GetBuffer().Concat(ogg.SelectMany(x => x));
            return SoundEffect.FromStream(new StreamOverEnumerable(bytes));
        }
        else
        {
            using var fileStream = File.OpenRead(soundsDirectory + path);
            var data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            using var stream = new MemoryStream(data);

            return SoundEffect.FromStream(stream);
        }
    }

    /// <summary>
    /// Plays the sound effect and returns a reference, just in case you want it.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public static SoundEffectInstance PlaySoundFile(string name, bool loop = false)
    {
        if (files.ContainsKey(name) == false)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        if (sounds.ContainsKey(name) == false)
        {
            sounds[name] = LoadSound(files[name].Split('\\', '/').Last());
        }
        var sound = sounds[name];
        var instance = sound.CreateInstance();
        instance.IsLooped = loop;
        instance.Play();
        return instance;
    }


    private static SoundEffectInstance? currentMusic = null;
    private static string currentMusicName = string.Empty;

    public static void PlayMusic(string name)
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
            currentMusic.Dispose();
        }
        if (currentMusicName == name) return;

        currentMusicName = name;
        currentMusic = PlaySoundFile(name, true);
    }
}
