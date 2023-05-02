using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod;

using Terraria.Localization;

namespace LordOfTheMysteriesMod.Buffs
{
    public class SailorBuff : ModBuff
    {
        BeyonderAbilities Abilities = new BeyonderAbilities();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor Buff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "水手");
            Description.SetDefault("Sailor Pathway Sequence 9 - Sailor");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "水手途径序列9-水手");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //Increase immense physical abilities.
            player.statDefense += 10;
            player.statLifeMax2 += 40;
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.moveSpeed += 0.05f;

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