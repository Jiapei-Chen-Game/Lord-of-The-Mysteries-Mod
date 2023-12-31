using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace LordOfTheMysteriesMod.Buffs
{
    public class SeerBuff : ModBuff
    {
        // readonly BeyonderAbilities Abilities = new();
        string texturePath = "LordOfTheMysteriesMod/Buffs/SeerBuff";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seer Buff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "占卜家");
            Description.SetDefault("Fool Pathway Sequence 9 - Seer");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "愚者途径序列9-占卜家");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //Now the player belongs to the Fool Pathway.

            //Increase a tiny amount of ability.
            player.statDefense += 1;
            player.statLifeMax2 += 20;
            player.moveSpeed += 0.05f;

            //Spirit Vision...Maybe I can find a better way to do this in the future.
            //Being able to master Divination methods...
            BeyonderAbilities.UpdateBeyonderAbilities(player);
            
            //Make sure that this buff will never be removed...at least for now.
            player.buffTime[buffIndex] = 1;
        }

        public override bool RightClick(int BuffIndex)
        {
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway = "";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName = "";
            Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence = 10;
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