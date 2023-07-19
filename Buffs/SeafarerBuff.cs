using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace LordOfTheMysteriesMod.Buffs
{
    public class SeafarerBuff : ModBuff
    {
        // readonly BeyonderAbilities Abilities = new();
        string texturePath = "LordOfTheMysteriesMod/Buffs/SeafarerBuff";

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
            BeyonderAbilities.UpdateBeyonderAbilities(player);

            //Make sure that this buff will never be removed...at least for now.
            player.buffTime[buffIndex] = 1;
        }

        public override bool RightClick(int BuffIndex)
        {
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = "";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName = "";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = 10;
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList.Clear();
            Main.NewText("Player Sequence: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Pathway: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, drawParams.Position, null, Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
            return false;
        }
    }
}