using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

#region do not open this region
/*
i told ya


                         `-:+//:::/+o++o+++/-.`                        
                    `-:+sydmmmdyssyhhdmmmddddhyso/oo/:.`              
               `.:+o+hhhhdmmmmdddmddddmmmmddmmdddmhhhhhy+-`           
             -:ohdhhdhyydmmdhdmmddddmmmmdhhddmddmmdmmdyyyys+-         
         `-+syshysydmhyyhhhhddddddhhhhhhdhdmmddmmmmhyyhhdhyyhy/`      
       `-+syyohysyddhoysosydhdhyyddmdddddhmmmddmmmdhhddmmmmmmmmhy/`   
     `-oyosyyhhsshhhsoosyhdmdyydmmmdmmmmmdmNmmmmmmhdmmmmmmmmmmNmmmy-  
    ./shyyyhhhysyhhysyddmmmdydmmmddmmmmNmmmmmmmmmdmmmmmmmmmmmmmmmmNy` 
  `-+syyhyyyhyhyhddhdddhhhdmddmmmmmmmmmmmmmmmmmmmdmmmmmmmmmmNNmNNmNd+ 
  :oshyyyyyyyhhddmmddhhddmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmNNNmNNms 
 .oyyhysyyhhhddmmmmmdmmmmmmmmmmmmmmmmmmmmmmmdddddmmmmmmmmmmmmNNNNNNd+ 
 +yyyysshhdddmdmmmmmNmmmmmmmmmmmmmmmmmmmmddhhhyyyyhhddmmmmmmmmNmmmNmy 
.yhhhhyddddmmmmmmmmNmNmmmmmmmmmmmmmmddhhysoooo++o+ossyhhdmmmmNNmmNmNd`
oddddddmdmmmmmmmmNmmmmmmmmmmmmmmmmddhyo+//://///++++ooosyhdmmNNmNNNNm-
dmmmmmmmmmmmmmmNNNNNNmmmmmmdddmmdhsso+/:::/://///+++++++oyhdmmNNNNNNm:
mmmmmmmmmmmmmmmNmNNNmmmmmdhhhhhddo++//:::::://///++++++osyhhddmNmNNNm:
mmmmmmmmmmNmmmNNmmmmmmmddhs+++oyy+///:::::::////+++++ooosyhhhhdmNNNNm:
dmmmmmmmNmmmmmNNmmmmmmmdddds+//+os/+::--::://+++++++++oossyyyhhdmNNNm`
ymmmmmNNmmmmmmmmdddddddhs+oos+:::/+/:---:://////+///+++oossyyyhhmmNNm`
+mmmmmNNNNmmmmmmmddhy+++/::-::------:---::///////::////+oossyyhhmmNNd 
-dNmmNmmmmmmmmmmmdhs+:::-------------::://+++++///://++oossyyyhhdmNNh 
 ymNmmNNmmmmmmmmho+::------..--::/++ossssssoooo++ooossyyyhhhhhhhhmNms 
 -hNNmNmNmmmmmds/::--------::/oyyhdmmmmmddhysoossyhddmmdmmmmmdmmddmd:  KAPPA !
 `/shmNmmmmmmmo/:----:/+oosyhhdddmmmmmmmmmdhsooyhdmmmmmmmmmmmmmmmdmy  
 :///sdNmmmmmh------:+oyhddmddmmmmmmmmmmmmdho//ohmmmmNNNmmmmmmmmddds  
 ohdyssdmmmmdo------:+oyhddddddddddddddhyyyo/-.-+mmmmmmmmmmmmmmdhhy+  
 :ss+/+shmmmdo-----..--:/++osyyhhhhdddhyo++/:-..-yddddddddddddhhyyo-  
 ./+//oddyhdh+:----.......--:/+oosssso+////::-..-+hdhhhhhhhhyyyysy+.  
 `:::+yoo:-/+:::----...`````..--::::---:::-:--.`./shhyssssssoosssyo.  
  `-::+:/-..--::::----.....```........-:::----.``-/syyso++oo+osyydo.  
   `-:-..:-.-::::::-:-----.........--://::----.`.-:oyhysooooosshhdo`  
    `.......-:::::::::::::::::::::/++o+:.....-...:/ossyyyyssyyyddmo`  
     ````..-::::::::::::////////+osso++++so++////ooyhhhhhhhhhhhddd+   
      .-...-::::::::::///+++++oosss+///+sydddddhhhddmddhhhdhhddddd/   
      `/////:/::::::://///++oossso+::::://+osyhdmmmmmdddhhhhhdddmy:   
        `..../+//::::///////+oo+///:////++ooosyhddmmddddhhhhhhhdm+.   
             `o+///////////::////++oooooooossyhhhddddddddhysyhhdd:    
              ./+++/+++/////:::/+syyyhhyyhhdddddmmmmmmmdhysyhddm+`    
               `-+oooo++++++++/+++/::///+++ossyyhhhdhhhyyyhhddmy.     
                 `/ssssosssssssso/:---::/+osyyyyhhhhhyyyddddmdy`      
                   .shyyyyyhhhhys+/:://+oosyhhddddhhhhhhddmmh+        
                    `+yhhhhhhhyyo+++/+oossssyyhhddhhhhhddmmy/         
                      .:shhhyyssoo+++++++ooooossyyyysyhdmd+.          
                        `-ohhhyyssso++//::://+ooosssyhdm+`            
                           `+shhhhyhysssooossyyyyhhhdmd-              
                              ./shhddddddddddddddmmmmy-               
                                 `-/syhhyhyhhhhyyo+/.`         
             
*/

#endregion

