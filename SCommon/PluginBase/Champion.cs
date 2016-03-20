using System;
using System.Runtime.CompilerServices;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.ThirdParty;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Constants;
using SCommon.Orbwalking;
using SharpDX;
using SharpDX.Direct3D9;
//typedefs
using Geometry = SCommon.Maths.Geometry;
using Color = System.Drawing.Color;
using TargetSelector = EloBuddy.SDK.TargetSelector;

namespace SCommon.PluginBase
{
    public abstract class Champion
    {
        public const int Q = 0, W = 1, E = 2, R = 3;

        public Menu ConfigMenu, DrawingMenu;
        public Orbwalking.Orbwalker Orbwalker;
        public Font Text;

        public delegate void dVoidDelegate();
        public dVoidDelegate OnUpdate, OnDraw, OnCombo, OnHarass, OnLaneClear, OnLastHit;

        public Champion(string szChampName, string szMenuName, bool enableRangeDrawings = true, bool enableEvader = true)
        {
            Text = new Font(Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Malgun Gothic",
                    Height = 15,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural
                });

           
            #region Events
            Game.OnUpdate += this.Game_OnUpdate;
            Orbwalking.Events.BeforeAttack += this.Orbwalking_BeforeAttack;
            Orbwalking.Events.AfterAttack += this.Orbwalking_AfterAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Obj_AI_Base.OnProcessSpellCast += this.Obj_AI_Base_OnProcessSpellCast;
            Dash.OnDash += Dash_OnDash;
            #endregion
        }

        private void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            //
        }

        private void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            //
        }

        private void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            //
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            //
        }

        public virtual void Game_OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead || ObjectManager.Player.IsRecalling() || args == null)
                return;

            if (OnUpdate != null)
                OnUpdate();

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.Orbwalker.Mode.Combo:
                    {
                        if (OnCombo != null)
                            OnCombo();
                    }
                    break;
                case Orbwalking.Orbwalker.Mode.Mixed:
                    {
                        if (OnHarass != null)
                            OnHarass();
                    }
                    break;
                case Orbwalking.Orbwalker.Mode.LaneClear:
                    {
                        if (OnLaneClear != null)
                            OnLaneClear();
                    }
                    break;
                case Orbwalking.Orbwalker.Mode.LastHit:
                    {
                        if (OnLastHit != null)
                            OnLastHit();
                    }
                    break;

            }
        }

        protected virtual void Orbwalking_BeforeAttack(BeforeAttackArgs args)
        {
            //
        }

        protected virtual void Orbwalking_AfterAttack(AfterAttackArgs args)
        {
            //
        }
        protected virtual void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //
        }
        /// <summary>
        /// Checks if combo is ready
        /// </summary>
        /// <returns>true if combo is ready</returns>
        
        #region Damage Calculation Funcitons
        /// <summary>
        /// Calculates combo damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="aacount">Auto Attack Count</param>
        /// <returns>Combo damage</returns>
        public double CalculateComboDamage(AIHeroClient target, int aacount = 2)
        {
            return CalculateSpellDamage(target) + CalculateSummonersDamage(target) + CalculateItemsDamage(target) + CalculateAADamage(target, aacount);
        }

        /// <summary>
        /// Calculates Spell Q damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Spell Q Damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateDamageQ(AIHeroClient target)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).IsReady)
                return ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);

            return 0.0;
        }

        /// <summary>
        /// Calculates Spell W damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Spell W Damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateDamageW(AIHeroClient target)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).IsReady)
                return ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);

            return 0.0;
        }

        /// <summary>
        /// Calculates Spell E damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Spell E Damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateDamageE(AIHeroClient target)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).IsReady)
                return ObjectManager.Player.GetSpellDamage(target, SpellSlot.E);

            return 0.0;
        }

        /// <summary>
        /// Calculates Spell R damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Spell R Damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateDamageR(AIHeroClient target)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady)
                return ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);

            return 0.0;
        }

        /// <summary>
        /// Calculates all spell's damage to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>All spell's damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateSpellDamage(AIHeroClient target)
        {
            return CalculateDamageQ(target) + CalculateDamageW(target) + CalculateDamageE(target) + CalculateDamageR(target);
        }

        /// <summary>
        /// Calculates summoner spell damages to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Summoner spell damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateSummonersDamage(AIHeroClient target)
        {
            var ignite = ObjectManager.Player.GetSpellSlotFromName("summonerdot");
            if (ignite != SpellSlot.Unknown && ObjectManager.Player.Spellbook.CanUseSpell(ignite) == SpellState.Ready && ObjectManager.Player.Distance(target, false) < 550)
                return ObjectManager.Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite); //ignite

            return 0.0;
        }

        /// <summary>
        /// Calculates Item's active damages to given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Item's damage</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateItemsDamage(AIHeroClient target)
        {
            double dmg = 0.0;

            if (Item.CanUseItem(3144) && ObjectManager.Player.Distance(target, false) < 550)
                dmg += ObjectManager.Player.GetItemDamage(target, ItemId.Bilgewater_Cutlass); //bilgewater cutlass

            if (Item.CanUseItem(3153) && ObjectManager.Player.Distance(target, false) < 550)
                dmg += ObjectManager.Player.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King); //botrk

            if (Item.HasItem(3057))
                dmg += DamageLibrary.GetItemDamage(ObjectManager.Player,target,ItemId.Sheen); //sheen

            if (Item.HasItem(3100))
                dmg += DamageLibrary.GetItemDamage(ObjectManager.Player, target, ItemId.Lich_Bane);//lich bane

            if (Item.HasItem(3285))
                dmg += DamageLibrary.GetItemDamage(ObjectManager.Player, target, ItemId.Ludens_Echo); //luden

            return dmg;

        }

        /// <summary>
        /// Calculates Auto Attack damage to given target
        /// </summary>
        /// <param name="target">Targetparam>
        /// <param name="aacount">Auto Attack count</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual double CalculateAADamage(AIHeroClient target, int aacount = 2)
        {
            return Damage.AutoAttack.GetDamage(target) * aacount;
        }
        #endregion
    }
}
