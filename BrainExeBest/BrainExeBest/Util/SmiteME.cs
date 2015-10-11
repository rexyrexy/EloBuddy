using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

namespace BrainDotExe.Util
{
    internal class SmiteME
    {
        public static Spell.Targeted smite; // declarando o smite como spell
        private static SpellDataInst slot1;
        private static SpellDataInst slot2;
        private static SpellSlot smiteSlot;
        private static Menu Smiterino;

        private static readonly string[] BuffsSmite =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon",
            "SRU_Baron", "SRU_Gromp", "SRU_Murkwolf",
            "SRU_Razorbeak", "SRU_Krug", "Sru_Crab",
            "TT_Spiderboss", "TTNGolem", "TTNWolf",
            "TTNWraith"
        };

        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        #region Init

        public static void Init()
        {
            slot1 = _Player.Spellbook.GetSpell(SpellSlot.Summoner1);
            slot2 = _Player.Spellbook.GetSpell(SpellSlot.Summoner2);

            var smiteNames = new[]
            {
                "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick",
                "s5_summonersmiteduel", "summonersmite"
            };
            if (smiteNames.Contains("smite"))
            {
                smite = new Spell.Targeted(SpellSlot.Summoner1, (uint) 560f);
                smiteSlot = SpellSlot.Summoner1;
            }
            if (smiteNames.Contains("smite"))
            {
                smite = new Spell.Targeted(SpellSlot.Summoner2, (uint) 560f);
                smiteSlot = SpellSlot.Summoner2;
            }
            Smiterino = Program.Menu.AddSubMenu("Auto Smite", "Smite");
            Smiterino.AddGroupLabel("Auto Smite Settings");
            Smiterino.AddSeparator();
            Smiterino.Add("smiteActive", new CheckBox("Smite Active"));
            Smiterino.Add("drawHp", new CheckBox("Draw HP Bar on Minions"));
            Smiterino.Add("autoSmite", new KeyBind("AutoSmite Active HotKey", true, KeyBind.BindTypes.PressToggle, 'N'));
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        #endregion

        private static double SmiteDamage()
        {
            if(smite == null) return 0d;
            var damage = new[]
            {
                20*_Player.Level + 370, 30*_Player.Level + 330, 40*+_Player.Level + 240,
                50*_Player.Level + 100
            };
            return _Player.Spellbook.CanUseSpell(smite.Slot) == SpellState.Ready ? damage.Max() : 0;
        }

        private static void JungleSmite()
        {
            var minion =
                ObjectManager.Get<Obj_AI_Minion>()
                    .FirstOrDefault(m => m.IsValidTarget() && BuffsSmite.Contains(m.BaseSkinName));
            if (minion == null || smiteSlot == SpellSlot.Unknown) return;
            if (SmiteDamage() >= minion.Health && Smiterino["autoSmite"].Cast<KeyBind>().CurrentValue)
            {
                Player.CastSpell(smite.Slot, minion);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (SmiteDamage() != 0 && smiteSlot != SpellSlot.Unknown && Misc.isChecked(Smiterino, "drawHp") && smite.IsReady())
            {
                var minions = ObjectManager.Get<Obj_AI_Minion>().Where(
                    m => m.Team == GameObjectTeam.Neutral && m.IsValidTarget() &&
                         BuffsSmite.Contains(m.BaseSkinName));

                foreach (var m in minions)
                {
                    var hpBarPosition = m.HPBarPosition;
                    var maxHealth = m.MaxHealth;
                    var sDmg = SmiteDamage()/maxHealth;
                    var barWidth = 0;

                    switch (m.BaseSkinName)
                    {
                        case "SRU_Red":
                            barWidth = 145;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X - 22 + (float) (barWidth*sDmg), hpBarPosition.Y + 13),
                                new Vector2(hpBarPosition.X - 22 + (float) (barWidth*sDmg), hpBarPosition.Y + 29),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X - 22 + (float) (barWidth*sDmg),
                                hpBarPosition.Y - 3,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Blue":
                            barWidth = 145;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X - 22 + (float) (barWidth*sDmg), hpBarPosition.Y + 13),
                                new Vector2(hpBarPosition.X - 22 + (float) (barWidth*sDmg), hpBarPosition.Y + 29),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X - 22 + (float) (barWidth*sDmg),
                                hpBarPosition.Y - 3,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Gromp":
                            barWidth = 87;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 16),
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 21),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(hpBarPosition.X + 5 + (float) (barWidth*sDmg),
                                hpBarPosition.Y,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Krug":
                            barWidth = 81;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 16),
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 21),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X + 5 + (float) (barWidth*sDmg),
                                hpBarPosition.Y,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Razorbeak":
                            barWidth = 75;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 16),
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 21),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X + 54 + (float) (barWidth*sDmg),
                                hpBarPosition.Y,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Murkwolf":
                            barWidth = 75;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 16),
                                new Vector2(hpBarPosition.X + 14 + (float) (barWidth*sDmg), hpBarPosition.Y + 21),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X + 50 + (float) (barWidth*sDmg),
                                hpBarPosition.Y,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "Sru_Crab":
                            barWidth = 61;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X + 26 + (float) (barWidth*sDmg), hpBarPosition.Y + 30),
                                new Vector2(hpBarPosition.X + 26 + (float) (barWidth*sDmg), hpBarPosition.Y + 37),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X + 26 + (float) (barWidth*sDmg),
                                hpBarPosition.Y + 16,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Baron":
                            barWidth = 194;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X - 43 + (float) (barWidth*sDmg), hpBarPosition.Y + 11),
                                new Vector2(hpBarPosition.X - 43 + (float) (barWidth*sDmg), hpBarPosition.Y + 29),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X - 22 + (float) (barWidth*sDmg),
                                hpBarPosition.Y - 3,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                        case "SRU_Dragon":
                            barWidth = 145;
                            Drawing.DrawLine(
                                new Vector2(hpBarPosition.X - 15 + (float) (barWidth*sDmg), hpBarPosition.Y + 15),
                                new Vector2(hpBarPosition.X - 15 + (float) (barWidth*sDmg), hpBarPosition.Y + 28),
                                2f,
                                Color.Aqua);
                            Drawing.DrawText(
                                hpBarPosition.X - 22 + (float) (barWidth*sDmg),
                                hpBarPosition.Y,
                                Color.Aqua,
                                SmiteDamage().ToString());
                            break;
                    }
                }
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            if (!_Player.IsDead && Misc.isChecked(Smiterino, "smiteActive"))
            {
                JungleSmite();
            }
        }
    }
}
