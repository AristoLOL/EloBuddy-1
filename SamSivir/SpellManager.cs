﻿using EloBuddy;
using EloBuddy.SDK;

namespace SamSivir
{
    public static class SpellManager
    {
        // You will need to edit the types of spells you have for each champ as they
        // don't have the same type for each champ, for example Xerath Q is chargeable,
        // right now it's  set to Active.
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            // Initialize spells
            //Q = new Spell.Active(SpellSlot.Q, /*Spell range*/ 1000);
            Q = new Spell.Skillshot(SpellSlot.Q, 1250, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 1350, 90);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            // TODO: Uncomment the other spells to initialize them
            //W = new Spell.Chargeable(SpellSlot.W);
            //E = new Spell.Skillshot(SpellSlot.E);
            //R = new Spell.Targeted(SpellSlot.R);
        }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }
    }
}