using System;

using Terraria;
using LordOfTheMysteriesMod.Buffs;

using Terraria.DataStructures;
using System.Diagnostics;

namespace LordOfTheMysteriesMod.Potions
{
    public static class BeyonderPotion
    {
        static bool DEBUG = true;

        public static bool Roll(int BaseSurvivePossibility, double Factor)
        {
            int TruePossibility = (int)(Factor * BaseSurvivePossibility);
            Random rnd = new();
            int RollResult = rnd.Next(0, 100);
            if (DEBUG == true) {
                Main.NewText("True Survive Possibility: " + TruePossibility, 255, 255, 255);
                Main.NewText("Roll Result: " + RollResult, 255, 255, 255);
            }          
            if (RollResult >= TruePossibility) {
                return false;
            }
            return true;
        }

        //Decide whether the advancement of the player is successful.
        //Parameter: player: The player who just took the potion.
        //Return: void
        public static void PotionEffectRoll(Player player, string Pathway, string SequenceName, int Sequence, int BeyonderBuff, string[] Abilities)
        {
            //If the player is not a beyonder, then the possibility of survival after drinking a potion will decrease
            //from 80% to 1% as the sequence of the potion decrease. If the sequence of the potion is smaller than 6, 
            //the possibility that the player will not survive is 100%!
            //If the player is a beyonder and the player's pathway is equal to the potion's pathway, then the possibility
            //of survival will decrease...
            if (player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway.Equals("")) {
                if (Sequence < 6) {
                    player.KillMe(PlayerDeathReason.ByCustomReason("You lost control and died."), 1.0, 0, false);
                } else {
                    int BaseSurvivePossibility = 80;
                    double Factor = Math.Pow(0.25, 9 - Sequence);
                    bool AdvancementResult = BeyonderPotion.Roll(BaseSurvivePossibility, Factor);
                    
                    if (!AdvancementResult) {
                        player.KillMe(PlayerDeathReason.ByCustomReason("You lost control and died."), 1.0, 0, false);
                    } else {
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = Pathway;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName = SequenceName;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = Sequence;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().BeyonderBuff = BeyonderBuff;
                        AddBeyonderAbilities(player, Abilities);
                        player.AddBuff(BeyonderBuff, 1);
                    }
                }
            } else if (player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway.Equals(Pathway) && player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence > Sequence) {
                if (player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence > 5 && Sequence < 5) {
                    player.KillMe(PlayerDeathReason.ByCustomReason("You lost control and died."), 1.0, 0, false);
                } else {
                    
                    int BaseSurvivePossibility = 80;
                    double Factor = Math.Pow(0.25, player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence - Sequence - 1);
                    bool AdvancementResult = Roll(BaseSurvivePossibility, Factor);

                    if (!AdvancementResult) {
                        player.KillMe(PlayerDeathReason.ByCustomReason("You lost control and died."), 1.0, 0, false);
                    } else {
                        player.ClearBuff(player.GetModPlayer<LordOfTheMysteriesModPlayer>().BeyonderBuff);
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = Pathway;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName = SequenceName;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = Sequence;
                        player.GetModPlayer<LordOfTheMysteriesModPlayer>().BeyonderBuff = BeyonderBuff;
                        AddBeyonderAbilities(player, Abilities);
                        player.AddBuff(BeyonderBuff, 1);
                    }
                }
            }
        }

        //Add beyonder abilities of the potion to the player beyonder ability list.
        //Parameter: player: The player who just took the potion. 
        //           PotionBeyonderAbilities: A string array containing names of beyonder abilities given by the potion.
        //Return: void
        public static void AddBeyonderAbilities(Player player, string[] PotionBeyonderAbilities)
        {
            for (int i = 0; i < PotionBeyonderAbilities.Length; i++) {
                if (!player.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList.ContainsKey(PotionBeyonderAbilities[i])) {
                    player.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList.Add(PotionBeyonderAbilities[i], BeyonderAbilities.Abilities[PotionBeyonderAbilities[i]]);
                    if (DEBUG == true) {
                        Main.NewText("Beyonder Ability Get: " + PotionBeyonderAbilities[i], 255, 255, 255);
                    }
                }
            }    
        }
    }
}
