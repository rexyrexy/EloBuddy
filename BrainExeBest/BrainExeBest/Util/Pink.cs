using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using ItemData = EloBuddy.ItemData;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;


namespace BrainDotExe.Util
{
    internal class Pink
    {
        public static Menu PinkInviMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        /*
        Aqui começa a magica!
        */
        static int Delay = 0; //delay para o spellcast não bugar 
        static float VayneBuffEndTime = 0; // tempo de acabar a ult da vayne

        public static void Init()
        {
            PinkInviMenu = Program.Menu.AddSubMenu("Pink Ward", "pinkward");
            PinkInviMenu.AddGroupLabel("Pink/Trinket Invisible Menu");
            PinkInviMenu.AddSeparator();
            PinkInviMenu.Add("usePink", new CheckBox("Activate Pink/Trinker Usage"));
            PinkInviMenu.AddSeparator();
            // TODO adicionar os checks marotos das spells
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            GameObject.OnCreate += OnCreate;
            Game.OnUpdate += Game_OnUpdate;
        }

        #region SpellS "DB"
        private static void OnProcessSpellCast(Obj_AI_Base enemy, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Misc.isChecked(PinkInviMenu, "usePink")) return;
            try
            {
                if (enemy.Type == GameObjectType.AIHeroClient && enemy.IsEnemy)
                {
                    // checka as magias !!!!!!!!!!
                    if (args.SData.Name == "akalismokebomb" ||
                        args.SData.Name == "deceive" ||
                        args.SData.Name == "khazixr" ||
                        args.SData.Name == "khazixrlong" ||
                        args.SData.Name == "talonshadowassault" ||
                        args.SData.Name == "monkeykingdecoy" ||
                        args.SData.Name == "hideinshadows" ||
                        args.SData.Name == "vaynetumble")
                    {
                        if (CheckSlot() == SpellSlot.Unknown) return;
                        if (_Player.Distance(enemy.Position) > 800) return;
                        if (args.SData.Name.ToLower().Contains("vaynetumble") && Game.Time > VayneBuffEndTime) return;
                        if (Environment.TickCount - Delay > 1500 || Delay == 0)
                        {
                            var pos = _Player.Distance(args.End) > 600 ? _Player.Position : args.End;
                            _Player.Spellbook.CastSpell(CheckSlot(), pos);
                            Delay = Environment.TickCount;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Deu erro '{0}'", e);
            }
        }
        #endregion

        #region Rengar/LB
        private static void OnCreate(GameObject sender, EventArgs args)
        {
            if (!Misc.isChecked(PinkInviMenu, "usePink")) return;
            // Rengar ! _ !
            var Rengar = HeroManager.Enemies.Find(x => x.ChampionName.ToLower() == "rengar");
            if (Rengar != null)
            {
                if (sender.IsEnemy && sender.Name.Contains("Rengar_Base_R_Alert"))
                {
                    if (_Player.HasBuff("rengarralertsound") &&
                        !Rengar.IsVisible &&
                        !Rengar.IsDead &&
                        CheckSlot() != SpellSlot.Unknown)
                    {
                        _Player.Spellbook.CastSpell(CheckSlot(), _Player.Position);
                    }
                }
            }

            // Le Blanc
            var LeBlank = HeroManager.Enemies.Find(x => x.ChampionName.ToLower() == "leblanc");
            if (LeBlank != null)
            {
                if (_Player.Distance(sender.Position) > 600) return;
                if (sender.IsEnemy && sender.Name == "LeBlanc_Base_P_poof.troy")
                {
                    if (!LeBlank.IsVisible &&
                        !LeBlank.IsDead &&
                        CheckSlot() != SpellSlot.Unknown)
                    {
                        _Player.Spellbook.CastSpell(CheckSlot(), _Player.Position);
                    }
                }
            }
        }
        #endregion

        #region Vayne Ult
        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Misc.isChecked(PinkInviMenu, "usePink")) return;
            // Vayne
            var Vayne = HeroManager.Enemies.Find(x => x.ChampionName.ToLower() == "vayne");
            if (Vayne != null)
            {
                foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(
                    x =>
                        x.IsEnemy && x.ChampionName.ToLower().Contains("vayne") &&
                        x.Buffs.Any(y => y.Name == "VayneInquisition")))
                {
                    VayneBuffEndTime = hero.Buffs.First(x => x.Name == "VayneInquisition").EndTime;
                }
            }
        }
        #endregion
        
        #region Check Spell Slot
        private static SpellSlot CheckSlot() // trinket e pink
        {
            SpellSlot slot = SpellSlot.Unknown;
            if (Item.CanUseItem(3362) && Item.HasItem(3362, _Player)) 
            {
                slot = SpellSlot.Trinket;
            }
            else if (Item.CanUseItem(3364) && Item.HasItem(3364, _Player))
            {
                slot = SpellSlot.Trinket;
            }
            else if (Item.CanUseItem(2043) && Item.HasItem(2043, _Player))
            {
                slot = _Player.GetSpellSlotFromName("VisionWard");
            }
            return slot;
        }
        #endregion

    }
}
