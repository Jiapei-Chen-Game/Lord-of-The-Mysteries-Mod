using System;

using Terraria;
using LordOfTheMysteriesMod.Buffs;

using Terraria.DataStructures;

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
                    KillPlayer(player);
                } else {
                    int BaseSurvivePossibility = 100;
                    double Factor = Math.Pow(0.25, 9 - Sequence);
                    bool AdvancementResult = Roll(BaseSurvivePossibility, Factor);
                    
                    if (!AdvancementResult) {
                        KillPlayer(player);
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
                    KillPlayer(player);
                } else {
                    int BaseSurvivePossibility = 100;
                    double Factor = Math.Pow(0.25, player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence - Sequence - 1);
                    bool AdvancementResult = Roll(BaseSurvivePossibility, Factor);

                    if (!AdvancementResult) {
                        KillPlayer(player);
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

        /// <summary>
        /// Add beyonder abilities of the potion to the player beyonder ability list.
        /// </summary>
        /// <param name="player">The player who just took the potion</param>
        /// <param name="PotionBeyonderAbilities">A string array containing names of beyonder abilities given by the potion</param>
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

        /// <summary>
        /// If the player drinks the potion and failed to advance, kill the player and clear up his beyonder status.
        /// </summary>
        /// <param name="player">The player who just took the potion</param>
        public static void KillPlayer(Player player) {
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = "";
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName = "";
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = 10;
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().BeyonderBuff = 0;
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList.Clear();
            player.KillMe(PlayerDeathReason.ByCustomReason("You lost control and died."), 1.0, 0, false);
        }
    }
}
