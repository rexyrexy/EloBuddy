using System.Linq;
using EloBuddy;

namespace RyzePro
{
    class Starting
    {
        public static AIHeroClient Ryze { get { return ObjectManager.Player; } }
        public static int StackPassive { get { return Ryze.Buffs.Count(buf => buf.Name == "RyzePassiveStack"); } }
    }
}
