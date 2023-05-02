using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

namespace LordOfTheMysteriesMod.UI
{
    public class NoteBookPanel : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/PanelBackground").Value, GetDimensions().Position(), Color.White);
        }
    }

    public class NoteBookButton : UIState
    {
        public override void OnInitialize()
        {
            UIImageButton noteBookButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/ButtonPlay"));
            noteBookButton.Width.Set(96f, 0f);
            noteBookButton.Height.Set(96f, 0f);
            noteBookButton.Left.Set(-320f, 1f);
            noteBookButton.Top.Set(20f, 0f);
            noteBookButton.OnClick += (evt, element) => {
                NoteBookUI.Visible = true;
            };
            Append(noteBookButton);
        }
    }

    public class NoteBookUI : UIState
    {
        public static bool Visible = false;

        public override void OnInitialize() 
        {
            NoteBookPanel panel = new NoteBookPanel();
            panel.Width.Set(568f, 0f);
            panel.Height.Set(488f, 0f);
            panel.Left.Set(-284f, 0.5f);
            panel.Top.Set(-244f, 0.5f);
            Append(panel);

            UIImageButton closeButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/ButtonDelete"));
            closeButton.Width.Set(32f, 0f);
            closeButton.Height.Set(32f, 0f);
            closeButton.Left.Set(-48f, 1f);
            closeButton.Top.Set(16f, 0f);
            closeButton.OnClick += (evt, element) => {
                Visible = false;
            };
            panel.Append(closeButton);
        }
    }
}