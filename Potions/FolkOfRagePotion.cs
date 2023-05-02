using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod.Buffs;

using Terraria.DataStructures;
using Terraria.Localization;

namespace LordOfTheMysteriesMod.Potions
{
	public class FolkOfRagePotion : ModItem
	{

		public int Sequence = 8;
		public string Pathway = "Tyrant";
		public string[] Abilities = {"EnhancedBreathing", "FastSwimming", "NightVision", "RagingBlow"};

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Folk Of Rage Potion");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暴怒之民魔药");
			Tooltip.SetDefault("This is a tube of Folk Of Rage Potion.\n" + "Drink it to become a Sailor.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "这是一试管暴怒之民魔药\n" + "喝下它以成为暴怒之民。");
		}

		public override void SetDefaults()
		{
			Item.width = 9;
			Item.height = 30;

			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.useStyle = 2;

			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.maxStack = 30;

			Item.rare = 5;
			Item.UseSound = SoundID.Item3;

			Item.consumable = true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			//recipe.AddTile(TileID.Bottles);
			//recipe.AddTile(TileID.AlchemyTable);
			recipe.Register();
        }

        public override bool CanUseItem(Player player)
		{
			return true;
		}

        public override bool? UseItem(Player player)
        {
            BeyonderPotion.PotionEffectRoll(player, Pathway, Sequence, ModContent.BuffType<FolkOfRageBuff>(), Abilities);
			Main.NewText("Player Sequence: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Pathway: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
		    return true;
        }
	}
}