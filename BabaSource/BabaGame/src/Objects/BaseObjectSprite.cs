using BabaGame.src.Resources;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AnimationInfo = System.Collections.Generic.Dictionary<string, int[][][]>;

namespace BabaGame.src.Objects
{
    public class BaseObjectSprite : AnimatedPackedSprite
    {
        private AnimationInfo? _allAnimations = null;

        public static bool ENABLE_WOBBLE = true;

        public string Phase { get; private set; }
        private uint Index;
        private readonly string name;
        private DateTime lastUpdate;

        // takes care of the wobble
        public BaseObjectSprite(string name, string? phase=null) : base(ObjectSprites.GetTextureHandle(name))
        {
            _allAnimations = JsonValues.Animations[name];
            this.name = name;
            Phase = phase ?? _allAnimations.Keys.First();
            _updateAnimation();
            lastUpdate = DateTime.Now;
        }

        public bool HasPhase(string phase) => _allAnimations?.ContainsKey(phase) ?? false;

        public void SetPhase(string phase)
        {
            if (_allAnimations?.ContainsKey(phase) != true)
                throw new Exception($"Cannot set phase to {phase} for {name}");
            Phase = phase;
            _updateAnimation();
        }

        public void StepIndex()
        {
            Index = (Index + 1) % (uint)(_allAnimations?[Phase].Length ?? throw new Exception("what"));
            _updateAnimation();
        }

        private void _updateAnimation()
        {
            SetAnimation(_allAnimations?[Phase][Index] ?? throw new Exception());
        }

        public override void Draw()
        {
            if (ENABLE_WOBBLE && DateTime.Now - lastUpdate > TimeSpan.FromMilliseconds(300))
            {
                Step();
                lastUpdate = DateTime.Now;
            }

            base.Draw();
        }

    }
}
