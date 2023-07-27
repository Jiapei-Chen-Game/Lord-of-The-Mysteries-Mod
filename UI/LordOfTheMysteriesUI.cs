using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using Terraria.Localization;

namespace LordOfTheMysteriesMod.UI
{
    public class NotebookCover : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/NotebookCover";
        public NotebookCover()
        {
            Width.Set(900f, 0f);
            Height.Set(600f, 0f);
            Left.Set(-450f, 0.5f);
            Top.Set(-300f, 0.5f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), Color.White);
        }
    }

    public class NotebookLeftPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/NotebookLeftPage";
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
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/NotebookRightPage";
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

    public class NoteBookBeyonderAbilityThumbNail : UIImageButton
    {
        public string imagePath;
        public string text;
        public List<UIElement> DetailedPage;
        public NoteBookBeyonderAbilityThumbNail(ReLogic.Content.Asset<Texture2D> texture, string text, List<UIElement> DetailedPage, string imagePath = "LordOfTheMysteriesMod/UI/UIImages/TestImage") : base(texture)
        {
            this.imagePath = imagePath;
            this.text = text;
            this.DetailedPage = DetailedPage;
        }

        public string GetText() {
            return text;
        }
    }

    public class NoteBookScrollbar : UIScrollbar
    {
        readonly string texturePath;
        UIImage scrollbarSlider;
        UIList uIList;

        float uIListTotalHeight;
        public NoteBookScrollbar(string texturePath, UIList uIList)
        {
            this.texturePath = texturePath;
            this.uIList = uIList;

            scrollbarSlider = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/NotebookScrollBarSlider"));
            scrollbarSlider.Height.Set(384f, 0f);
            scrollbarSlider.Width.Set(9f, 0f);
            scrollbarSlider.Left.Set(3f, 0f);
            scrollbarSlider.Top.Set(3f, 0f);
            scrollbarSlider.ScaleToFit = true;
            Append(scrollbarSlider);

            uIListTotalHeight = uIList.GetTotalHeight();
        }

        public override void Update(GameTime gameTime)
        {
            if (uIListTotalHeight != uIList.GetTotalHeight()) {
                if (uIList.GetTotalHeight() > uIList.Height.Pixels) {
                    scrollbarSlider.Height.Set(uIList.Height.Pixels * 384f / uIList.GetTotalHeight(), 0f);
                    scrollbarSlider.Top.Set(uIList.ViewPosition * (384 - scrollbarSlider.Height.Pixels) / (uIList.GetTotalHeight() - uIList.Height.Pixels) + 3f, 0f);
                } else {
                    scrollbarSlider.Height.Set(384f, 0f);
                    scrollbarSlider.Top.Set(3f, 0f);
                }
                scrollbarSlider.Recalculate();
            }
            base.Update(gameTime);
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
            UIImageButton noteBookButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonPlay"));

            noteBookButton.SetVisibility(1f, 0.8f);
            noteBookButton.Width.Set(96f, 0f);
            noteBookButton.Height.Set(64f, 0f);
            noteBookButton.Top.Set(-120f, 1f);
            noteBookButton.HAlign = 0.5f;

            noteBookButton.OnClick += (evt, element) => {
                NoteBookUI.PageVisible[NoteBookUI.currentPageNumber] = false;
                NoteBookUI.currentPageNumber = 0;
                NoteBookUI.PageVisible[NoteBookUI.currentPageNumber] = true;
                NoteBookUI.Visible = true;
            };

            Append(noteBookButton);
        }
    }

    public class NoteBookUI : UIState
    {
        public static bool DEBUG = true;

        public static bool Visible = false;
        public static bool[] PageVisible = new bool[256];
        public static int currentPageNumber = 0;
        public static int PageNumber = 0;

        public static List<List<UIElement>> PlayerBasicBeyonderInformation = new();
        public static List<List<UIElement>> NoteBookMenu = new();
        public static List<List<UIElement>> PlayerAbilityThumbNailSection = new();
        public static List<List<UIElement>> PlayerAbilityDetailedPageSection = new();
        public static List<List<UIElement>> PotionFormulaThumbNailSection = new();
        public static List<List<UIElement>> PotionFormulaDetailedPageSection = new();
        public static List<List<UIElement>> PlayerNoteSection = new();

        NoteBookScrollbar PlayerAbilityThumbNailScrollbar;

        NotebookCover Cover = new();
        NotebookLeftPage LeftPage = new();
        NotebookRightPage RightPage = new();
        UIText LeftPageNumber = new("1");
        UIText RightPageNumber = new("2");

        UIImage Page1PlayerPathSymbol = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/TestImage"));
        UIText Page1PlayerPathInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceNameInformation = new("To Be Loaded");

        UIText Page2MenuTitle = new("Menu", textScale: 1.5f);
        UIText Page2MenuPlayerBeyonderAbilities = new("1. My Beyonder Abilities");
        UIText Page2MenuPotionFormula = new("2. Potion Formulas");
        UIText Page2MenuPlayerNotes = new("3. My Notes");

        UIList BeyonderAbilityThumbNails = new();

        UIImageButton closeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonDelete"));
        UIImageButton nextPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonNextPage"));
        UIImageButton previousPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonPreviousPage"));

        public override void OnInitialize()
        {
            Player player = Main.CurrentPlayer;

            PageVisible[0] = true;

            Append(Cover);
            Cover.Append(LeftPage);
            Cover.Append(RightPage);

            PlayerBasicBeyonderInformation.Add(new List<UIElement>());

            PlayerBasicBeyonderInformation[0].Add(Page1PlayerPathSymbol);
            Page1PlayerPathSymbol.Width.Set(160f, 0f);
            Page1PlayerPathSymbol.Height.Set(160f, 0f);
            Page1PlayerPathSymbol.HAlign = 0.5f;
            Page1PlayerPathSymbol.Top.Set(120f, 0f);

            PlayerBasicBeyonderInformation[0].Add(Page1PlayerPathInformation);
            Page1PlayerPathInformation.HAlign = 0.5f;
            Page1PlayerPathInformation.Top.Set(0, 0.5f);

            PlayerBasicBeyonderInformation[0].Add(Page1PlayerSequenceInformation);
            Page1PlayerSequenceInformation.HAlign = 0.5f;
            Page1PlayerSequenceInformation.Top.Set(30f, 0.5f);

            PlayerBasicBeyonderInformation[0].Add(Page1PlayerSequenceNameInformation);
            Page1PlayerSequenceNameInformation.HAlign = 0.5f;
            Page1PlayerSequenceNameInformation.Top.Set(60f, 0.5f);

            NoteBookMenu.Add(new List<UIElement>());

            NoteBookMenu[0].Add(Page2MenuTitle);
            Page2MenuTitle.Top.Set(80f, 0);
            Page2MenuTitle.Left.Set(170f, 0);

            NoteBookMenu[0].Add(Page2MenuPlayerBeyonderAbilities);
            Page2MenuPlayerBeyonderAbilities.Top.Set(150f, 0);
            Page2MenuPlayerBeyonderAbilities.MarginLeft = 60f;
            Page2MenuPlayerBeyonderAbilities.OnClick += (evt, element) => {
                PageVisible[currentPageNumber] = false;
                currentPageNumber = 1;
                PageVisible[currentPageNumber] = true;
            };

            NoteBookMenu[0].Add(Page2MenuPotionFormula);
            Page2MenuPotionFormula.Top.Set(200f, 0);
            Page2MenuPotionFormula.MarginLeft = 60f;
            Page2MenuPotionFormula.OnClick += (evt, element) => {
                PageVisible[currentPageNumber] = false;
                currentPageNumber = (PlayerBasicBeyonderInformation.Count +
                                     NoteBookMenu.Count +
                                     PlayerAbilityThumbNailSection.Count +
                                     PlayerAbilityDetailedPageSection.Count) / 2;
                PageVisible[currentPageNumber] = true;
            };

            NoteBookMenu[0].Add(Page2MenuPlayerNotes);
            Page2MenuPlayerNotes.Top.Set(250f, 0);
            Page2MenuPlayerNotes.MarginLeft = 60f;
            Page2MenuPlayerNotes.OnClick += (evt, element) => {
                PageVisible[currentPageNumber] = false;
                currentPageNumber = (PlayerBasicBeyonderInformation.Count +
                                     NoteBookMenu.Count +
                                     PlayerAbilityThumbNailSection.Count +
                                     PlayerAbilityDetailedPageSection.Count +
                                     PotionFormulaThumbNailSection.Count +
                                     PotionFormulaDetailedPageSection.Count) / 2;
                PageVisible[currentPageNumber] = true;
            };

            PlayerAbilityThumbNailSection.Add(new List<UIElement>());

            UIText PlayerAbilitySectionTitleText = new("Beyonder Abilities", 1.5f);
            PlayerAbilityThumbNailSection[0].Add(PlayerAbilitySectionTitleText);
            PlayerAbilitySectionTitleText.HAlign = 0.5f;
            PlayerAbilitySectionTitleText.VAlign = 0.5f;

            UIText BeyonderAbilitiesPanelEditor = new("Edit Beyonder Abilities Panel");
            PlayerAbilityThumbNailSection[0].Add(BeyonderAbilitiesPanelEditor);
            BeyonderAbilitiesPanelEditor.Top.Set(30f, 0.5f);
            BeyonderAbilitiesPanelEditor.HAlign = 0.5f;

            PlayerAbilityThumbNailSection.Add(new List<UIElement>());

            PlayerAbilityThumbNailSection[1].Add(BeyonderAbilityThumbNails);
            BeyonderAbilityThumbNails.Width.Set(320f, 0f);
            BeyonderAbilityThumbNails.Height.Set(400f, 0f);
            BeyonderAbilityThumbNails.Top.Set(90f, 0f);
            BeyonderAbilityThumbNails.HAlign = 0.5f;

            PlayerAbilityThumbNailScrollbar = new("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/NotebookScrollBar", BeyonderAbilityThumbNails);
            PlayerAbilityThumbNailScrollbar.Height.Set(400f, 0f);
            PlayerAbilityThumbNailScrollbar.Width.Set(15f, 0f);
            PlayerAbilityThumbNailScrollbar.Left.Set(-60f, 1f);
            PlayerAbilityThumbNailScrollbar.VAlign = 0.5f;
            PlayerAbilityThumbNailSection[1].Add(PlayerAbilityThumbNailScrollbar);
            BeyonderAbilityThumbNails.SetScrollbar(PlayerAbilityThumbNailScrollbar);

            PotionFormulaThumbNailSection.Add(new List<UIElement>());

            UIText PotionFormulaSectionTitleText = new("Potion Formulas", 1.5f);
            PotionFormulaThumbNailSection[0].Add(PotionFormulaSectionTitleText);
            PotionFormulaSectionTitleText.HAlign = 0.5f;
            PotionFormulaSectionTitleText.VAlign = 0.5f;

            PotionFormulaThumbNailSection.Add(new List<UIElement>());

            PlayerNoteSection.Add(new List<UIElement>());

            closeButton.Width.Set(32f, 0f);
            closeButton.Height.Set(32f, 0f);
            closeButton.Left.Set(-90f, 1f);
            closeButton.Top.Set(30f, 0f);
            closeButton.OnClick += (evt, element) => {
                Visible = false;
            };
            Cover.Append(closeButton);

            nextPageButton.Width.Set(32f, 0f);
            nextPageButton.Height.Set(32f, 0f);
            nextPageButton.Left.Set(381f, 0.5f);
            nextPageButton.Top.Set(258f, 0.5f);
            nextPageButton.SetVisibility(1.0f, 0.8f);
            nextPageButton.OnClick += (evt, element) => {
                if (currentPageNumber < PageVisible.Length - 1) {
                    PageVisible[currentPageNumber] = false;
                    currentPageNumber += 1;
                    PageVisible[currentPageNumber] = true;
                }
            };
            Cover.Append(nextPageButton);

            previousPageButton.Width.Set(32f, 0f);
            previousPageButton.Height.Set(32f, 0f);
            previousPageButton.Left.Set(-413f, 0.5f);
            previousPageButton.Top.Set(258f, 0.5f);
            previousPageButton.SetVisibility(1.0f, 0.8f);
            previousPageButton.OnClick += (evt, element) => {
                if (currentPageNumber > 0) {
                    PageVisible[currentPageNumber] = false;
                    currentPageNumber -= 1;
                    PageVisible[currentPageNumber] = true;
                }
            };
            Cover.Append(previousPageButton);

            List<UIElement>[] initialPages = ToPage(currentPageNumber);

            LeftPage.Append(LeftPageNumber);
            LeftPageNumber.SetText((2 * currentPageNumber + 1).ToString());
            LeftPageNumber.HAlign = 0.5f;
            LeftPageNumber.Top.Set(-60f, 1f);

            RightPage.Append(RightPageNumber);
            RightPageNumber.SetText((2 * currentPageNumber + 2).ToString());
            RightPageNumber.HAlign = 0.5f;
            RightPageNumber.Top.Set(-60f, 1f);

            for (int i = 0; i < initialPages[0].Count; i++) {
                LeftPage.Append(initialPages[0][i]);
            }

            for (int i = 0; i < initialPages[1].Count; i++) {
                RightPage.Append(initialPages[1][i]);
            }
        }

        public override void Update(GameTime gametime) {

            bool needsUpdate = false;

            LordOfTheMysteriesModPlayer player = Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>();

            // If the player is viewing page 1 and page 2, then update the player's information based on
            // player status accordingly.
            if (PageVisible[0]) {
                Page1PlayerPathInformation.SetText("Pathway: " + player.Pathway);
                Page1PlayerSequenceInformation.SetText("Sequence: " + player.Sequence.ToString());
                Page1PlayerSequenceNameInformation.SetText(player.SequenceName);
                if (!player.Pathway.Equals("")) {
                    Page1PlayerPathSymbol.SetImage(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/Buffs/" + CapitalizeAfterSpace(player.SequenceName) + "Buff"));
                } else {
                    Page1PlayerPathSymbol.SetImage(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/TestImage"));
                }
            }

            // If the player is viewing beyonder abilities thumbnail/menu section, then update the corresponding
            // information based on player beyonder abilities accordingly
            if (CheckSectionPageVisibility(ToPageVisibilityIndex(0, PlayerAbilityThumbNailSection), 255)) {

                UIList PlayerAbilityThumbNails = (UIList)PlayerAbilityThumbNailSection[1][0];
                Dictionary<string, Action<Player>> playerAbilityDictionary = player.AbilityList;
                // int currentAbilitySectionIndex = 1;

                // Check every beyonder ability thumbnail. If there exists a thumbnail whose corresponding beyonder
                // ability is not currently owned by the player, then remove the thumbnail from Pages.
                for (int i = 0; i < PlayerAbilityThumbNails.Count; i++) {
                    NoteBookBeyonderAbilityThumbNail textItem = (NoteBookBeyonderAbilityThumbNail)PlayerAbilityThumbNails._items[i];
                    if (!playerAbilityDictionary.ContainsKey(textItem.GetText())) {
                        PlayerAbilityThumbNails.Remove(textItem);
                        PlayerAbilityDetailedPageSection.Remove(textItem.DetailedPage);
                        needsUpdate = true;
                    }
                }

                foreach (string key in playerAbilityDictionary.Keys) {
                    bool containsKey = false;

                    for (int i = 0; i < PlayerAbilityThumbNails.Count; i++) {
                        NoteBookBeyonderAbilityThumbNail textItem = (NoteBookBeyonderAbilityThumbNail)PlayerAbilityThumbNails._items[i];
                        if (textItem is not null && textItem.GetText().Equals(key)) {
                            containsKey = true;
                            break;
                        }
                    }

                    if (!containsKey) {

                        List<UIElement> newPlayerAbilityDetailedPage = new();
                        NoteBookBeyonderAbilityThumbNail AbilityTextPanel = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/BeyonderAbilityThumbnail"), key, newPlayerAbilityDetailedPage);
                        PlayerAbilityThumbNails.Add(AbilityTextPanel);

                        PlayerAbilityDetailedPageSection.Add(AbilityTextPanel.DetailedPage);

                        AbilityTextPanel.Width.Set(320f, 0f);
                        AbilityTextPanel.Height.Set(100f, 0f);
                        AbilityTextPanel.SetVisibility(1f, 0.8f);
                        AbilityTextPanel.HAlign = 0.5f;

                        AbilityTextPanel.OnClick += (evt, element) => {
                            PageVisible[currentPageNumber] = false;
                            currentPageNumber = ToPageVisibilityIndex(PlayerAbilityDetailedPageSection.IndexOf(AbilityTextPanel.DetailedPage), PlayerAbilityDetailedPageSection);
                            PageVisible[currentPageNumber] = true;
                        };

                        UIImage image = new(ModContent.Request<Texture2D>(AbilityTextPanel.imagePath));
                        AbilityTextPanel.Append(image);
                        image.Height.Set(64f, 0f);
                        image.Width.Set(64f, 0f);
                        image.ScaleToFit = true;
                        image.Left.Set(26f, 0f);
                        image.VAlign = 0.5f;

                        UIText title = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{AbilityTextPanel.text}.Title"));
                        AbilityTextPanel.Append(title);
                        title.Left.Set(150f, 0f);
                        title.VAlign = 0.5f;

                        // Add the detailed page to the notebook
                        AppendDetailedBeyonderAbilityPage(AbilityTextPanel, key);

                        needsUpdate = true;
                    }
                }
            }

            // Update the displayed page when the current page is changed.
            if (currentPageNumber < PageVisible.Length && (PageNumber != currentPageNumber || needsUpdate)) {
                LeftPage.RemoveAllChildren();
                RightPage.RemoveAllChildren();

                List<UIElement>[] toAppend = ToPage(currentPageNumber);

                foreach (UIElement element in toAppend[0]) {
                    LeftPage.Append(element);
                }
                LeftPage.Append(LeftPageNumber);
                LeftPageNumber.SetText((2 * currentPageNumber + 1).ToString());
                LeftPageNumber.HAlign = 0.5f;
                LeftPageNumber.Top.Set(-60f, 1f);

                foreach (UIElement element in toAppend[1]) {
                    RightPage.Append(element);
                }
                RightPage.Append(RightPageNumber);
                RightPageNumber.SetText((2 * currentPageNumber + 2).ToString());
                RightPageNumber.HAlign = 0.5f;
                RightPageNumber.Top.Set(-60f, 1f);

                PageNumber = currentPageNumber;
            }

            PlayerAbilityThumbNailScrollbar.Update(Main._drawInterfaceGameTime);
        }

        /// <summary>
        /// Given a PageVisibility index, convert it to two Lists of UIElements of corresponding pages.
        /// </summary>
        /// <param name="index">A PageVisibility index</param>
        /// <returns>Returns an array of two Lists of UIElements</returns>
        private List<UIElement>[] ToPage(int index) {

            int PageNumber = 2 * index + 1;
            List<UIElement>[] toReturn = { new List<UIElement>(), new List<UIElement>() };

            if (PageNumber > 512 || PageNumber < 1) {
                return toReturn;
            }

            if (PageNumber < PlayerBasicBeyonderInformation.Count) {
                toReturn[0] = PlayerBasicBeyonderInformation[PageNumber - 1];
                toReturn[1] = PlayerBasicBeyonderInformation[PageNumber];
                return toReturn;
            } else if (PageNumber == PlayerBasicBeyonderInformation.Count) {
                toReturn[0] = PlayerBasicBeyonderInformation[PageNumber - 1];
                toReturn[1] = NoteBookMenu[0];
                return toReturn;
            }

            PageNumber -= PlayerBasicBeyonderInformation.Count;

            if (PageNumber < NoteBookMenu.Count) {
                toReturn[0] = NoteBookMenu[PageNumber - 1];
                toReturn[1] = NoteBookMenu[PageNumber];
                return toReturn;
            } else if (PageNumber == NoteBookMenu.Count) {
                toReturn[0] = NoteBookMenu[PageNumber - 1];
                toReturn[1] = PlayerAbilityThumbNailSection[0];
                return toReturn;
            }

            PageNumber -= NoteBookMenu.Count;

            if (PageNumber < PlayerAbilityThumbNailSection.Count) {
                toReturn[0] = PlayerAbilityThumbNailSection[PageNumber - 1];
                toReturn[1] = PlayerAbilityThumbNailSection[PageNumber];
                return toReturn;
            } else if (PageNumber == PlayerAbilityThumbNailSection.Count) {
                toReturn[0] = PlayerAbilityThumbNailSection[PageNumber - 1];
                if (PlayerAbilityDetailedPageSection.Count > 0) {
                    toReturn[1] = PlayerAbilityDetailedPageSection[0];
                } else {
                    toReturn[1] = PotionFormulaThumbNailSection[0];
                }
                return toReturn;
            }

            PageNumber -= PlayerAbilityThumbNailSection.Count;

            if (PageNumber < PlayerAbilityDetailedPageSection.Count) {
                toReturn[0] = PlayerAbilityDetailedPageSection[PageNumber - 1];
                toReturn[1] = PlayerAbilityDetailedPageSection[PageNumber];
                return toReturn;
            } else if (PageNumber == PlayerAbilityDetailedPageSection.Count) {
                toReturn[0] = PlayerAbilityDetailedPageSection[PageNumber - 1];
                toReturn[1] = PotionFormulaThumbNailSection[0];
                return toReturn;
            }

            PageNumber -= PlayerAbilityDetailedPageSection.Count;

            if (PageNumber < PotionFormulaThumbNailSection.Count) {
                toReturn[0] = PotionFormulaThumbNailSection[PageNumber - 1];
                toReturn[1] = PotionFormulaThumbNailSection[PageNumber];
                return toReturn;
            } else if (PageNumber == PotionFormulaThumbNailSection.Count) {
                toReturn[0] = PotionFormulaThumbNailSection[PageNumber - 1];
                if (PotionFormulaDetailedPageSection.Count > 0) {
                    toReturn[1] = PotionFormulaDetailedPageSection[0];
                } else {
                    toReturn[1] = PlayerNoteSection[0];
                }
                return toReturn;
            }

            PageNumber -= PotionFormulaThumbNailSection.Count;

            if (PageNumber < PotionFormulaDetailedPageSection.Count) {
                toReturn[0] = PotionFormulaDetailedPageSection[PageNumber - 1];
                toReturn[1] = PotionFormulaDetailedPageSection[PageNumber];
                return toReturn;
            } else if (PageNumber == PotionFormulaDetailedPageSection.Count) {
                toReturn[0] = PotionFormulaDetailedPageSection[PageNumber - 1];
                toReturn[1] = PlayerNoteSection[0];
                return toReturn;
            }

            PageNumber -= PotionFormulaDetailedPageSection.Count;

            if (PageNumber < PlayerNoteSection.Count) {
                toReturn[0] = PlayerNoteSection[PageNumber - 1];
                toReturn[1] = PlayerNoteSection[PageNumber];
                return toReturn;
            } else if (PageNumber == PlayerNoteSection.Count) {
                toReturn[0] = PlayerNoteSection[PageNumber - 1];
                return toReturn;
            }

            return toReturn;

        }

        /// <summary>
        /// Given a page index and a List, convert it to PageVisibility index.
        /// </summary>
        /// <param name="index"> The given page index</param>
        /// <param name="UIElementList"> The List that contains the page</param>
        /// <returns>Returns the corresponding PageVisibility index.</returns>
        private int ToPageVisibilityIndex(int index, List<List<UIElement>> UIElementList) {
            if (index < 0 || index > UIElementList.Count) {
                Main.NewText("Index " + index + " is out of bound.");
                return 0;
            }

            if (ReferenceEquals(UIElementList, PlayerBasicBeyonderInformation)) {
                return index / 2;
            }

            index += PlayerBasicBeyonderInformation.Count;

            if (ReferenceEquals(UIElementList, NoteBookMenu)) {
                return index / 2;
            }

            index += NoteBookMenu.Count;

            if (ReferenceEquals(UIElementList, PlayerAbilityThumbNailSection)) {
                return index / 2;
            }

            index += PlayerAbilityThumbNailSection.Count;

            if (ReferenceEquals(UIElementList, PlayerAbilityDetailedPageSection)) {
                return index / 2;
            }

            index += PlayerAbilityDetailedPageSection.Count;

            if (ReferenceEquals(UIElementList, PotionFormulaThumbNailSection)) {
                return index / 2;
            }

            index += PotionFormulaThumbNailSection.Count;

            if (ReferenceEquals(UIElementList, PotionFormulaDetailedPageSection)) {
                return index / 2;
            }

            index += PotionFormulaDetailedPageSection.Count;

            return index / 2;
        }

        /// <summary>
        /// Given a range from StartIndex to EndIndex, check whether there exists an element in this range such that 
        /// it is true.
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="EndIndex"></param>
        /// <returns>Returns true if one of the pages from StartIndex to EndIndex is visible.</returns>
        private bool CheckSectionPageVisibility(int StartIndex, int EndIndex) {
            for (int i = StartIndex; i < EndIndex; i++) {
                if (PageVisible[i]) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Capitalize every letter in the given string that is after a space and removed the spaces.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CapitalizeAfterSpace(string input) {
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++) {
                if (string.IsNullOrEmpty(words[i])) continue;
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return string.Join("", words);
        }

        private void AppendDetailedBeyonderAbilityPage(NoteBookBeyonderAbilityThumbNail AbilityTextPanel, string key) {

            LordOfTheMysteriesModPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<LordOfTheMysteriesModPlayer>();

            UIText abilityName = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Title"));
            abilityName.Height.Set(10f, 0f);
            abilityName.Width.Set(155f, 0f);
            abilityName.Top.Set(90f, 0f);
            abilityName.Left.Set(0f, 0.5f);
            AbilityTextPanel.DetailedPage.Add(abilityName);

            UIImage abilityImage = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/BeyonderAbilityImages/DefaultImage"));
            abilityImage.Height.Set(220f, 0f);
            abilityImage.Width.Set(135f, 0f);
            abilityImage.Top.Set(90f, 0f);
            abilityImage.Left.Set(60f, 0f);
            AbilityTextPanel.DetailedPage.Add(abilityImage);

            UIImage abilityDescriptionBackground = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/BeyonderAbilityDescriptionBackground"));
            abilityDescriptionBackground.Height.Set(180f, 0f);
            abilityDescriptionBackground.Width.Set(175f, 0f);
            abilityDescriptionBackground.Top.Set(130f, 0f);
            abilityDescriptionBackground.Left.Set(-15f, 0.5f);
            AbilityTextPanel.DetailedPage.Add(abilityDescriptionBackground);

            UIText abilityDescription = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Description"), textScale: 0.8f);
            abilityDescription.Height.Set(160f, 0f);
            abilityDescription.Width.Set(155f, 0f);
            abilityDescription.Top.Set(15f, 0f);
            abilityDescription.HAlign = 0.5f;
            abilityDescriptionBackground.Append(abilityDescription);

            UIImage abilitySettingBackground = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/BeyonderAbilitySettingBackground"));
            abilitySettingBackground.Height.Set(170f, 0f);
            abilitySettingBackground.Width.Set(320f, 0f);
            abilitySettingBackground.Top.Set(320f, 0f);
            abilitySettingBackground.Left.Set(60f, 0f);
            AbilityTextPanel.DetailedPage.Add(abilitySettingBackground);

            UIList abilitySettingList = new();
            abilitySettingList.Height.Set(150f, 0f);
            abilitySettingList.Width.Set(300f, 0f);
            abilitySettingList.VAlign = 0.5f;
            abilitySettingList.HAlign = 0.5f;
            abilitySettingBackground.Append(abilitySettingList);

            // Create the scroll bar
            UIScrollbar scrollbar = new();
            scrollbar.Height.Set(120f, 0f);
            scrollbar.Left.Set(-30f, 1f);
            scrollbar.VAlign = 0.5f;
            abilitySettingBackground.Append(scrollbar);
            abilitySettingList.SetScrollbar(scrollbar);

            string[] abilitySettingIndex = Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Index").Split(", ");

            if (abilitySettingIndex.Contains("Mode")) {

                UIElement ModeSettingSection = new();
                ModeSettingSection.Height.Set(32f, 0f);
                ModeSettingSection.Width.Set(240f, 0f);
                ModeSettingSection.Top.Set(20f, 0f);
                ModeSettingSection.HAlign = 0.5f;
                abilitySettingList.Add(ModeSettingSection);

                UIImageButton autoModeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonChoice"));
                autoModeButton.Height.Set(32f, 0f);
                autoModeButton.Width.Set(96f, 0f);
                autoModeButton.Top.Set(0f, 0f);
                autoModeButton.Left.Set(-96f, 0.5f);
                ModeSettingSection.Append(autoModeButton);

                UIText autoModeButtonText = new("Auto mode", textScale: 0.8f)
                {
                    VAlign = 0.5f,
                    HAlign = 0.5f
                };
                autoModeButton.Append(autoModeButtonText);

                UIImageButton manualModeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonChoice"));
                manualModeButton.Height.Set(32f, 0f);
                manualModeButton.Width.Set(96f, 0f);
                manualModeButton.Top.Set(0f, 0f);
                manualModeButton.Left.Set(0f, 0.5f);
                ModeSettingSection.Append(manualModeButton);

                UIText manualModeButtonText = new("Manual mode", textScale: 0.8f)
                {
                    VAlign = 0.5f,
                    HAlign = 0.5f
                };
                manualModeButton.Append(manualModeButtonText);

                UITextPanel<string> ModeDescription = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Mode.Manual"), textScale: 0.8f);
                ModeDescription.Height.Set(100f, 0f);
                ModeDescription.Width.Set(200f, 0f);
                ModeDescription.Top.Set(52f, 0f);
                ModeDescription.HAlign = 0.5f;
                ModeDescription.TextHAlign = 0.5f;
                ModeDescription.OverflowHidden = true;
                abilitySettingList.Add(ModeDescription);

                manualModeButton.SetVisibility(1.0f, 1.0f);

                autoModeButton.OnClick += (evt, element) => {
                    manualModeButton.SetVisibility(1.0f, 0.4f);
                    autoModeButton.SetVisibility(1.0f, 1.0f);
                    modPlayer.AbilityModeSettings[key] = true;
                    ModeDescription.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Mode.Auto"));
                };

                manualModeButton.OnClick += (evt, element) => {
                    manualModeButton.SetVisibility(1.0f, 1.0f);
                    autoModeButton.SetVisibility(1.0f, 0.4f);
                    modPlayer.AbilityModeSettings[key] = false;
                    ModeDescription.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Mode.Manual"));
                };
            }

            if (abilitySettingIndex.Contains("Target")) {

                string[] TargetSettingIndex = Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.Index").Split(", ");

                UIElement TargetSettingSection = new();
                TargetSettingSection.Height.Set(37f, 0f);
                TargetSettingSection.Width.Set(260f, 0f);
                TargetSettingSection.Top.Set(20f, 0f);
                TargetSettingSection.HAlign = 0.5f;
                abilitySettingList.Add(TargetSettingSection);

                UIText TargetSettingTitle = new("Target: ", textScale: 0.8f);
                TargetSettingTitle.Height.Set(32f, 0f);
                TargetSettingTitle.Width.Set(32f, 0f);
                TargetSettingTitle.Top.Set(5f, 0f);
                TargetSettingTitle.HAlign = 0f;
                TargetSettingSection.Append(TargetSettingTitle);

                UIImageButton previousTargetButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonPreviousChoice"));
                previousTargetButton.Height.Set(20f, 0f);
                previousTargetButton.Width.Set(11f, 0f);
                previousTargetButton.Top.Set(0f, 0f);
                previousTargetButton.Left.Set(-77f, 0.5f);
                TargetSettingSection.Append(previousTargetButton);

                UIText currentTargetChoice = new(TargetSettingIndex[0], textScale: 0.8f);
                currentTargetChoice.Height.Set(32f, 0f);
                currentTargetChoice.Width.Set(144f, 0f);
                currentTargetChoice.Top.Set(5f, 0f);
                currentTargetChoice.Left.Set(-56f, 0.5f);
                currentTargetChoice.IsWrapped = true;
                TargetSettingSection.Append(currentTargetChoice);

                UIImageButton nextTargetButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookUIImages/ButtonNextChoice"));
                nextTargetButton.Height.Set(20f, 0f);
                nextTargetButton.Width.Set(11f, 0f);
                nextTargetButton.Top.Set(0f, 0f);
                nextTargetButton.Left.Set(98f, 0.5f);
                TargetSettingSection.Append(nextTargetButton);

                UITextPanel<string> TargetDescription = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.NearestEnemy.Title"), textScale: 0.8f);
                TargetDescription.Height.Set(100f, 0f);
                TargetDescription.Width.Set(200f, 0f);
                TargetDescription.Top.Set(0f, 0f);
                TargetDescription.MarginTop = 5f;
                TargetDescription.HAlign = 0.5f;
                TargetDescription.TextHAlign = 0.5f;
                TargetDescription.OverflowHidden = true;
                abilitySettingList.Add(TargetDescription);

                previousTargetButton.OnClick += (evt, element) => {
                    if (modPlayer.AbilityTargetSettings[key] == 0) {
                        modPlayer.AbilityTargetSettings[key] = TargetSettingIndex.Length - 1;
                    } else {
                        modPlayer.AbilityTargetSettings[key]--;
                    }
                    currentTargetChoice.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.{TargetSettingIndex[modPlayer.AbilityTargetSettings[key]]}.Title"));
                    TargetDescription.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.{TargetSettingIndex[modPlayer.AbilityTargetSettings[key]]}.Description"));
                };

                nextTargetButton.OnClick += (evt, element) => {
                    if (modPlayer.AbilityTargetSettings[key] == TargetSettingIndex.Length - 1) {
                        modPlayer.AbilityTargetSettings[key] = 0;
                    } else {
                        modPlayer.AbilityTargetSettings[key]++;
                    }
                    currentTargetChoice.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.{TargetSettingIndex[modPlayer.AbilityTargetSettings[key]]}.Title"));
                    TargetDescription.SetText(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Target.{TargetSettingIndex[modPlayer.AbilityTargetSettings[key]]}.Description"));
                };
            }
        }
    }

    public class BeyonderAbilitiesPanelUI : UIState
    {
        public static bool Visible = true;

        UIImage BeyonderAbilitiesPanel = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/BeyonderAbilitiesPanelUIImages/BeyonderAbilitiesPanel"));
        UIElement[] BeyonderAbilitiesList = new UIElement[20];
        public override void OnInitialize()
        {
            BeyonderAbilitiesPanel.Width.Set(700f, 0f);
            BeyonderAbilitiesPanel.Height.Set(150f, 0f);
            BeyonderAbilitiesPanel.Top.Set(-150f, 1f);
            BeyonderAbilitiesPanel.HAlign = 0.5f;
            Append(BeyonderAbilitiesPanel);
        }

        /// <summary>
        /// Adds a beyonder ability to the beyonder abilities list.
        /// </summary>
        /// <param name="BeyonderAbilityName"> The name of a beyonder ability </param>
        /// <returns> If the beyonder ability is successfully added to the list, return true. Else, return false.</returns> 
        public bool AddBeyonderAbility(string BeyonderAbilityName) {
            //TODO
            return true;
        }

        /// <summary>
        /// Removes a beyonder ability to the beyonder abilities list.
        /// </summary>
        /// <param name="BeyonderAbilityName"> The name of a beyonder ability </param>
        /// <returns> If the beyonder ability is successfully removed from the list, return true. Else, return false.</returns> 
        public bool RemoveBeyonderAbility(string BeyonderAbilityName) {
            //TODO
            return true;
        }

        /// <summary>
        /// Removes all beyonder abilities in the beyonder abilities List.
        /// </summary>
        /// <returns>If the beyonder abilities are successfully cleared, return true. Else, return false.</returns>
        public bool ClearBeyonderAbilities() {
            //TODO
            return true;
        }
    }
}