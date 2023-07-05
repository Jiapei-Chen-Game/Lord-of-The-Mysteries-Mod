using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod.Buffs;

using Terraria.DataStructures;
using Terraria.Localization;

namespace LordOfTheMysteriesMod.Potions
{
	public class SeafarerPotion : ModItem
	{

		public int Sequence = 7;
		public string SequenceName = "Seafarer";
		public string Pathway = "Sailor";
		public string[] Abilities = {"EnhancedBreathing", "FastSwimming", "NightVision", "RagingBlow", "SeafarerSense", "WaterBall", "WaterLight"};

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Seafarer Potion");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "航海家魔药");
			Tooltip.SetDefault("This is a tube of Seafarer Potion.\n" + "Drink it to become a Sailor.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "这是一试管航海家魔药\n" + "喝下它以成为航海家。");
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
            BeyonderPotion.PotionEffectRoll(player, Pathway, SequenceName, Sequence, ModContent.BuffType<SeafarerBuff>(), Abilities);
			Main.NewText("Player Sequence: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence, 255, 255, 255);
			Main.NewText("Player Sequence Name: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName, 255, 255, 255);
			Main.NewText("Player Pathway: " + player.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway, 255, 255, 255);
		    return true;
        }
	}
}