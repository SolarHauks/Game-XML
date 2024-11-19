using System;

AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);
using var game = new JeuVideo.Game1();
game.Run();