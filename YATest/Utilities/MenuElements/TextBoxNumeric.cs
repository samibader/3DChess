using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities.MenuElements
{
    
    class TextBoxNumeric : Textbox
    {
        private int maxInt;
        public TextBoxNumeric(Game game, int x, int y, int width, int height, int maxInt, string defaultText, SpriteFont font)
            : base(game, x, y, width, height, defaultText, font)
        {
            this.maxInt = maxInt;
        }
        public override void HandleKeyboardInput()
        {
            if (isFocused == true)
            {
                curKeyState1 = Keyboard.GetState();
                //Handle backspace
                if (curKeyState1.IsKeyDown(Keys.Back) && oldKeyState1.IsKeyUp(Keys.Back) && text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
                else
                {
                    if (text.Length < MAX_CHARS)
                    {
                        //Handle digits from keyboard
                        for (int i = 48; i <= 56; i++)
                            if (curKeyState1.IsKeyDown((Keys)(i)) && oldKeyState1.IsKeyUp((Keys)(i)))
                            {
                                Text += Convert.ToChar(i);
                            }
                        //Handle digits from keypad
                        for (int i = 96; i <= 105; i++)
                            if (curKeyState1.IsKeyDown((Keys)(i)) && oldKeyState1.IsKeyUp((Keys)(i)))
                            {
                                Text += i - 96;
                            }
                        if (curKeyState1.IsKeyDown(Keys.Enter) && oldKeyState1.IsKeyDown(Keys.Enter))
                        {
                            if (text == "")
                                text = defaultText;
                            LostFocus();
                        }
                    }
                    if(Text != "")
                        if (Convert.ToInt32(Text) > maxInt)
                            Text = defaultText;
                }
                oldKeyState1 = curKeyState1;
            }
        }

    }
}
