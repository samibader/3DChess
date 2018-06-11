using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities
{
    public interface IControllable
    {
        void HandleKeyboardInput();
        void HandleMouseInput();
        bool Blocked{ get; set; }
    }
}
