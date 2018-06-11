using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YATest.GameEngine
{
    abstract class AbstractBorderTextureFactory
    {
        public abstract AbstractBorderTexture CreateBorderTexture(float thickness, float width, float bwidth, float height);
    }
}
