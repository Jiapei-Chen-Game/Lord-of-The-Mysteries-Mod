using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod;

using Terraria.Localization;

namespace LordOfTheMysteriesMod.Buffs
{
    public class MarauderBuff : ModBuff
    {
        readonly BeyonderAbilities Abilities = new();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marauder Buff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "偷盗者");
            Description.SetDefault("Error Pathway Sequence 9 - Marauder");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "错误途径序列9-偷盗者");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)        
        {
            //Now the player belongs to the Error Pathway.

            //Increase a tiny amount of ability.
            player.statDefense += 1;
            player.statLifeMax2 += 20;
            player.moveSpeed += 0.15f;

            //Being able to open a wooden chest.
            Abilities.UpdateBeyonderAbilities(player);
            
            //Make sure that this buff will never be removed...at least for now.
            player.buffTime[buffIndex] = 2;
        }

        public override bool RightClick(int BuffIndex)
        {
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = "Empty";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = 10;
            Main.NewText("Player Sequence: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Pathway: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
            return true;
        }
    }
}