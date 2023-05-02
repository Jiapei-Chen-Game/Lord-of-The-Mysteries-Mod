using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod.Buffs;

using Terraria.DataStructures;
using Terraria.Localization;

namespace LordOfTheMysteriesMod.Potions
{
	public class MarauderPotion : ModItem
	{

		public int Sequence = 9;
		public string Pathway = "Error";
		public string[] Abilities = {};

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("MarauderPotion");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "偷盗者魔药");
			Tooltip.SetDefault("This is a tube of Marauder Potion.\n" + "Drink it to become a Marauder.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "这是一试管偷盗者魔药\n" + "喝下它以成为偷盗者。");
		}

		public override void SetDefaults()
		{
			Item.width = 9;
			Item.height = 30;

			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.useStyle = 2;

			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.maxStack = 30;

			Item.rare = 4;
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
            BeyonderPotion.PotionEffectRoll(player, Pathway, Sequence, ModContent.BuffType<MarauderBuff>(), Abilities);
			Main.NewText("Player Sequence: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Pathway: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
		    return true;
        }
	}
}