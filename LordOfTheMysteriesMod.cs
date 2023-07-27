using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;

using Microsoft.Xna.Framework.Input;

using LordOfTheMysteriesMod.Buffs;

namespace LordOfTheMysteriesMod
{
	public class LordOfTheMysteriesMod : Mod
	{
		static public ModKeybind BeyonderAbilityScrollLeft;
		static public ModKeybind BeyonderAbilityScrollRight;
		static public ModKeybind BeyonderAbilityActivate;
		public override void Load()
		{
			BeyonderAbilities BeyonderAbilities = new();
			BeyonderAbilities.Abilities.Add("Swimmer", new Action<Player>(BeyonderAbilities.Swimmer));
			BeyonderAbilities.Abilities.Add("NightVision", new Action<Player>(BeyonderAbilities.NightVision));
			BeyonderAbilities.Abilities.Add("RagingBlow", new Action<Player>(BeyonderAbilities.RagingBlow));
			BeyonderAbilities.Abilities.Add("SeafarerSense", new Action<Player>(BeyonderAbilities.SeafarerSense));
			BeyonderAbilities.Abilities.Add("WaterBall", new Action<Player>(BeyonderAbilities.WaterBall));
			BeyonderAbilities.Abilities.Add("Bioluminescence", new Action<Player>(BeyonderAbilities.Bioluminescence));

			BeyonderAbilityScrollLeft = KeybindLoader.RegisterKeybind(this, "Beyonder Ability Loop backward", Keys.Z);
			BeyonderAbilityScrollRight = KeybindLoader.RegisterKeybind(this, "Beyonder Ability Loop forward", Keys.X);
			BeyonderAbilityActivate = KeybindLoader.RegisterKeybind(this, "Beyonder Ability Activate", Keys.C);
		}
	}
}