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
        string texturePath;
        float textureScale = 1.0f;

        public NoteBookElement(string path) {
            texturePath = path;
        }

        public void SetTexturePath(string path) {
            texturePath = path;
        }

        public void SetTextureScale(float scale) {
            textureScale = scale;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), null, Color.White, 0f, Vector2.Zero, textureScale, SpriteEffects.None, 0f);
        }
    }

    public class NotebookLeftPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/NotebookLeftPage";
        public NotebookLeftPage()
        {
            Width.Set(440f, 0f);
            Height.Set(580f, 0f);
            Left.Set(-440f, 0.5f);
            Top.Set(10f, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), Color.White);
        }
    }

    public class NotebookRightPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/NotebookRightPage";
        public NotebookRightPage()
        {
            Width.Set(440f, 0f);
            Height.Set(580f, 0f);
            Left.Set(0f, 0.5f);
            Top.Set(10f, 0f);
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

            noteBookButton.SetVisibility(1f, 0.8f);
            noteBookButton.Width.Set(96f, 0f);
            noteBookButton.Height.Set(96f, 0f);
            noteBookButton.Left.Set(-200f, 0.5f);
            noteBookButton.Top.Set(30f, 0f);

            noteBookButton.OnClick += (evt, element) => {
                NoteBookUI.Visible = true;
            };

            Append(noteBookButton);
        }
    }

    public class NoteBookUI : UIState
    {
        public static bool Visible = false;

        NoteBookElement Cover = new("LordOfTheMysteriesMod/UI/NotebookCover");

        NotebookLeftPage Page1 = new();
        NoteBookElement Page1PlayerPathSymbol = new("LordOfTheMysteriesMod/Buffs/SailorBuff");
        UIText Page1PlayerPathInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceNameInformation = new("To Be Loaded");

        NotebookRightPage Page2 = new();
        UIImageButton closeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/ButtonDelete"));

        public override void OnInitialize() 
        {
            Player player = Main.CurrentPlayer;

            Cover.Width.Set(900f, 0f);
            Cover.Height.Set(600f, 0f);
            Cover.Left.Set(-450f, 0.5f);
            Cover.Top.Set(-300f, 0.5f);
            Append(Cover);
            
            Cover.Append(Page1);
            Page1.Append(Page1PlayerPathSymbol);
            Page1PlayerPathSymbol.Width.Set(160f, 0f);
            Page1PlayerPathSymbol.Height.Set(160f, 0f);
            Page1PlayerPathSymbol.HAlign = 0.5f;
            Page1PlayerPathSymbol.Top.Set(120f, 0f);
            Page1.Append(Page1PlayerPathInformation);
            Page1PlayerPathInformation.HAlign = 0.5f;
            Page1PlayerPathInformation.Top.Set(0, 0.5f);
            Page1.Append(Page1PlayerSequenceInformation);
            Page1PlayerSequenceInformation.HAlign = 0.5f;
            Page1PlayerSequenceInformation.Top.Set(30f, 0.5f);
            Page1.Append(Page1PlayerSequenceNameInformation);
            Page1PlayerSequenceNameInformation.HAlign = 0.5f;
            Page1PlayerSequenceNameInformation.Top.Set(60f, 0.5f);

            Cover.Append(Page2);
 
            closeButton.Width.Set(32f, 0f);
            closeButton.Height.Set(32f, 0f);
            closeButton.Left.Set(-75f, 1f);
            closeButton.Top.Set(27f, 0f);
            closeButton.OnClick += (evt, element) => {
                Visible = false;
            };
            Cover.Append(closeButton);
        }

        public override void Update(GameTime gametime) {
            Page1PlayerPathInformation.SetText("Pathway: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Pathway);
            Page1PlayerSequenceInformation.SetText("Sequence: " + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence.ToString());
            Page1PlayerSequenceNameInformation.SetText(Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName);
            Page1PlayerPathSymbol.SetTexturePath("LordOfTheMysteriesMod/Buffs/" + Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>().SequenceName + "Buff");
            Page1PlayerPathSymbol.SetTextureScale(5.0f);
        }
    }
}