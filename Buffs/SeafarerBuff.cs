using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod;

using Terraria.Localization;

namespace LordOfTheMysteriesMod.Buffs
{
    public class SeafarerBuff : ModBuff
    {
        BeyonderAbilities Abilities = new BeyonderAbilities();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seafarer Buff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "航海家");
            Description.SetDefault("Sailor Pathway Sequence 7 - Seafarer");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "水手途径序列7-航海家");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //Increase physical abilities. When the player is near the ocean, increase more abilities.
            if (player.ZoneBeach) {
                player.statDefense += 35;
                player.statLifeMax2 += 120;
                player.GetDamage(DamageClass.Melee) += 0.5f;
                player.moveSpeed += 0.5f;
            } else {
                player.statDefense += 25;
                player.statLifeMax2 += 120;
                player.GetDamage(DamageClass.Melee) += 0.4f;
                player.moveSpeed += 0.4f;
            }
            

            //Use Beyonder Abilities.
            Abilities.UpdateBeyonderAbilities(player);

            //Make sure that this buff will never be removed...at least for now.
            player.buffTime[buffIndex] = 1;
        }

        public override bool RightClick(int BuffIndex)
        {
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = "Empty";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = 10;
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList.Clear();
            Main.NewText("Player Sequence: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Pathway: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
            return true;
        }
    }
}