using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;

public record class PositionData(int PositionId, short ScreenId, short ScreenDisplayX, short ScreenDisplayY, int north, int south, int east, int west)
{
    /// <summary>
    /// globally unique position id
    /// </summary>
    /// <summary>
    /// the screen that this position belongs to, used for rendering and application of rules
    /// </summary>
    /// <summary>
    /// a coordinate relative to the screen
    /// </summary>

    public override string ToString()
    {
        return $$"""({{PositionId}}, {{ScreenId}}, {{ScreenDisplayX}}, {{ScreenDisplayY}}, {{north}}, {{south}}, {{east}}, {{west}})""";
    }

    public static implicit operator PositionData((int p, short i, short x, short y, int n, int s, int e, int w) t)
    {
        return new PositionData(t.p, t.i, t.x, t.y, t.n, t.s, t.e, t.w);
    }
}
