using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

namespace LordOfTheMysteriesMod.UI
{
    public class NoteBookElement : UIElement
    {
        readonly string texturePath;

        public NoteBookElement(string path) {
            texturePath = path;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), Color.White);
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
            NoteBookElement Cover = new NoteBookElement("LordOfTheMysteriesMod/UI/NotebookCover");
            Cover.Width.Set(900f, 0f);
            Cover.Height.Set(600f, 0f);
            Cover.Left.Set(-450f, 0.5f);
            Cover.Top.Set(-300f, 0.5f);
            Append(Cover);

            NoteBookElement leftPage = new NoteBookElement("LordOfTheMysteriesMod/UI/NotebookLeftPage");
            leftPage.Width.Set(440f, 0f);
            leftPage.Height.Set(580f, 0f);
            leftPage.Left.Set(-440f, 0.5f);
            leftPage.Top.Set(10f, 0f);
            Cover.Append(leftPage);

            NoteBookElement rightPage = new NoteBookElement("LordOfTheMysteriesMod/UI/NotebookRightPage");
            rightPage.Width.Set(440f, 0f);
            rightPage.Height.Set(580f, 0f);
            rightPage.Left.Set(0f, 0.5f);
            rightPage.Top.Set(10f, 0f);
            Cover.Append(rightPage);

            UIImageButton closeButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/ButtonDelete"));
            closeButton.Width.Set(32f, 0f);
            closeButton.Height.Set(32f, 0f);
            closeButton.Left.Set(-75f, 1f);
            closeButton.Top.Set(27f, 0f);
            closeButton.OnClick += (evt, element) => {
                Visible = false;
            };
            Cover.Append(closeButton);
        }
    }
}