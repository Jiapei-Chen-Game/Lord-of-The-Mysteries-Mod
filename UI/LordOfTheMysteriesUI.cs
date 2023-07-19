using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using Hjson;
using Terraria.Localization;

namespace LordOfTheMysteriesMod.UI
{
    public class NotebookLeftPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookLeftPage";
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
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookRightPage";
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
            UIImageButton noteBookButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonPlay"));

            noteBookButton.SetVisibility(1f, 0.8f);
            noteBookButton.Width.Set(96f, 0f);
            noteBookButton.Height.Set(96f, 0f);
            noteBookButton.Left.Set(-200f, 0.5f);
            noteBookButton.Top.Set(30f, 0f);

            noteBookButton.OnClick += (evt, element) => {
                NoteBookUI.currentPageNumber = 0;
                NoteBookUI.Visible = true;
            };

            Append(noteBookButton);
        }
    }

    public class NoteBookBeyonderAbilityThumbNail : UIPanel
    {
        public string imagePath;
        public string text;
        public List<UIElement> DetailedPage;
        public NoteBookBeyonderAbilityThumbNail(string text, List<UIElement> DetailedPage, string imagePath = "LordOfTheMysteriesMod/UI/UIImages/TestImage")
        {
            this.imagePath = imagePath;
            this.text = text;
            this.DetailedPage = DetailedPage;
        }

        public string GetText() {
            return text;
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

        UIImage Cover = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookCover"));

        NotebookLeftPage LeftPage = new();
        NotebookRightPage RightPage = new();
        UIText LeftPageNumber = new("1");
        UIText RightPageNumber = new("2");

        UIImage Page1PlayerPathSymbol = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/TestImage"));
        UIText Page1PlayerPathInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceNameInformation = new("To Be Loaded");

        UIText Page2MenuTitle = new("Menu", large: true);
        UIText Page2MenuPlayerBeyonderAbilities = new("1. My Beyonder Abilities");
        UIText Page2MenuPotionFormula = new("2. Potion Formulas");
        UIText Page2MenuPlayerNotes = new("3. My Notes");

        UIImageButton closeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonDelete"));
        UIImageButton nextPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonNextPage"));
        UIImageButton previousPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonPreviousPage"));

        public override void OnInitialize()
        {
            Player player = Main.CurrentPlayer;

            PageVisible[0] = true;

            Cover.Width.Set(900f, 0f);
            Cover.Height.Set(600f, 0f);
            Cover.Left.Set(-450f, 0.5f);
            Cover.Top.Set(-300f, 0.5f);
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
            Page2MenuTitle.HAlign = 0.5f;

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

            PlayerAbilityThumbNailSection.Add(new List<UIElement>());

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

            /// <summary>
            /// When there is a change on the page that is displayed, set this value to true. Otherwise false.
            /// </summary>
            bool needsUpdate = false;

            /// <summary>
            /// The player who is using this UI system
            /// </summary>
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
                Dictionary<string, Action<Player>> playerAbilityDictionary = player.AbilityList;
                int currentAbilitySectionIndex = 1;

                // Check every beyonder ability thumbnail. If there exists a thumbnail whose corresponding beyonder
                // ability is not currently owned by the player, then remove the thumbnail from Pages.
                for (int i = 1; i < PlayerAbilityThumbNailSection.Count; i++) {
                    int removeNumber = PlayerAbilityThumbNailSection[i].RemoveAll(item =>
                    {
                        NoteBookBeyonderAbilityThumbNail textItem = (NoteBookBeyonderAbilityThumbNail)item;
                        // When the thumbnail is removed from Pages, its corresponding detailed page should also
                        // be removed.
                        if (!playerAbilityDictionary.ContainsKey(textItem.GetText())) {
                            PlayerAbilityDetailedPageSection.Remove(textItem.DetailedPage);
                            return true;
                        }
                        return false;
                    });
                    // If there is any thumbnail removed, then we need an update
                    if (removeNumber > 0) {
                        needsUpdate = true;
                        if (PlayerAbilityThumbNailSection[i].Count == 0 && i > 1) {
                            PlayerAbilityThumbNailSection.RemoveAt(i);
                        }
                    }
                }

                foreach (string key in playerAbilityDictionary.Keys) {
                    bool containsKey = false;

                    for (int i = 1; i < PlayerAbilityThumbNailSection.Count; i++) {
                        foreach (NoteBookBeyonderAbilityThumbNail element in PlayerAbilityThumbNailSection[i].Cast<NoteBookBeyonderAbilityThumbNail>()) {
                            if (element is not null && element.GetText().Equals(key)) {
                                containsKey = true;
                                break;
                            }
                        }
                        if (containsKey) {
                            break;
                        }
                    }

                    if (!containsKey) {

                        //Add a beyonder ability thumbnail to the notebook
                        if (PlayerAbilityThumbNailSection[currentAbilitySectionIndex].Count == 4) {
                            int previousAbilitySectionIndex = currentAbilitySectionIndex;
                            for (int i = currentAbilitySectionIndex; i < PlayerAbilityThumbNailSection.Count; i++) {
                                if (PlayerAbilityThumbNailSection[i].Count < 4) {
                                    currentAbilitySectionIndex = i;
                                    break;
                                }
                            }
                            if (currentAbilitySectionIndex == previousAbilitySectionIndex) {
                                PlayerAbilityThumbNailSection.Add(new List<UIElement>());
                                currentAbilitySectionIndex = PlayerAbilityThumbNailSection.Count - 1;
                            }
                        }

                        List<UIElement> newPlayerAbilityDetailedPage = new();
                        NoteBookBeyonderAbilityThumbNail AbilityTextPanel = new(key, newPlayerAbilityDetailedPage);

                        PlayerAbilityDetailedPageSection.Add(AbilityTextPanel.DetailedPage);

                        AbilityTextPanel.Width.Set(320f, 0f);
                        AbilityTextPanel.Height.Set(100f, 0f);
                        AbilityTextPanel.MarginLeft = 60f;

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
                        image.Left.Set(10f, 0f);
                        image.VAlign = 0.5f;

                        UIText title = new(AbilityTextPanel.text);
                        AbilityTextPanel.Append(title);
                        title.Left.Set(100f, 0f);
                        title.VAlign = 0.5f;

                        AbilityTextPanel.Top.Set(100f * PlayerAbilityThumbNailSection[currentAbilitySectionIndex].Count + 90f, 0);
                        PlayerAbilityThumbNailSection[currentAbilitySectionIndex].Add(AbilityTextPanel);

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

        /// <summary>
        /// Return the total page number.
        /// </summary>
        /// <returns>Return the total page number.</returns> 
        // public int GetTotalPageNumber() {
        //     int totalPageNumber = PlayerBasicBeyonderInformation.Count +
        //                           NoteBookMenu.Count +
        //                           PlayerAbilityThumbNailSection.Count +
        //                           PlayerAbilityDetailedPageSection.Count +
        //                           PotionFormulaThumbNailSection.Count +
        //                           PotionFormulaDetailedPageSection.Count +
        //                           PlayerNoteSection.Count;
        //     return totalPageNumber;
        // }

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

            UITextPanel<string> abilityDescription = new(Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Description"), textScale: 0.8f);
            abilityDescription.Height.Set(180f, 0f);
            abilityDescription.Width.Set(175f, 0f);
            abilityDescription.Top.Set(130f, 0f);
            abilityDescription.Left.Set(-15f, 0.5f);
            abilityDescription.TextHAlign = 0f;
            abilityDescription.OverflowHidden = true;
            AbilityTextPanel.DetailedPage.Add(abilityDescription);

            UIPanel abilitySetting = new();
            abilitySetting.Height.Set(170f, 0f);
            abilitySetting.Width.Set(320f, 0f);
            abilitySetting.Top.Set(320f, 0f);
            abilitySetting.Left.Set(60f, 0f);
            AbilityTextPanel.DetailedPage.Add(abilitySetting);

            UIList abilitySettingList = new();
            abilitySettingList.Height.Set(170f, 0f);
            abilitySettingList.Width.Set(320f, 0f);
            abilitySettingList.VAlign = 0.5f;
            abilitySettingList.HAlign = 0.5f;
            abilitySetting.Append(abilitySettingList);

            // Create the scroll bar
            UIScrollbar scrollbar = new();
            scrollbar.Height.Set(120f, 0f);
            scrollbar.HAlign = 1f;
            scrollbar.VAlign = 0.5f;
            scrollbar.SetView(120f, 1200f);
            abilitySetting.Append(scrollbar);
            abilitySettingList.SetScrollbar(scrollbar);

            string[] abilitySettingIndex = Language.GetTextValue($"Mods.LordOfTheMysteriesMod.BeyonderAbilities.{key}.Settings.Index").Split(' ');

            if (abilitySettingIndex.Contains("Mode")) {

                UIElement ModeSettingSection = new();
                ModeSettingSection.Height.Set(32f, 0f);
                ModeSettingSection.Width.Set(240f, 0f);
                ModeSettingSection.Top.Set(20f, 0f);
                ModeSettingSection.HAlign = 0.5f;
                abilitySettingList.Add(ModeSettingSection);
                
                UIImageButton autoModeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonChoice"));
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

                UIImageButton manualModeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonChoice"));
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
                ModeDescription.Width.Set(220f, 0f);
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
        }
    }
}