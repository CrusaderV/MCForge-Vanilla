/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.IO;

namespace MCForge
{
    public class CmdHide : Command
    {
        public override string name { get { return "hide"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdHide() { }

        public override void Use(Player p, string message)
        {
            if (message == "check")
            {
                if (p.hidden == true)
                {
                    Player.SendMessage(p, "You are currently hidden!");
                    return;
                }
                else
                {
                    Player.SendMessage(p, "You are not currently hidden!");
                    return;
                }
            }
            else
                if (message != "")
                    if (p.possess != "")
                    {
                        Player.SendMessage(p, "Stop your current possession first.");
                return;
            }
            Command opchat = Command.all.Find("opchat");
            p.hidden = !p.hidden;
            if (p.hidden)
            {
                Player.GlobalDie(p, true);
                Player.GlobalMessageOps("To Ops -" + p.color + p.name + "-" + Server.DefaultColor + " is now &finvisible" + Server.DefaultColor + ".");
                Player.GlobalChat(p, "&c- " + p.color + p.prefix + p.name + Server.DefaultColor + " disconnected.", false);
                if (p.opchat == false)
                {
                    opchat.Use(p, message);
                }
                else { }
                //Player.SendMessage(p, "You're now &finvisible&e.");
            }
            else
            {
                Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                Player.GlobalMessageOps("To Ops -" + p.color + p.name + "-" + Server.DefaultColor + " is now &8visible" + Server.DefaultColor + ".");
                Player.GlobalChat(p, "&a+ " + p.color + p.prefix + p.name + Server.DefaultColor + " joined the game.", false);
                if (p.opchat == true)
                {
                    opchat.Use(p, message);
                }
                else { }
                //Player.SendMessage(p, "You're now &8visible&e.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/hide - Makes yourself (in)visible to other players also turns opchat on and off.");
        }
    }
}