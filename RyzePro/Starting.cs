using EloBuddy;

namespace RyzePro
{
    class Starting
    {
        public static AIHeroClient Ryze { get { return ObjectManager.Player; } }
        public static int PassiveCount {get { return Player.Instance.GetBuffCount("ryzepassivestack"); }}
        public static bool PassiveCharged { get { return Player.Instance.HasBuff("ryzepassivecharged"); } }
    }
}
