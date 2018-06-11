using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using YATest.GameEngine;

namespace ParticleClassLibrary
{
    public class FireParticleSystem : ParticleSystem
    {
        public FireParticleSystem(Game game, Tint colorTint)
            : base(game, colorTint)
        { }


        protected override void InitializeSettingsForDark(ParticleSettings settings)
        {
            settings.TextureName = "fire";

            settings.MaxParticles = 2000;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.DurationRandomness = 2;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.5f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 0.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0.3f, 0.3f, 0.3f);

            settings.MinColor = new Color(255, 255, 0, 10);
            settings.MaxColor = new Color(255, 255, 0, 40);

            settings.MinStartSize = 0.1f;
            settings.MaxStartSize = 0.5f;

            settings.MinEndSize = 0.1f;
            settings.MaxEndSize = 0.5f;

            // Use additive blending.
            //settings.SourceBlend = Blend.SourceAlpha;
            //settings.DestinationBlend = Blend.One;
        }

        protected override void InitializeSettingsForLight(ParticleSettings settings)
        {
            settings.TextureName = "fire";

            settings.MaxParticles = 2000;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.DurationRandomness = 2;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.5f;

            settings.MinVerticalVelocity = -0.5f;
            settings.MaxVerticalVelocity = 0.5f;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0.0f, 0.1f, 0.0f);

            settings.MinColor = new Color(255, 0, 255, 10);
            settings.MaxColor = new Color(255, 255, 255, 40);

            settings.MinStartSize = 0.1f;
            settings.MaxStartSize = 0.5f;

            settings.MinEndSize = 0.1f;
            settings.MaxEndSize = 0.5f;

            // Use additive blending.
            //settings.SourceBlend = Blend.SourceAlpha;
            //settings.DestinationBlend = Blend.One;
        }
    }
}
