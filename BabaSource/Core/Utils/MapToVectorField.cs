using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Core.Utils
{
    public static class MapToVectorField
    {
        static IEnumerable<int> range(int radius)
        {
            for(var i = -radius; i <= radius; i++)
            {
                yield return i;
            }
        }

        public static Vector3[] ToVectorField(System.Drawing.Color[] PathData, int mapWidth, int mapHeight, int radius)
        {
            byte GetDataFromPoint(int x, int y)
            {
                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
                {
                    return 0;
                }
                return PathData[x + y * mapWidth].A;
            }

            var deltas = range(radius).SelectMany(x => range(radius).Select(y => new Vector2(x, y))).ToList();

            var timer = new Stopwatch();
            timer.Start();

            var emptyWall = new Vector3(0, 0, 1);

            IEnumerable<Vector3> GetVectors()
            {
                long c = 0;
                foreach(var item in PathData)
                {
                    if (c % mapWidth == mapWidth-1)
                    {
                        Console.WriteLine($"elapsed: {timer.ElapsedMilliseconds / 1000}s");
                        var ttc = (((float)mapHeight*mapWidth / c) - 1) * timer.ElapsedMilliseconds / 1000;
                        Console.WriteLine($"{(float)c * 100 / (mapHeight*mapWidth)}% Estimated time to completion: {ttc}s");
                    }

                    if (PathData[c].A > 10)
                    {
                        var x = c % mapWidth;
                        var y = c / mapWidth;

                        var dirs = deltas.Where(d => GetDataFromPoint((int)d.X + (int)x, (int)d.Y + (int)y) == 0).ToList();
                        if (dirs.Count == 0)
                        {
                            yield return emptyWall;
                        }
                        else
                        {
                            var dir = dirs.Aggregate((v1, v2) => v1 + v2);
                            if (dir != Vector2.Zero)
                                dir.Normalize();
                            yield return new Vector3(dir, emptyWall.Z);
                        }
                    }
                    else
                    {
                        yield return Vector3.Zero;
                    }

                    c++;
                }
            }

            return GetVectors().ToArray();
        }
    }
}
