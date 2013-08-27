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
	public sealed class CmdDraw : Command
	{
		public override string name { get { return "draw"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "build"; } }
		public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
		public CmdDraw() { }

		public override void Use(Player p, string message)
		{
			int height;
			int radius;

			if (p != null)
			{
				if (p.level.permissionbuild > p.group.Permission)
				{
					p.SendMessage("You can not edit this map.");
					return;
				}
				string[] message2 = message.Split(' ');

				#region cones
				if (message2[0].ToLower() == "cone")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 1)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeCone);

					return;
				}
				if (message2[0].ToLower() == "hcone")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 1)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeHCone);

					return;
				}

				if (message2[0].ToLower() == "icone")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 1)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeICone);

					return;
				}
				if (message2[0].ToLower() == "hicone")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 1)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeHICone);

					return;
				}
				#endregion
				#region pyramids
				if (message2[0].ToLower() == "pyramid")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 2)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangePyramid);

					return;
				}
				if (message2[0].ToLower() == "hpyramid")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 2)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeHPyramid);

					return;
				}
				if (message2[0].ToLower() == "ipyramid")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 2)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeIPyramid);

					return;
				}
				if (message2[0].ToLower() == "hipyramid")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 2)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeHIPyramid);

					return;
				}
				#endregion
				#region spheres
				if (message2[0].ToLower() == "sphere")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 3)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 3)).name + "+"); return; }
					if (message2.Length != 2)
						goto Help;

					try
					{
						radius = Convert.ToUInt16(message2[1].Trim());
						p.BcVar = new int[2] { 0, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeSphere);

					return;
				}
				if (message2[0].ToLower() == "hsphere")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 3)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 3)).name + "+"); return; }
					if (message2.Length != 2)
						goto Help;

					try
					{
						radius = Convert.ToUInt16(message2[1].Trim());
						p.BcVar = new int[2] { 0, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeHSphere);

					return;
				}
				#endregion
				#region other
				if (message2[0].ToLower() == "volcano")
				{
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this, 4)) { Player.SendMessage(p, "That commands addition is for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 4)).name + "+"); return; }
					if (message2.Length != 3)
						goto Help;

					try
					{
						height = Convert.ToUInt16(message2[1].Trim());
						radius = Convert.ToUInt16(message2[2].Trim());
						p.BcVar = new int[2] { height, radius };
					}
					catch
					{
						goto Help;
					}

					p.SendMessage("Place a block");
					p.ClearBlockchange();
					p.Blockchange += new Player.BlockchangeEventHandler(BlockchangeVolcano);

					return;
				}
				#endregion

			Help:
				Help(p);
				return;

			}
			Player.SendMessage(p, "This command can only be used in-game!");
		}
		public override void Help(Player p)
		{
			p.SendMessage("/draw <shape> <height> <baseradius> - Draw an object in game- Valid Types cones, spheres, and pyramids, hspheres (hollow sphere), and hpyramids (hollow pyramid)");
		}

		#region Cone Blockchanges
		public void BlockchangeCone(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.Cone(p, x, y, z, height, radius, type);
		}
		public void BlockchangeHCone(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.HCone(p, x, y, z, height, radius, type);
		}
		public void BlockchangeICone(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.ICone(p, x, y, z, height, radius, type);
		}
		public void BlockchangeHICone(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.HICone(p, x, y, z, height, radius, type);
		}
		#endregion
		#region Pyramid Blockchanges
		public void BlockchangePyramid(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.Pyramid(p, x, y, z, height, radius, type);
		}
		public void BlockchangeHPyramid(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.HPyramid(p, x, y, z, height, radius, type);
		}
		public void BlockchangeIPyramid(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.IPyramid(p, x, y, z, height, radius, type);
		}
		public void BlockchangeHIPyramid(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.HIPyramid(p, x, y, z, height, radius, type);
		}
		#endregion
		#region Sphere Blockchanges
		public void BlockchangeSphere(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.Sphere(p, x, y, z, radius, type);
		}
		public void BlockchangeHSphere(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.HSphere(p, x, y, z, radius, type);
		}
		#endregion
		#region Special Blockchanges
		public void BlockchangeVolcano(Player p, ushort x, ushort y, ushort z, byte type)
		{
			int height = p.BcVar[0];
			int radius = p.BcVar[1];

			byte b = p.level.GetTile(x, y, z);
			p.SendBlockchange(x, y, z, b);
			p.ClearBlockchange();
			Util.SCOGenerator.Volcano(p, x, y, z, height, radius);
		}
		#endregion
	}
}