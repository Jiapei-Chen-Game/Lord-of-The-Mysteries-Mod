using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod;

using Terraria.Localization;

namespace LordOfTheMysteriesMod.Buffs
{
    public class FolkOfRageBuff : ModBuff
    {
        BeyonderAbilities Abilities = new BeyonderAbilities();
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Folk Of Rage Buff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暴怒之民");
            Description.SetDefault("Sailor Pathway Sequence 8 - Folk Of Rage");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "水手途径序列8-暴怒之民");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //Increase a tiny amount of ability.
            player.statDefense += 20;
            player.statLifeMax2 += 80;
            player.GetDamage(DamageClass.Melee) += 0.3f;
            player.moveSpeed += 0.3f;

            //Being able to stay underwater for more than 10 minutes.
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