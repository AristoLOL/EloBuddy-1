﻿using System;
using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace SamLeeSin2
{
    class Program
    {
        public static Menu Config;
        //Spells
        public static Spell.Skillshot Q;
        public static Spell.Active Q2;
        public static Spell.Targeted W;
        public static Spell.Active E;
        public static Spell.Active E2;
        public static Spell.Targeted R;
        public static Spell.Targeted R2;
        public static string LastSpell;
        public static long LastSpellTime;
        public static long LastETime = 0;
        public static long LastAATime = 0;
        public static long LastRTime = 0;
        public static AttackableUnit LastRTarget;
        public static long LastQTime = 0;
        public static AttackableUnit LastQTarget;
        public static AttackableUnit LastAATarget;
        public static long PassiveTimer;
        public static int PassiveStacks;
        public static Menu menu, DrawingMenu;

        public static Dictionary<string, string> Spells = new Dictionary<string, string>
        {
            {"Q1", "BlindMonkQOne"},
            {"W1", "BlindMonkWOne"},
            {"E1", "BlindMonkEOne"},
            {"W2", "blindmonkwtwo"},
            {"Q2", "blindmonkqtwo"},
            {"E2", "blindmonketwo"},
            {"R1", "BlindMonkRKick"}
        };

        public static Dictionary<string, long> LastCast = new Dictionary<string, long>
        {
            {"BlindMonkQOne", 0},
            {"BlindMonkWOne", 0},
            {"BlindMonkEOne", 0},
            {"blindmonkwtwo", 0},
            {"blindmonkqtwo", 0},
            {"blindmonketwo", 0},
            {"BlindMonkRKick", 0}
        };
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnComplete;
        }
        private static void Loading_OnComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.LeeSin) return;

            menu = MainMenu.AddMenu("Sam LeeSin", "SamLeeSinMenu");
            menu.AddGroupLabel("SamLeeSin");
            menu.AddSeparator();
            
            menu.Add("useEAfterAutoAttack", new CheckBox("Use E After Auto Attack on Champions", true));
            menu.Add("useQAfterR", new CheckBox("Use Q After R", true));
            DrawingMenu = menu.AddSubMenu("Drawing Settings", "menuleesin");
            DrawingMenu.Add("drawQ1", new CheckBox("Draw Q1", false));
            DrawingMenu.Add("drawQ2", new CheckBox("Draw Q2", false));
            DrawingMenu.Add("drawW1", new CheckBox("Draw W1", false));
            DrawingMenu.Add("drawE1", new CheckBox("Draw E1", false));
            DrawingMenu.Add("drawE2", new CheckBox("Draw E2", false));
            DrawingMenu.Add("drawR", new CheckBox("Draw R", false));

            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 70);
            Q2 = new Spell.Active(SpellSlot.Q, 1400); // 1400
            W = new Spell.Targeted(SpellSlot.W, 700);
            E = new Spell.Active(SpellSlot.E, 430); //430
            E2 = new Spell.Active(SpellSlot.E, 600); // 600
            R = new Spell.Targeted(SpellSlot.R, 375);
            R2 = new Spell.Targeted(SpellSlot.R, 800);
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnAttack;
            //InsecManager.Init();
            ////StateManager.Init();
            WardJumper.Init();
            Hydra.Init();
            //Smiter.Init();
            Game.OnTick += Game_OnTick;

            Chat.Print("Sam Lee Sin Buddy Loaded.");
        }
        //private static void CastHydra()
        //{
        //    //Chat.Print(ObjectManager.Player.InventoryItems[0].Id);
        //    if (LastETime + 3000 > Environment.TickCount)
        //    {
                
        //        var hydraSlot = GetHydraSlot();
        //        if (hydraSlot == null)
        //            return;
        //        //Chat.Print("OnTick => CheckBox Checked => Inside CastHydra, Just used E => Not null");
        //        hydraSlot.Cast();
        //    }
        //}
        //public static InventorySlot GetHydraSlot()
        //{
        //    ItemId titanicHydra = (ItemId)3748;
        //    var hydraIds = new[] { ItemId.Ravenous_Hydra_Melee_Only, ItemId.Tiamat_Melee_Only, titanicHydra };
        //    return ObjectManager.Player.InventoryItems.FirstOrDefault(a => hydraIds.Contains(a.Id) && a.CanUseItem());
        //}
        
        private static void Drawing_OnDraw(EventArgs args)
        {
            /*
            if (DrawingMenu["drawQ1"].Cast<CheckBox>().CurrentValue && Q.Instance().Name == Spells["Q1"])
            {
                Circle.Draw(Q.IsReady() ? Color.BlueViolet : Color.OrangeRed, Q.Range, Player.Instance.Position);
            }
            if (DrawingMenu["drawQ2"].Cast<CheckBox>().CurrentValue && Q.Instance().Name == Spells["Q2"])
            {
                Circle.Draw(Q.IsReady() ? Color.BlueViolet : Color.OrangeRed, Q2.Range, Player.Instance.Position);
            }
            if (DrawingMenu["drawW1"].Cast<CheckBox>().CurrentValue && W.Instance().Name == Spells["W1"])
            {
                Circle.Draw(W.IsReady() ? Color.BlueViolet : Color.OrangeRed, W.Range, Player.Instance.Position);
            }
            if (DrawingMenu["drawE1"].Cast<CheckBox>().CurrentValue && E.Instance().Name == Spells["E1"])
            {
                Circle.Draw(E.IsReady() ? Color.BlueViolet : Color.OrangeRed, E.Range, Player.Instance.Position);
            }
            if (DrawingMenu["drawE2"].Cast<CheckBox>().CurrentValue && E.Instance().Name == Spells["E2"])
            {
                Circle.Draw(E.IsReady() ? Color.BlueViolet : Color.OrangeRed, E2.Range, Player.Instance.Position);
            }
            if (DrawingMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(R.IsReady() ? Color.BlueViolet : Color.OrangeRed, R.Range, Player.Instance.Position);
            }
             * */
        }

        private static void Game_OnTick(EventArgs args)
        {
            //if (menu["useHydraAfterE"].Cast<CheckBox>().CurrentValue)
            //{
            //    CastHydra();
            //}
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (LastQTarget != null &&
                LastQTarget.IsValidTarget(Q.Range) &&
                LastQTime + 500 > Environment.TickCount &&
                menu["useQAfterR"].Cast<CheckBox>().CurrentValue &&
                Q2.Name == Spells["Q2"] &&
                Q2.IsReady() &&
                Damage.Q2Damage((Obj_AI_Base)LastQTarget) >= LastQTarget.Health)
            {
                Q2.Cast();
                
                //return;
            }
                
            
            if (LastRTarget != null &&
                LastRTarget.IsValidTarget(Q.Range) &&
                LastRTime + 500 > Environment.TickCount &&
                menu["useQAfterR"].Cast<CheckBox>().CurrentValue &&
                Q.Name == Spells["Q1"] &&
                Q.IsReady())
            {
                Q.Cast((Obj_AI_Base)LastRTarget);
             
                //return;
            }
            //if (LastAATarget != null &&
            //    LastAATarget.IsValidTarget(E.Range) &&
            //    LastAATime + 3000 > Environment.TickCount &&
            //    menu["useEAfterAutoAttack"].Cast<CheckBox>().CurrentValue &&
            //    E.Name == Spells["E1"] &&
            //    E.IsReady())
            //{
            //    E.Cast();

            //    //return;

            //}
                

                
        }
        private static void Combo()
        {
            if (LastAATarget != null &&
                LastAATarget.IsValidTarget(E.Range) &&
                LastAATime + 1000 > Environment.TickCount &&
                menu["useEAfterAutoAttack"].Cast<CheckBox>().CurrentValue &&
                E.Name == Spells["E1"] &&
                E.IsReady())
            {
                E.Cast();
                
                //return;

            }
                

        }
        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            if (!target.IsValidTarget(E.Range) || !menu["useEAfterAutoAttack"].Cast<CheckBox>().CurrentValue)
                return;
            LastAATime = Environment.TickCount;
            LastAATarget = target;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;
            //Chat.Print(args.SData.Name);
            if (!LastCast.ContainsKey(args.SData.Name)) return;

            //if (args.SData.Name == Spells["E1"])
            //{
            //    LastAATarget = null;
            //    LastETime = Environment.TickCount;
            //}
                
            if (args.SData.Name == Spells["R1"])
            {
                //Chat.Print("R");
                LastRTarget = (AttackableUnit)args.Target;
                LastRTime = Environment.TickCount;
            }
            if (args.SData.Name == Spells["Q1"])
            {
                AttackableUnit t = (AttackableUnit)args.Target;
                if(t.IsValidTarget(Q2.Range)){
                    LastQTarget = t;
                    LastQTime = Environment.TickCount;
                    //LastRTarget = null;
                }
                
            }
                

            LastSpellTime = Environment.TickCount;
            LastSpell = args.SData.Name;
            LastCast[args.SData.Name] = Environment.TickCount;
        }
    }
}
