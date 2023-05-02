using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;

using LordOfTheMysteriesMod.UI;

namespace LordOfTheMysteriesMod
{
    public class LordOfTheMysteriesModSystem : ModSystem
    {
		NoteBookUI NoteBook = new NoteBookUI();
		UserInterface NoteBookInterface = new UserInterface();
		NoteBookButton NoteBookButton = new NoteBookButton();
		UserInterface NoteBookButtonInterface = new UserInterface();

        public override void Load()
        {
			NoteBook.Activate();
			NoteBookInterface.SetState(NoteBook);
			NoteBookButton.Activate();
			NoteBookButtonInterface.SetState(NoteBookButton);
        }

        public override void UpdateUI(GameTime gameTime)
		{
			if (NoteBookUI.Visible) {
				NoteBookInterface?.Update(gameTime);
			}
			NoteBookButtonInterface?.Update(gameTime);
			base.UpdateUI(gameTime);
		}

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

			if (MouseTextIndex != -1) {
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				"LordOfTheMysteriesMod : NoteBookUI",
				delegate
				{
					if (NoteBookUI.Visible)
						NoteBook.Draw(Main.spriteBatch);
					return true;
				},
				InterfaceScaleType.UI)
			    );

				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				"LordOfTheMysteriesMod : NoteBookButton",
				delegate
				{
					NoteBookButton.Draw(Main.spriteBatch);
					return true;
				},
				InterfaceScaleType.UI)
			    );
			}
			base.ModifyInterfaceLayers(layers);
		}
    }
}