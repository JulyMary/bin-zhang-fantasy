using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;

#if WPF
namespace Syncfusion.Windows.Shared
#endif

#if SILVERLIGHT
namespace Syncfusion.Windows.Tools.Controls
#endif
{
    internal class MaskHandler
    {
        public static MaskHandler maskHandler = new MaskHandler();
       
        public bool MatchWithMask(MaskedTextBox maskedTextBox, string text)
        {
            if (maskedTextBox.IsReadOnly)
                return true;

            if (!string.IsNullOrEmpty(maskedTextBox.Mask))
            {
                #region Mask
                if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                {
                    int currentcaretPosition = maskedTextBox.SelectionStart;
                    if (currentcaretPosition == maskedTextBox.Text.Length)
                        return true;
                    while (currentcaretPosition < maskedTextBox.Text.Length)
                    {
                        if (maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter != null)
                        {
                            if (Regex.IsMatch(text, maskedTextBox.CharCollection[currentcaretPosition].RegExpression))
                            {
                                string temp = maskedTextBox.MaskedText;
                                if (maskedTextBox.CharCollection[currentcaretPosition].IsUpper != null)
                                {
                                    if (maskedTextBox.CharCollection[currentcaretPosition].IsUpper == true)
                                    {
                                        text = text.ToUpper();
                                    }
                                    else
                                    {
                                        text = text.ToLower();
                                    }
                                }
                                temp = temp.Remove(currentcaretPosition, 1);
                                temp = temp.Insert(currentcaretPosition, text);
                                maskedTextBox.MaskedText = temp;
                                maskedTextBox.SelectionStart = currentcaretPosition + 1;
                                maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter = false;
                                currentcaretPosition++;
                                return true;
                            }
                            else
                            {
                                currentcaretPosition++;
                                return true;
                            }
                        }
                        else
                        {
                            maskedTextBox.SelectionStart = currentcaretPosition + 1;
                            currentcaretPosition++;
                            //return true;
                        }
                    }
                    maskedTextBox.OnValidated(EventArgs.Empty);
                    return true;
                }
                else
                {
                    return true;
                }
                #endregion
            }
            else
            {
                #region RegEx
                if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                {
                    string maskedText = maskedTextBox.MaskedText;
                    maskedText.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectionLength);
                    if (maskedText.Length == maskedTextBox.SelectionStart)
                    {
                        maskedText = maskedText + text;//.Insert(maskedTextBox.SelectionStart, text);
                    }
                    else if (maskedTextBox.SelectionStart == 0)
                    {
                        maskedText = text;
                    }
                    else
                    {
                        maskedText.Insert(maskedTextBox.SelectionStart, text);
                    }

                    if (maskedTextBox.StringValidation == StringValidation.OnKeyPress)
                    {
                        if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                        {
                            string validationerror = "";
                            bool validationstatus = true;

                            validationstatus = Regex.IsMatch(maskedText, maskedTextBox.ValidationString);
                            string message = validationstatus ? "String validation succeeded" : "String validation failed";

                            if (!validationstatus)
                            {
                                if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
                                {

                                    MessageBox.Show(message, "Invalid value", MessageBoxButton.OK);
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                }
                            }
                            return false;
                        }
                    }
                    return false;
                }
                else
                {
                    return true;
                }
                #endregion
            }
        }

        public ObservableCollection<CharacterProperties> CreateRegularExpression(MaskedTextBox maskedTextBox)
        {
            bool mEscapeSequence = false;
            bool? mIsUpper = null;
            var charCollection = new ObservableCollection<CharacterProperties>();
            string mask = maskedTextBox.Mask;
            //CultureInfo culture = MaskedTextBoxAdv.Culture ?? CultureInfo.CurrentCulture;
            CultureInfo culture = maskedTextBox.GetCulture();
            if(mask!=null)
            {
                foreach (var ch in mask.ToString())
                {
                    CharacterProperties p;
                    switch (ch)
                    {
                        case '0':
                            p = new CharacterProperties
                            {
                                RegExpression = mEscapeSequence ? ch.ToString() : @"\d",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };

                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case '9':
                            p = new CharacterProperties
                            {
                                //RegExpression = mEscapeSequence ? ch.ToString() : @"[\d]?",
                                RegExpression = mEscapeSequence ? ch.ToString() : @"[\s\d]",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };

                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case '#':
                            p = new CharacterProperties
                            {
                                //RegExpression = mEscapeSequence ? ch.ToString() : @"[\d+-]?",
                                RegExpression = mEscapeSequence ? ch.ToString() : @"[\s\d+-]",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case 'L':
                            p = new CharacterProperties
                            {
                                RegExpression = mEscapeSequence ? ch.ToString() : @"[a-zA-Z]",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case '?':
                            p = new CharacterProperties
                            {
                                //RegExpression = mEscapeSequence ? ch.ToString() : @"[a-zA-Z]?",
                                RegExpression = mEscapeSequence ? ch.ToString() : @"[\sa-zA-Z]",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case '&':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence ? ch.ToString() : @"[\p{Ll}\p{Lu}\p{Lt}\p{Lm}\p{Lo}]",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case 'C':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence ? ch.ToString() : @"[\s\p{Ll}\p{Lu}\p{Lt}\p{Lm}\p{Lo}]?",
                                //mEscapeSequence ? ch.ToString() : @"[\p{Ll}\p{Lu}\p{Lt}\p{Lm}\p{Lo}]?",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case 'A':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence ? ch.ToString() : @"\w",
                                IsLiteral = mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case ',':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence
                                        ? ch.ToString()
                                        : culture.NumberFormat.NumberGroupSeparator,
                                IsLiteral = true,
                                // mEscapeSequence,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case '.':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence
                                        ? ch.ToString()
                                        : culture.NumberFormat.NumberDecimalSeparator,
                                IsLiteral = true,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;

                        case ':':
                            p = new CharacterProperties
                            {
                                IsLiteral = true,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            if (mEscapeSequence)
                            {
                                p.RegExpression = ch.ToString();
                            }
                            else
                            {
                                if (maskedTextBox.TimeSeparator != string.Empty)
                                {
                                    p.RegExpression = maskedTextBox.TimeSeparator[0].ToString();
                                }
                                else
                                {
                                    p.RegExpression = ":";
                                }
                            }
                            //p.RegExpression = mEscapeSequence ? ch.ToString() : ":";
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case '/':
                            p = new CharacterProperties
                            {
                                //RegExpression = mEscapeSequence ? ch.ToString() : "/",
                                IsLiteral = true,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            if (mEscapeSequence)
                            {
                                p.RegExpression = ch.ToString();
                            }
                            else
                            {
                                if (maskedTextBox.TimeSeparator != string.Empty)
                                {
                                    p.RegExpression = maskedTextBox.DateSeparator[0].ToString();
                                }
                                else
                                {
                                    p.RegExpression = "/";
                                }
                            }
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case '$':
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    mEscapeSequence
                                        ? ch.ToString()
                                        : culture.NumberFormat.CurrencySymbol,
                                IsLiteral = true,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                        case '\\':
                            switch (mEscapeSequence)
                            {
                                case false:
                                    mEscapeSequence = true;
                                    break;
                                default:
                                    p = new CharacterProperties
                                    {
                                        RegExpression =
                                            ch.ToString(),
                                        IsLiteral = true,
                                        IsUpper = mIsUpper,
                                        IsPromptCharacter = null
                                    };
                                    charCollection.Add(p);
                                    mEscapeSequence = false;
                                    break;
                            }
                            break;
                        case '>':
                            mIsUpper = true;
                            break;
                        case '<':
                            mIsUpper = false;
                            break;
                        default:
                            p = new CharacterProperties
                            {
                                RegExpression =
                                    ch.ToString(),
                                IsLiteral = true,
                                IsUpper = mIsUpper,
                                IsPromptCharacter = null
                            };
                            charCollection.Add(p);
                            mEscapeSequence = false;
                            break;
                    }
                }
            }
            return charCollection;
        }

        public string CreateDisplayText(MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.Mask != string.Empty)
            {
                #region Mask
                int valuecount = 0;
                string displayText = string.Empty;
                for (int i = 0; i < maskedTextBox.CharCollection.Count; i++)
                {
                    var charProperty = maskedTextBox.CharCollection[i];
                    if (charProperty.IsLiteral)
                    {
                        displayText += charProperty.IsUpper == null
                                           ? charProperty.RegExpression.ToString()
                                           : (charProperty.IsUpper == true
                                                  ? charProperty.RegExpression.ToUpper()
                                                  : charProperty.RegExpression.ToLower());
                        charProperty.IsPromptCharacter = null;
                        //valuecount++;
                    }
                    else
                    {
                        char valuechar;
                        if (maskedTextBox.mValue != null)
                        {
                            if (valuecount < maskedTextBox.mValue.ToString().Length)
                            {
                                valuechar = maskedTextBox.mValue.ToString()[valuecount];
                                string pattern = charProperty.RegExpression;
                                bool match = Regex.IsMatch(valuechar.ToString(), pattern);
                                if (match)
                                {
                                    displayText += charProperty.IsUpper == null
                                                       ? valuechar.ToString()
                                                       : (charProperty.IsUpper == true
                                                              ? Char.ToUpper(valuechar).ToString()
                                                              : Char.ToLower(valuechar).ToString());
                                    valuecount++;
                                    charProperty.IsPromptCharacter = false;
                                }
                                else
                                {
                                    displayText += maskedTextBox.PromptChar;
                                    charProperty.IsPromptCharacter = true;

                                    for (int j = i + 1; j < maskedTextBox.CharCollection.Count; j++)
                                    {
                                        charProperty = maskedTextBox.CharCollection[j];
                                        if (charProperty.IsLiteral)
                                        {
                                            displayText += charProperty.IsUpper == null
                                                               ? charProperty.RegExpression.ToString()
                                                               : (charProperty.IsUpper == true
                                                                      ? charProperty.RegExpression.ToUpper()
                                                                      : charProperty.RegExpression.ToLower());
                                            charProperty.IsPromptCharacter = null;
                                        }
                                        else
                                        {
                                            displayText += charProperty.IsLiteral
                                                               ? charProperty.RegExpression
                                                               : maskedTextBox.PromptChar.ToString();
                                        }
                                    }
                                    return displayText;
                                }
                            }
                            else
                            {
                                displayText += maskedTextBox.PromptChar;
                                charProperty.IsPromptCharacter = true;
                            }
                        }
                        else
                        {
                            displayText += maskedTextBox.PromptChar;
                            charProperty.IsPromptCharacter = true;
                        }
                    }
                }
                return displayText;
                #endregion
            }
            else
            {
                return maskedTextBox.Value;
            }
        }

        //public string CreateValueFromText(MaskedTextBox maskedTextBox)
        //{
        //    if (maskedTextBox.Mask != string.Empty)
        //    {
        //        string valueText = string.Empty;
        //        for (int i = 0; i < maskedTextBox.CharCollection.Count; i++)
        //        {
        //            var charProperty = maskedTextBox.CharCollection[i];
        //            if (charProperty.IsLiteral)
        //                valueText += maskedTextBox.Text[i];
        //            else
        //            {
        //                if (charProperty.IsPromptCharacter == false)
        //                    valueText += maskedTextBox.Text[i];
        //            }
        //        }
        //        return valueText;
        //    }
        //    else
        //    {
        //        return maskedTextBox.Text;
        //    }
        //}

        public string CreateValueFromText(MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.Mask != string.Empty)
            {
                string valueText = string.Empty;
                for (int i = 0; i < maskedTextBox.CharCollection.Count; i++)
                {
                    var charProperty = maskedTextBox.CharCollection[i];
                    if (charProperty.IsLiteral)
                    {
                        if (maskedTextBox.TextMaskFormat == MaskFormat.IncludeLiterals || maskedTextBox.TextMaskFormat == MaskFormat.IncludePromptAndLiterals)
                            valueText += maskedTextBox.Text[i];
                    }
                    else if (charProperty.IsPromptCharacter == false)
                    {
                        valueText += maskedTextBox.Text[i];
                    }
                    else
                    {
                        if (maskedTextBox.TextMaskFormat == MaskFormat.IncludePrompt || maskedTextBox.TextMaskFormat == MaskFormat.IncludePromptAndLiterals)
                        {
                            valueText += maskedTextBox.Text[i];
                        }
                    }
                    //if (charProperty.IsLiteral)
                    //    valueText += maskedTextBox.Text[i];
                    //else
                    //{
                    //    if (charProperty.IsPromptCharacter == false)
                    //        valueText += maskedTextBox.Text[i];
                    //}
                }
                return valueText;
            }
            else
            {
                return maskedTextBox.Text;
            }
        }

        public bool HandleKeyDown(MaskedTextBox maskedTextBox, KeyEventArgs eventArgs)
        {
            switch (eventArgs.Key)
            {
                case Key.None:
                    break;
#if WPF
                case Key.Cancel:
                    break;
#endif
                case Key.Back:
                    return HandleBackSpaceKey(maskedTextBox);
                case Key.Tab:
                    break;
#if WPF
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    if (maskedTextBox.EnterToMoveNext)
                    {
                        FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                        TraversalRequest request = new TraversalRequest(focusDirection);
                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                        {
                            elementWithFocus.MoveFocus(request);
                        }

                        return true;
                    }
                    else if (maskedTextBox.Mask == string.Empty)
                        eventArgs.Handled = false;
                    else
                        return true;
                    break;
                case Key.Pause:
                    break;
                case Key.Capital:
                    break;
                case Key.KanaMode:
                    break;
                case Key.JunjaMode:
                    break;
                case Key.FinalMode:
                    break;
                case Key.HanjaMode:
                    break;
#endif
#if SILVERLIGHT
                case Key.Enter:
                    if (maskedTextBox.EnterToMoveNext)
                    {
                        if ((maskedTextBox.SelectionStart + 1) <= maskedTextBox.Text.Length)
                        {
                            maskedTextBox.SelectionStart = maskedTextBox.SelectionStart + 1;
                            return true;
                        }
                    }
                    break;
#endif
#if SILVERLIGHT
                case Key.Shift:
                    break;
                case Key.Ctrl:
                    break;
                case Key.Alt:
                    break;
#endif
#if SILVERLIGHT
                case Key.CapsLock:
                    break;
#endif
#if WPF
                case Key.ImeConvert:
                    break;
                case Key.ImeNonConvert:
                    break;
                case Key.ImeAccept:
                    break;

                case Key.ImeModeChange:
                    break;
                case Key.Space:
                    return true;
                    
                case Key.Prior:
                    break;
                case Key.Next:
                    break;
#endif
                case Key.Escape:
                    break;
#if SILVERLIGHT
                case Key.Space:
                    break;
                case Key.PageUp:
                    break;
                case Key.PageDown:
                    break;
#endif
                case Key.End:
                    break;
                case Key.Home:
                    break;
                case Key.Left:
                    break;
                case Key.Up:
                    break;
                case Key.Right:
                    break;
                case Key.Down:
                    break;
#if WPF
                case Key.Select:
                    break;
                case Key.Print:
                    break;
                case Key.Execute:
                    break;
                case Key.Snapshot:
                    break;
#endif
                case Key.Insert:
                    break;
                case Key.Delete:
                    return HandleDeleteKey(maskedTextBox);
#if WPF
                case Key.Help:
                    break;
#endif
                case Key.D0:
                    break;
                case Key.D1:
                    break;
                case Key.D2:
                    break;
                case Key.D3:
                    break;
                case Key.D4:
                    break;
                case Key.D5:
                    break;
                case Key.D6:
                    break;
                case Key.D7:
                    break;
                case Key.D8:
                    break;
                case Key.D9:
                    break;
                case Key.A:
                    break;
                case Key.B:
                    break;
                case Key.C:
                    break;
                case Key.D:
                    break;
                case Key.E:
                    break;
                case Key.F:
                    break;
                case Key.G:
                    break;
                case Key.H:
                    break;
                case Key.I:
                    break;
                case Key.J:
                    break;
                case Key.K:
                    break;
                case Key.L:
                    break;
                case Key.M:
                    break;
                case Key.N:
                    break;
                case Key.O:
                    break;
                case Key.P:
                    break;
                case Key.Q:
                    break;
                case Key.R:
                    break;
                case Key.S:
                    break;
                case Key.T:
                    break;
                case Key.U:
                    break;
                case Key.V:
                    break;
                case Key.W:
                    break;
                case Key.X:
                    break;
                case Key.Y:
                    break;
                case Key.Z:
                    break;
#if WPF
                case Key.LWin:
                    break;
                case Key.RWin:
                    break;
                case Key.Apps:
                    break;
                case Key.Sleep:
                    break;
#endif

                case Key.NumPad0:
                    break;
                case Key.NumPad1:
                    break;
                case Key.NumPad2:
                    break;
                case Key.NumPad3:
                    break;
                case Key.NumPad4:
                    break;
                case Key.NumPad5:
                    break;
                case Key.NumPad6:
                    break;
                case Key.NumPad7:
                    break;
                case Key.NumPad8:
                    break;
                case Key.NumPad9:
                    break;
                case Key.Multiply:
                    break;
                case Key.Add:
                    break;
#if WPF
                case Key.Separator:
                    break;
#endif
                case Key.Subtract:
                    break;
                case Key.Decimal:
                    break;
                case Key.Divide:
                    break;
                case Key.F1:
                    break;
                case Key.F2:
                    break;
                case Key.F3:
                    break;
                case Key.F4:
                    break;
                case Key.F5:
                    break;
                case Key.F6:
                    break;
                case Key.F7:
                    break;
                case Key.F8:
                    break;
                case Key.F9:
                    break;
                case Key.F10:
                    break;
                case Key.F11:
                    break;
                case Key.F12:
                    break;
#if WPF
                case Key.F13:
                    break;
                case Key.F14:
                    break;
                case Key.F15:
                    break;
                case Key.F16:
                    break;
                case Key.F17:
                    break;
                case Key.F18:
                    break;
                case Key.F19:
                    break;
                case Key.F20:
                    break;
                case Key.F21:
                    break;
                case Key.F22:
                    break;
                case Key.F23:
                    break;
                case Key.F24:
                    break;
                case Key.NumLock:
                    break;
                case Key.Scroll:
                    break;
                case Key.LeftShift:
                    break;
                case Key.RightShift:
                    break;
                case Key.LeftCtrl:
                    break;
                case Key.RightCtrl:
                    break;
                case Key.LeftAlt:
                    break;
                case Key.RightAlt:
                    break;
                case Key.BrowserBack:
                    break;
                case Key.BrowserForward:
                    break;
                case Key.BrowserRefresh:
                    break;
                case Key.BrowserStop:
                    break;
                case Key.BrowserSearch:
                    break;
                case Key.BrowserFavorites:
                    break;
                case Key.BrowserHome:
                    break;
                case Key.VolumeMute:
                    break;
                case Key.VolumeDown:
                    break;
                case Key.VolumeUp:
                    break;
                case Key.MediaNextTrack:
                    break;
                case Key.MediaPreviousTrack:
                    break;
                case Key.MediaStop:
                    break;
                case Key.MediaPlayPause:
                    break;
                case Key.LaunchMail:
                    break;
                case Key.SelectMedia:
                    break;
                case Key.LaunchApplication1:
                    break;
                case Key.LaunchApplication2:
                    break;
                case Key.Oem1:
                    break;
                case Key.OemPlus:
                    break;
                case Key.OemComma:
                    break;
                case Key.OemMinus:
                    break;
                case Key.OemPeriod:
                    break;
                case Key.Oem2:
                    break;
                case Key.Oem3:
                    break;
                case Key.AbntC1:
                    break;
                case Key.AbntC2:
                    break;
                case Key.Oem4:
                    break;
                case Key.Oem5:
                    break;
                case Key.Oem6:
                    break;
                case Key.Oem7:
                    break;
                case Key.Oem8:
                    break;
                case Key.Oem102:
                    break;
                case Key.ImeProcessed:
                    break;
                case Key.System:
                    break;
                case Key.OemAttn:
                    break;
                case Key.OemFinish:
                    break;
                case Key.OemCopy:
                    break;
                case Key.OemAuto:
                    break;
                case Key.OemEnlw:
                    break;
                case Key.OemBackTab:
                    break;
                case Key.Attn:
                    break;
                case Key.CrSel:
                    break;
                case Key.ExSel:
                    break;
                case Key.EraseEof:
                    break;
                case Key.Play:
                    break;
                case Key.Zoom:
                    break;
                case Key.NoName:
                    break;
                case Key.Pa1:
                    break;
                case Key.OemClear:
                    break;
                //case Key.DeadCharProcessed:
                //    break;
#endif
#if SILVERLIGHT
                case Key.Unknown:
                    break;
#endif
                default:
                    break;
                    //throw new ArgumentOutOfRangeException();
            }
            return false;
        }
        public bool HandlePaste(MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.IsReadOnly)
                return true;
            else
            {
                int currentcaretPosition = maskedTextBox.SelectionStart;
                int cliptxtlength = 0;
                string cliptext = Clipboard.GetText();
                cliptxtlength = Clipboard.GetText().Length;
                int limit = cliptxtlength;
                int currentclipcharposition = 0;
                string text = "";
                int cursor = maskedTextBox.SelectionStart;
                if (maskedTextBox.Mask != string.Empty)
                {
                    text = maskedTextBox.Text;
                    while (currentclipcharposition < limit && currentcaretPosition < text.Length)
                    {
                        if (maskedTextBox.CharCollection[currentcaretPosition].IsLiteral)
                        {
                            currentcaretPosition++;
                        }
                        else
                        {
                            if (Regex.IsMatch(cliptext[currentclipcharposition].ToString(), maskedTextBox.CharCollection[currentcaretPosition].RegExpression))
                            {
                                text = text.Remove(currentcaretPosition, 1);
                                text = text.Insert(currentcaretPosition, cliptext[currentclipcharposition].ToString());
                                maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter = false;

                                currentclipcharposition++;
                                currentcaretPosition++;
                                cursor++;

                            }
                            else
                                currentclipcharposition++;
                        }
                    }
                }
                else
                {
                    this.HandleValidationString(maskedTextBox, cliptext);

                    return true;
                }
                maskedTextBox.SelectionLength = 0;
                string str = MaskHandler.maskHandler.ValueFromMaskedText(maskedTextBox, maskedTextBox.TextMaskFormat, text, maskedTextBox.CharCollection);
                maskedTextBox.SetValue(false, str);
                maskedTextBox.MaskedText = text;
                maskedTextBox.SelectionStart = currentcaretPosition;
                return true;
            }
        }

        public bool HandleDeleteKey(MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.IsReadOnly)
                return true;

            if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
            {
                if (maskedTextBox.Mask != string.Empty)
                {
                    #region Standard
                    int currentcaretPosition = maskedTextBox.SelectionStart;
                    string text = maskedTextBox.MaskedText;

                    int len = maskedTextBox.SelectionStart + (maskedTextBox.SelectionLength == 0
                                                                  ? 1
                                                                  : maskedTextBox.SelectionLength);

                    while (currentcaretPosition <= len - 1 && currentcaretPosition < maskedTextBox.Text.Length)
                    {
                        if (maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter != null &&
                            maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter != true)
                        {
                            maskedTextBox.CharCollection[currentcaretPosition].IsPromptCharacter = true;
                            text = text.Remove(currentcaretPosition, 1);
                            text = text.Insert(currentcaretPosition, maskedTextBox.PromptChar.ToString());
                        }
                        currentcaretPosition++;
                    }
                    //int selectionstart = MaskedTextBoxAdv.SelectionStart;
                    maskedTextBox.MaskedText = text;
                    //MaskedTextBoxAdv.SelectionStart = selectionstart;
                    maskedTextBox.SelectionStart = currentcaretPosition;
                    maskedTextBox.SelectionLength = 0;
                    maskedTextBox.SetValue(null, maskHandler.CreateValueFromText(maskedTextBox));
                    maskedTextBox.OnValidated(EventArgs.Empty);
                    return true;
                    #endregion
                }
                else
                {
                    string maskedText = maskedTextBox.MaskedText;

                    if (maskedTextBox.MaskedText.Length == maskedTextBox.SelectionStart)
                    {
                        maskedTextBox.OnValidated(EventArgs.Empty);
                        return true;
                    }

                    int len = (maskedTextBox.SelectionLength == 0
                                                                  ? 1
                                                                  : maskedTextBox.SelectionLength);

                    maskedText.Remove(maskedTextBox.SelectionStart, len);
                    if (maskedTextBox.StringValidation == StringValidation.OnKeyPress)
                    {
                        if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                        {
                            string validationerror = "";
                            bool validationstatus = true;

                            validationstatus = Regex.IsMatch(maskedText, maskedTextBox.ValidationString);
                            string message = validationstatus ? "String validation succeeded" : "String validation failed";

                            if (!validationstatus)
                            {
                                if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
                                {

                                    MessageBox.Show(message, "Invalid value", MessageBoxButton.OK);
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                }
                            }
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool HandleValidationString(MaskedTextBox maskedTextBox, string text)
        {
            if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
            {
                string maskedText = maskedTextBox.MaskedText;
                maskedText.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectionLength);
                if (maskedText.Length == maskedTextBox.SelectionStart)
                {
                    maskedText = maskedText + text;//.Insert(maskedTextBox.SelectionStart, text);
                }
                else if (maskedTextBox.SelectionStart == 0)
                {
                    maskedText = text + maskedText;
                }
                else
                {
                    maskedText.Insert(maskedTextBox.SelectionStart, text);
                }

                if (maskedTextBox.StringValidation == StringValidation.OnKeyPress)
                {
                    if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                    {
                        string validationerror = "";
                        bool validationstatus = true;

                        validationstatus = Regex.IsMatch(maskedText, maskedTextBox.ValidationString);
                        string message = validationstatus ? "String validation succeeded" : "String validation failed";

                        if (!validationstatus)
                        {
                            if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
                            {

                                MessageBox.Show(message, "Invalid value", MessageBoxButton.OK);
                                maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                maskedTextBox.OnValidated(EventArgs.Empty);
                                return true;
                            }
                            else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
                            {
                                maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                maskedTextBox.OnValidated(EventArgs.Empty);
                                return true;
                            }
                            else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
                            {
                                maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                maskedTextBox.OnValidated(EventArgs.Empty);
                            }
                        }
                        maskedTextBox.Value = maskedText;
                        maskedTextBox.MaskedText = maskedText;
                        return false;
                    }
                }
                maskedTextBox.Value = maskedText;
                maskedTextBox.MaskedText = maskedText;
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HandleBackSpaceKey(MaskedTextBox maskedTextBox)
        {
            if (maskedTextBox.IsReadOnly)
                return true;

            if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
            {
                if (maskedTextBox.Mask != string.Empty)
                {
                    #region Standard
                    int currentcaretPosition = maskedTextBox.SelectionStart + (maskedTextBox.SelectionLength > 0 ? 1 : 0);
                    string text = maskedTextBox.MaskedText;

                    int len = maskedTextBox.SelectionStart + maskedTextBox.SelectionLength;

                    while (currentcaretPosition <= len && currentcaretPosition > 0)
                    {
                        if (maskedTextBox.CharCollection[currentcaretPosition - 1].IsPromptCharacter != null &&
                            maskedTextBox.CharCollection[currentcaretPosition - 1].IsPromptCharacter != true)
                        {
                            maskedTextBox.CharCollection[currentcaretPosition - 1].IsPromptCharacter = true;
                            text = text.Remove(currentcaretPosition - 1, 1);
                            text = text.Insert(currentcaretPosition - 1, maskedTextBox.PromptChar.ToString());
                        }
                        currentcaretPosition++;
                    }

                    int selectionstart = maskedTextBox.SelectionStart;
                    maskedTextBox.MaskedText = text;
                    maskedTextBox.SelectionStart = selectionstart == 0 ? 0 : selectionstart - 1;
                    maskedTextBox.SelectionLength = 0;
                    maskedTextBox.SetValue(null, MaskHandler.maskHandler.CreateValueFromText(maskedTextBox));
                    maskedTextBox.OnValidated(EventArgs.Empty);
                    return true;
                    #endregion
                }
                else
                {
                    string maskedText = maskedTextBox.MaskedText;
                    if (maskedTextBox.SelectionStart == 0 && maskedTextBox.SelectionLength == 0)
                    {
                        maskedTextBox.OnValidated(EventArgs.Empty);
                        return true;
                    }
                    else if (maskedTextBox.SelectionLength == 0)
                    {
                        if (maskedTextBox.SelectionStart == 0)
                            return true;
                        maskedText = maskedText.Remove(maskedTextBox.SelectionStart - 1, 1);
                    }
                    else
                    {
                        maskedText = maskedText.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectionLength);
                    }

                    if (maskedTextBox.StringValidation == StringValidation.OnKeyPress)
                    {
                        if (!maskedTextBox.OnValidating(new CancelEventArgs(false)))
                        {
                            string validationerror = "";
                            bool validationstatus = true;

                            validationstatus = Regex.IsMatch(maskedText, maskedTextBox.ValidationString);
                            string message = validationstatus ? "String validation succeeded" : "String validation failed";

                            if (!validationstatus)
                            {
                                if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
                                {

                                    MessageBox.Show(message, "Invalid value", MessageBoxButton.OK);
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                    return true;
                                }
                                else if (maskedTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
                                {
                                    maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(validationstatus, validationerror, maskedTextBox.ValidationString));
                                    maskedTextBox.OnValidated(EventArgs.Empty);
                                }
                            }
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// like Create Display Text
        /// </summary>
        /// <param name="maskedTextbox"></param>
        /// <param name="value"></param>
        /// <param name="maskformat"></param>
        /// <returns></returns>
        public string  CoerceValue(MaskedTextBox maskedTextbox, string value, MaskFormat maskformat)
        {
            int valuecount = 0;
            string displayText = string.Empty;
            //ObservableCollection<CharacterProperties> CharCollection = maskedTextbox.CharCollection;//this.CreateRegularExpression(maskedTextbox);
            for (int i = 0; i < maskedTextbox.CharCollection.Count; i++)
            {
                char valuechar;
                if (value != string.Empty)
                {
                    if (maskedTextbox.CharCollection[i].IsLiteral)
                    {
                        displayText += maskedTextbox.CharCollection[i].RegExpression;
                        maskedTextbox.CharCollection[i].IsPromptCharacter = null;
                    }
                    else if (maskedTextbox.CharCollection[i].IsPromptCharacter == true)
                    {
                        displayText += maskedTextbox.PromptChar;
                        maskedTextbox.CharCollection[i].IsPromptCharacter = true;
                    }
                    else
                    {
                        if (valuecount < value.Length)
                        {
                            valuechar = value.ToString()[valuecount];
                            string pattern = maskedTextbox.CharCollection[i].RegExpression;
                            bool match = Regex.IsMatch(valuechar.ToString(), pattern);
                            if (match)
                            {
                                valuecount++;
                                displayText += valuechar.ToString();
                                maskedTextbox.CharCollection[i].IsPromptCharacter = false;
                            }
                            else
                            {
                                if (valuechar == '-' || valuechar == '$' || valuechar == '>' || valuechar == '<' || valuechar == '\\' || valuechar == '.' || valuechar == ':' || valuechar == ',' || valuechar == '(' || valuechar == ')')
                                {
                                    valuecount++;
                                    i--;
                                    //displayText += value[valuecount];
                                }
                                else
                                {
                                    valuecount++;
                                    displayText += maskedTextbox.PromptChar;
                                    maskedTextbox.CharCollection[i].IsPromptCharacter = true;
                                }
                            }
                        }

                        else
                        {

                            if (maskedTextbox.CharCollection[i].IsLiteral)
                            {
                                displayText += maskedTextbox.CharCollection[i].RegExpression;
                                maskedTextbox.CharCollection[i].IsPromptCharacter = null;
                            }
                            else
                            {
                                displayText += maskedTextbox.PromptChar;
                                maskedTextbox.CharCollection[i].IsPromptCharacter = true;
                            }
                        }
                    }
                }
                else
                {
                    if (maskedTextbox.CharCollection[i].IsLiteral)
                    {                       
                        displayText += maskedTextbox.CharCollection[i].RegExpression;
                        maskedTextbox.CharCollection[i].IsPromptCharacter = null;
                    }
                    else
                    {
                        displayText += maskedTextbox.PromptChar;
                        maskedTextbox.CharCollection[i].IsPromptCharacter = true;

                        for (int j = i + 1; j < maskedTextbox.CharCollection.Count; j++)
                        {
                            //charProperty = CharCollection[j];
                            if (maskedTextbox.CharCollection[j].IsLiteral)
                            {
                                displayText += maskedTextbox.CharCollection[j].IsUpper == null
                                                   ? maskedTextbox.CharCollection[j].RegExpression.ToString()
                                                   : (maskedTextbox.CharCollection[j].IsUpper == true
                                                          ? maskedTextbox.CharCollection[j].RegExpression.ToUpper()
                                                          : maskedTextbox.CharCollection[j].RegExpression.ToLower());
                                maskedTextbox.CharCollection[j].IsPromptCharacter = null;
                            }
                            else
                            {
                                displayText += maskedTextbox.CharCollection[j].IsLiteral
                                                   ? maskedTextbox.CharCollection[j].RegExpression
                                                   : maskedTextbox.PromptChar.ToString();
                                maskedTextbox.CharCollection[j].IsPromptCharacter = true;
                            }
                        }
                        return displayText;
                    }

                }
            }
            return displayText;
        }
               







                //if (maskedTextbox.CharCollection[i].IsLiteral)
                //{
                //    displayText += maskedTextbox.CharCollection[i].IsUpper == null
                //                       ? maskedTextbox.CharCollection[i].RegExpression.ToString()
                //                       : (maskedTextbox.CharCollection[i].IsUpper == true
                //                              ? maskedTextbox.CharCollection[i].RegExpression.ToUpper()
                //                              : maskedTextbox.CharCollection[i].RegExpression.ToLower());
                //    maskedTextbox.CharCollection[i].IsPromptCharacter = null;
                //    //valuecount++;
                //}
                //else
                //{
                //    char valuechar;
                //    if (valuecount < value.ToString().Length)
                //    {
                //        valuechar = value.ToString()[valuecount];
                //        string pattern = maskedTextbox.CharCollection[i].RegExpression;
                //        bool match = Regex.IsMatch(valuechar.ToString(), pattern);
                //        if (match)
                //        {
                //            displayText += maskedTextbox.CharCollection[i].IsUpper == null
                //                               ? valuechar.ToString()
                //                               : (maskedTextbox.CharCollection[i].IsUpper == true
                //                                      ? Char.ToUpper(valuechar).ToString()
                //                                      : Char.ToLower(valuechar).ToString());
                //            valuecount++;
                //            maskedTextbox.CharCollection[i].IsPromptCharacter = false;
                //        }
                //        else
                //        {
                //            if (valuechar == maskedTextbox.PromptChar)
                //            {
                //                valuecount++;
                //                displayText += maskedTextbox.PromptChar;
                //                maskedTextbox.CharCollection[i].IsPromptCharacter = true;
                //            }
                //            else
                //            {
                //                displayText += maskedTextbox.PromptChar;
                //                maskedTextbox.CharCollection[i].IsPromptCharacter = true;

                //                for (int j = i + 1; j < maskedTextbox.CharCollection.Count; j++)
                //                {
                //                    //charProperty = CharCollection[j];
                //                    if (maskedTextbox.CharCollection[j].IsLiteral)
                //                    {
                //                        displayText += maskedTextbox.CharCollection[j].IsUpper == null
                //                                           ? maskedTextbox.CharCollection[j].RegExpression.ToString()
                //                                           : (maskedTextbox.CharCollection[j].IsUpper == true
                //                                                  ? maskedTextbox.CharCollection[j].RegExpression.ToUpper()
                //                                                  : maskedTextbox.CharCollection[j].RegExpression.ToLower());
                //                        maskedTextbox.CharCollection[j].IsPromptCharacter = null;
                //                    }
                //                    else
                //                    {
                //                        displayText += maskedTextbox.CharCollection[j].IsLiteral
                //                                           ? maskedTextbox.CharCollection[j].RegExpression
                //                                           : maskedTextbox.PromptChar.ToString();
                //                        maskedTextbox.CharCollection[j].IsPromptCharacter = true;
                //                    }
                //                }
                //                return displayText;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        displayText += maskedTextbox.PromptChar;
                //        maskedTextbox.CharCollection[i].IsPromptCharacter = true;
                //    }
                //}
        //    }
        //    return displayText;
        //}

        public string ValueFromMaskedText(MaskedTextBox maskedTextBox, MaskFormat TextMaskFormat, string maskedText, ObservableCollection<CharacterProperties> CharCollection)
        {
            if (!string.IsNullOrEmpty(maskedTextBox.Mask))
            {
                string valueText = string.Empty;
                for (int i = 0; i < CharCollection.Count; i++)
                {
                    var charProperty = CharCollection[i];
                    if (charProperty.IsLiteral)
                    {
                        if (TextMaskFormat == MaskFormat.IncludeLiterals || TextMaskFormat == MaskFormat.IncludePromptAndLiterals)
                            //valueText += maskedTextBox.Text[i];
                            if(i< maskedText.Length)
                            valueText += maskedText[i];
                    }
                    else if (charProperty.IsPromptCharacter == false)
                    {
                        //valueText += maskedTextBox.Text[i];
                        if (i < maskedText.Length)
                        valueText += maskedText[i];
                    }
                    else
                    {
                        if (TextMaskFormat == MaskFormat.IncludePrompt)
                        {
                            //valueText += maskedTextBox.Text[i];
                            if (i < maskedText.Length)
                            valueText += maskedText[i];
                        }
                    }
                }
                return valueText;
            }
            else
            {
                return maskedTextBox.Text;
            }
        }
    }
}
