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
			BeyonderAbilities.Abilities.Add("EnhancedBreathing", new Action<Player>(BeyonderAbilities.EnhancedBreathing));
			BeyonderAbilities.Abilities.Add("FastSwimming", new Action<Player>(BeyonderAbilities.FastSwimming));
			BeyonderAbilities.Abilities.Add("NightVision", new Action<Player>(BeyonderAbilities.NightVision));
			BeyonderAbilities.Abilities.Add("RagingBlow", new Action<Player>(BeyonderAbilities.RagingBlow));
			BeyonderAbilities.Abilities.Add("SeafarerSense", new Action<Player>(BeyonderAbilities.SeafarerSense));
			BeyonderAbilities.Abilities.Add("WaterBall", new Action<Player>(BeyonderAbilities.WaterBall));
			BeyonderAbilities.Abilities.Add("WaterLight", new Action<Player>(BeyonderAbilities.WaterLight));
		}
	}
}