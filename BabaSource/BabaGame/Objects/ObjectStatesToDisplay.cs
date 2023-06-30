using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Objects;

public enum ObjectStatesToDisplay
{

    /// <summary>
    /// display the sleeping animation
    /// </summary>
    Sleep = 0x1,

    /// <summary>
    /// increase the sprite size
    /// </summary>
    Big = 0x2,

    /// <summary>
    /// particle effect
    /// </summary>
    Best = 0x4,

    /// <summary>
    /// particle effect
    /// </summary>
    Broken = 0x8,

    /// <summary>
    /// particle effect
    /// </summary>
    Party = 0x10,

    /// <summary>
    /// particle effect
    /// </summary>
    Pet = 0x20,

    /// <summary>
    /// lower sprite opacity
    /// </summary>
    Phantom = 0x40,

    /// <summary>
    /// particle effect
    /// </summary>
    Powered = 0x80,

    /// <summary>
    /// particle effect
    /// </summary>
    Powered2 = 0x100,

    /// <summary>
    /// particle effect
    /// </summary>
    Powered3 = 0x200,

    /// <summary>
    /// particle effect
    /// </summary>
    Sad = 0x400,

    /// <summary>
    /// particle effect
    /// </summary>
    Tele = 0x800,

    /// <summary>
    /// particle effect
    /// </summary>
    Win = 0x1000,

    /// <summary>
    /// animate a floating effect
    /// </summary>
    Float = 0x2000,

    /// <summary>
    /// sprite opacity = 0
    /// </summary>
    Hidden = 0x4000,
}
