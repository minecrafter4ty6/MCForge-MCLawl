/*
Copyright (C) 2010-2013 David Mitchell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
namespace MCForge.Commands
{
    public sealed class CmdCalculate : Command
    {
        public override string name { get { return "calculate"; } }
        public override string shortcut { get { return "calc"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            if(message == "")
            {
                Help(p);
                return;
            }

            var split = message.Split(' ');
			if(split.Length < 2)
			{
				Help(p);
				return;
			}
			
			if(!ValidChar(split[0]))
			{
				Player.SendMessage(p, "Invalid number given");
				return;
			}

            double result = 0;
			float num1 = float.Parse(split[0]);
			String operation = split[1];
			
			// All 2-parameter operations go here
            
			if(split.Length == 2)
			{
				switch(operation)
				{
					case "square":
						result = num1 * num1;
						Player.SendMessage(p, "The answer: %aThe square of " + split[0] + Server.DefaultColor + " = %c" + result);
						return;
					case "root":
						result = Math.Sqrt(num1);
						Player.SendMessage(p, "The answer: %aThe root of " + split[0] + Server.DefaultColor + " = %c" + result);
						return;
					case "cube":
						result = num1 * num1 * num1;
						Player.SendMessage(p, "The answer: %aThe cube of " + split[0] + Server.DefaultColor + " = %c" + result);
						return;
					case "pi":
						result = num1 * Math.PI;
						Player.SendMessage(p, "The answer: %a" + split[0] + " x PI" + Server.DefaultColor + " = %c" + result);
						return;
					default:
						Player.SendMessage(p, "There is no such method");
						return;
				}
			}
			
			// Now we try 3-parameter methods
			
			if(split.Length == 3)
			{
				if(!ValidChar(split[2]))
				{
					Player.SendMessage(p, "Invalid number given");
					return;
				}
				
				float num2 = float.Parse(split[2]);
				
				switch(operation)
				{
					case "x":
					case "*":
						result = num1 * num2;
						Player.SendMessage(p, "The answer: %a" + split[0] + " x " + split[2] + Server.DefaultColor + " = %c" + result);
						return;
					case "+":
						result = num1 + num2;
						Player.SendMessage(p, "The answer: %a" + split[0] + " + " + split[2] + Server.DefaultColor + " = %c" + result);
						return;
					case "-":
						result = num1 - num2;
						Player.SendMessage(p, "The answer: %a" + split[0] + " - " + split[2] + Server.DefaultColor + " = %c" + result);
						return;
					case "/":
						if(num2 == 0)
						{
							Player.SendMessage(p, "Cannot divide by 0");
							return;
						}
						
						result = num1 / num2;
						Player.SendMessage(p, "The answer: %a" + split[0] + " / " + split[2] + Server.DefaultColor + " = %c" + result);
						return;
					default:
						Player.SendMessage(p, "There is no such method");
						return;
				}
			}

            // If we get here, the player did something wrong

			Help(p);

        }
        public override void Help(Player p)
        {
            //Help message
            Player.SendMessage(p, "/calculate <num1> <method> <num2> - Calculates <num1> <method> <num2>");
            Player.SendMessage(p, "methods with 3 fillins: /, x, -, +");
            Player.SendMessage(p, "/calculate <num1> <method> - Calculates <num1> <method>");
            Player.SendMessage(p, "methods with 2 fillins: square, root, pi, cube");
        }
        public static bool ValidChar(string chr)
        {
            string allowedchars = "01234567890.,";
            foreach (char ch in chr) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }
    }
}