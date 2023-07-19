using System;
using Terraria;
using Terraria.ModLoader;
using LordOfTheMysteriesMod.Buffs;

namespace LordOfTheMysteriesMod
{
	public class LordOfTheMysteriesMod : Mod
	{
		public override void Load()
		{
			BeyonderAbilities BeyonderAbilities = new();
			BeyonderAbilities.Abilities.Add("Swimmer", new Action<Player>(BeyonderAbilities.Swimmer));
			BeyonderAbilities.Abilities.Add("NightVision", new Action<Player>(BeyonderAbilities.NightVision));
			BeyonderAbilities.Abilities.Add("RagingBlow", new Action<Player>(BeyonderAbilities.RagingBlow));
			BeyonderAbilities.Abilities.Add("SeafarerSense", new Action<Player>(BeyonderAbilities.SeafarerSense));
			BeyonderAbilities.Abilities.Add("WaterBall", new Action<Player>(BeyonderAbilities.WaterBall));
			BeyonderAbilities.Abilities.Add("Bioluminescence", new Action<Player>(BeyonderAbilities.Bioluminescence));
		}
	}
}