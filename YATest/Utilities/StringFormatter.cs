using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities
{
    class StringFormatter
    {
        public static int SetTextWidth(ref string text, SpriteFont spriteFont, float width)
        {
            // get a vector representing the size of our string
            Vector2 textSize = spriteFont.MeasureString(text);

            // if text already fits, don't do anything
            if (textSize.X < width)
                return 1;

            // remove existing newline chars in case it has already been formatted
            text = text.Replace("\n", "");
            
            // loop through all characters in string, if a character extends past our width
            float lineTotal = 0;
            float wordTotal = 0;
            int charCount = 0;
            int linesNum = 0;
            int i = 0;
            do
            {
                // if we find whitespace, we're reached the end of a word
                if (char.IsWhiteSpace(text, i))
                {
                    // measure the length of the word
                    wordTotal = spriteFont.MeasureString(text.Substring(i - charCount, charCount)).X;
                    // if the word extends past our width, we insert
                    // a \n before the word to pop it to the next line
                    if ((lineTotal + wordTotal) > width && lineTotal != 0)
                    {
                        text = text.Insert(i - charCount, "\n");
                        linesNum++;
                        lineTotal = wordTotal;
                        charCount = 0;
                    }
                    // otherwise the word fits on our current line
                    else
                    {
                        lineTotal += wordTotal + 1;
                        charCount = 0;
                        i++;
                    }
                }
                else
                {
                    charCount++;
                    i++;
                }
            } while (i < text.Length);
            return linesNum+1;
        }
    }
}