namespace BrainDotExe.Common
{
    public static class Utility
    {
        /// <summary>
        ///     Checks if the unit is a Hero or Champion
        /// </summary>
        public static bool IsChampion(this Obj_AI_Base unit)
        {
            var hero = unit as AIHeroClient;
            return hero != null && hero.IsValid;
        }

        public static bool InFountain(Vector2 postion)
        {
            float fountainRange = 562500; //750 * 750
            var map = Map.GetMap();
            if (map != null && map.Type == Map.MapType.SummonersRift)
            {
                fountainRange = 1102500; //1050 * 1050
            }
            return ObjectManager.Get<Obj_SpawnPoint>()
                       .Any(sp => postion.Distance(sp.Position, true) < fountainRange);
        }

        public static bool IsChampion(this Obj_AI_Base unit, string championName)
        {
            var hero = unit as AIHeroClient;
            return hero != null && hero.IsValid && hero.ChampionName.Equals(championName);
        }

        public static bool IsRecalling(this AIHeroClient unit)
        {
            return unit.Buffs.Any(buff => buff.Name.ToLower().Contains("recall") && buff.Type == BuffType.Aura);
        }

        public static bool IsOnScreen(this Vector3 position)
        {
            var pos = Drawing.WorldToScreen(position);
            return pos.X > 0 && pos.X <= Drawing.Width && pos.Y > 0 && pos.Y <= Drawing.Height;
        }

        public static bool IsOnScreen(this Vector2 position)
        {
            return position.To3D().IsOnScreen();
        }

        public static Vector3 Randomize(this Vector3 position, int min, int max)
        {
            var ran = new Random(Environment.TickCount);
            return position + new Vector2(ran.Next(min, max), ran.Next(min, max)).To3D();
        }

        public static Vector2 Randomize(this Vector2 position, int min, int max)
        {
            return position.To3D().Randomize(min, max).To2D();
        }

        public static bool IsWall(this Vector3 position)
        {
            return NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall);
        }

        public static bool IsWall(this Vector2 position)
        {
            return position.To3D().IsWall();
        }

       public static int GetRecallTime(AIHeroClient obj)
        {
            return GetRecallTime(obj.Spellbook.GetSpell(SpellSlot.Recall).Name);
        }

        public static int GetRecallTime(string recallName)
        {
            var duration = 0;

            switch (recallName.ToLower())
            {
                case "recall":
                    duration = 8000;
                    break;
                case "recallimproved":
                    duration = 7000;
                    break;
                case "odinrecall":
                    duration = 4500;
                    break;
                case "odinrecallimproved":
                    duration = 4000;
                    break;
                case "superrecall":
                    duration = 4000;
                    break;
                case "superrecallimproved":
                    duration = 4000;
                    break;
            }
            return duration;
        }

        public static short GetPacketId(this GamePacketEventArgs gamePacketEventArgs)
        {
            var packetData = gamePacketEventArgs.PacketData;
            if (packetData.Length < 2)
            {
                return 0;
            }
            return (short)(packetData[0] + packetData[1] * 256);
        }

        public static void SendAsPacket(this byte[] packetData,
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags protocolFlags = PacketProtocolFlags.Reliable)
        {
            Game.SendPacket(packetData, channel, protocolFlags);
        }

        public static void ProcessAsPacket(this byte[] packetData, PacketChannel channel = PacketChannel.S2C)
        {
            Game.ProcessPacket(packetData, channel);
        }

        public class Map
        {
            public enum MapType
            {
                Unknown,
                SummonersRift,
                CrystalScar,
                TwistedTreeline,
                HowlingAbyss
            }

            private static readonly IDictionary<int, Map> MapById = new Dictionary<int, Map>
            {
                {
                    8,
                    new Map
                    {
                        Name = "The Crystal Scar",
                        ShortName = "crystalScar",
                        Type = MapType.CrystalScar,
                        Grid = new Vector2(13894f / 2, 13218f / 2),
                        StartingLevel = 3
                    }
                },
                {
                    10,
                    new Map
                    {
                        Name = "The Twisted Treeline",
                        ShortName = "twistedTreeline",
                        Type = MapType.TwistedTreeline,
                        Grid = new Vector2(15436f / 2, 14474f / 2),
                        StartingLevel = 1
                    }
                },
                {
                    11,
                    new Map
                    {
                        Name = "Summoner's Rift",
                        ShortName = "summonerRift",
                        Type = MapType.SummonersRift,
                        Grid = new Vector2(13982f / 2, 14446f / 2),
                        StartingLevel = 1
                    }
                },
                {
                    12,
                    new Map
                    {
                        Name = "Howling Abyss",
                        ShortName = "howlingAbyss",
                        Type = MapType.HowlingAbyss,
                        Grid = new Vector2(13120f / 2, 12618f / 2),
                        StartingLevel = 3
                    }
                }
            };

            public MapType Type { get; private set; }
            public Vector2 Grid { get; private set; }
            public string Name { get; private set; }
            public string ShortName { get; private set; }
            public int StartingLevel { get; private set; }

            /// <summary>
            ///     Returns the current map.
            /// </summary>
            public static Map GetMap()
            {
                if (MapById.ContainsKey((int)Game.MapId))
                {
                    return MapById[(int)Game.MapId];
                }

                return new Map
                {
                    Name = "Unknown",
                    ShortName = "unknown",
                    Type = MapType.Unknown,
                    Grid = new Vector2(0, 0),
                    StartingLevel = 1
                };
            }
        }

        /// <summary>
        ///     Internal class used to get the waypoints even when the enemy enters the fow of war.
        /// </summary>
        internal static class WaypointTracker
        {
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();
        }
    }
}
