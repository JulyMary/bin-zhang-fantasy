using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#if WPF
namespace Syncfusion.Windows.Shared
#endif

#if SILVERLIGHT
namespace Syncfusion.Windows.Tools.Controls
#endif
{
    internal class CurrencyValueHandler
    {
        public static CurrencyValueHandler currencyValueHandler = new CurrencyValueHandler();

        public bool MatchWithMask(CurrencyTextBox currencyTextBox, string text)
        {
            if (currencyTextBox.IsReadOnly)
                return true;

            bool minusKeyValidationflag = false;
            if (currencyTextBox.mValue == null || currencyTextBox.mValue == 0)
            {
                if (text == "-")
                {
                    currencyTextBox.minusPressed = true;
                    return true;
                }
                else if (text == "." || text == currencyTextBox.CurrencyDecimalSeparator.ToString())
                {
                    if(currencyTextBox.SelectionStart <= 1)
                    currencyTextBox.CaretIndex = currencyTextBox.CaretIndex + 2;
                }
                else
                {
                    if (currencyTextBox.minusPressed == true)
                    {
                        currencyTextBox.minusPressed = false;
                        minusKeyValidationflag = true;
                    }
                }
            }

            NumberFormatInfo numberFormat = currencyTextBox.GetCulture().NumberFormat;
           

            if (text == "-" || text == "+")
            {
                if (currencyTextBox.mValue != null)
                {
                    decimal tempVal = (decimal)currencyTextBox.mValue * -1;

                    if ((tempVal > currencyTextBox.MaxValue) && (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (currencyTextBox.MaxValueOnExceedMaxDigit)
                            tempVal = currencyTextBox.MaxValue;
                        else return true;
                    }

                    if ((tempVal < currencyTextBox.MinValue) && (currencyTextBox.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (currencyTextBox.MinValueOnExceedMinDigit)
                            tempVal = currencyTextBox.MinValue;
                        else return true;
                    }
                    currencyTextBox.MaskedText = tempVal.ToString("C", numberFormat);
                    currencyTextBox.SetValue(false, tempVal);
                }
                return true;
            }

            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            string maskedText = currencyTextBox.MaskedText;
            int separatorStart = maskedText.IndexOf(numberFormat.CurrencyDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.CurrencyDecimalSeparator.Length;

            if (text == numberFormat.CurrencyDecimalSeparator)
            {
                currencyTextBox.SelectionStart = separatorEnd;
                return true;
            }

            int caretPosition = currencyTextBox.SelectionStart;
            string unmaskedText = "";

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == currencyTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (currencyTextBox.SelectionStart + currencyTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.ToString();
                }

                if (i < maskedText.Length)
                {
                    if (char.IsDigit(maskedText[i]))
                    {
                        if (unmaskedText.Length == 0)
                        {
                            if (maskedText[i] != '0')
                            {
                                unmaskedText += maskedText[i];
                            }
                        }
                        else
                        {
                            unmaskedText += maskedText[i];
                        }
                    }
                }
            }
            selectionLength = selectionEnd - selectionStart;
            if (separatorStart < 0)
            {
                separatorStart = unmaskedText.Length;
                separatorEnd = unmaskedText.Length;
            }

            #region TextChange
            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd)
            {
                for (int decpos = separatorEnd; decpos < selectionEnd; decpos++)
                {
                    if (decpos != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        unmaskedText = unmaskedText.Insert(decpos, "0");
                    }
                }
                unmaskedText = unmaskedText.Remove(selectionStart, (separatorStart - selectionStart));
                caretPosition = selectionStart;

                unmaskedText = unmaskedText.Insert(selectionStart, text);
                caretPosition = caretPosition + text.Length;
            }

            else if (selectionStart <= separatorStart && selectionEnd < separatorEnd)
            {
                unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                caretPosition = selectionStart;

                unmaskedText = unmaskedText.Insert(selectionStart, text);
                caretPosition = caretPosition + text.Length;
            }
            else
            {
                if (selectionStart == selectionEnd)
                {
                    if (selectionStart != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(selectionStart, 1);
                        unmaskedText = unmaskedText.Insert(selectionStart, text[0].ToString());
                        if (currencyTextBox.Value > -1 && currencyTextBox.Value < 1)
                        {
                            caretPosition = selectionStart + 1;
                        }
                    }
                    else
                        return true;
                }
                else
                {
                    int textpos = 0;
                    for (int decpos = selectionStart; decpos < selectionEnd; decpos++)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        if (textpos < text.Length)
                            unmaskedText = unmaskedText.Insert(decpos, text[textpos].ToString());
                        else
                            unmaskedText = unmaskedText.Insert(decpos, "0");
                        textpos++;
                    }
                    caretPosition = selectionStart + text.Length;
                }
            }
            #endregion
          
            decimal preValue;
            if (decimal.TryParse(unmaskedText, out preValue))
            {
                if (currencyTextBox.IsNegative || minusKeyValidationflag)
                {
                    preValue = preValue * -1;
                }

                if ((preValue > currencyTextBox.MaxValue) && (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (currencyTextBox.MaxValueOnExceedMaxDigit)
                        preValue = currencyTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < currencyTextBox.MinValue) && (currencyTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (currencyTextBox.MinValueOnExceedMinDigit)
                    {                       
                        
                            preValue = currencyTextBox.MinValue;
                    }
                    else
                    {
                        currencyTextBox.checktext = currencyTextBox.checktext + text;
                        if (decimal.Parse(currencyTextBox.checktext) > currencyTextBox.MinValue)
                        {
                            currencyTextBox.Value = decimal.Parse(currencyTextBox.checktext);
                            currencyTextBox.CaretIndex = currencyTextBox.Value.ToString().Length+1;
                        }                     
                        return true;
                    }
                }

                if (currencyTextBox.MaxLength != 0)
                {
                    if (unmaskedText.Length > currencyTextBox.MaxLength && currencyTextBox.CurrencyDecimalDigits < currencyTextBox.MaxLength)
                    {
                        int len = currencyTextBox.CurrencyDecimalDigits;
                        if (len < 0)
                        {
                            preValue = decimal.Parse(unmaskedText.Remove((currencyTextBox.MaxLength) - 3));
                            caretPosition++;
                            currencyTextBox.CaretIndex = caretPosition;
                        }
                        else
                            preValue = decimal.Parse(unmaskedText.Remove((currencyTextBox.MaxLength) - 1 - len));
                        currencyTextBox.SetValue(false, preValue);
                        currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                        if (caretPosition == currencyTextBox.CaretIndex)
                            caretPosition++;
                        currencyTextBox.CaretIndex = caretPosition;
                        return true;
                    }

                }              
                currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                maskedText = currencyTextBox.MaskedText;
                currencyTextBox.SetValue(false, preValue);

                int j = 0;
                for (i = 0; i < unmaskedText.Length; i++)
                {
                    if (i == caretPosition)
                    {
                        break;
                    }
                    if (j == maskedText.Length)
                        break;
                    if (char.IsDigit(maskedText[j]))
                        j++;
                    else
                    {
                        for (int k = j; k < maskedText.Length; k++)
                        {
                            if (char.IsDigit(maskedText[k]))
                                break;
                            j++;
                        }
                        i--;
                    }
                }
                currencyTextBox.SelectionStart = j;
                currencyTextBox.SelectionLength = 0;
            }
            return true;
        }

        public decimal? ValueFromText(CurrencyTextBox currencyTextBox, string maskedText)
        {
            decimal preValue;
            if (decimal.TryParse(maskedText, NumberStyles.Number, currencyTextBox.GetCulture().NumberFormat, out preValue))
                return preValue;
            else
            {
                if (currencyTextBox.UseNullOption)
                    return null;
                else
                {
                    decimal temp = 0;
                    if (temp > currencyTextBox.MaxValue && currencyTextBox.MinValidation == MinValidation.OnKeyPress)
                    {
                        temp = currencyTextBox.MaxValue;
                    }
                    else if (temp < currencyTextBox.MinValue && currencyTextBox.MaxValidation == MaxValidation.OnKeyPress)
                    {
                        temp = currencyTextBox.MinValue;
                    }
                    return temp;
                }
            }
        }

        public bool HandleKeyDown(CurrencyTextBox currencyTextBox, KeyEventArgs eventArgs)
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
                    return HandleBackSpaceKey(currencyTextBox);
                case Key.Tab:
                    break;
#if WPF
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    if (currencyTextBox.EnterToMoveNext)
                    {
                        FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                        TraversalRequest request = new TraversalRequest(focusDirection);
                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                        {
                            elementWithFocus.MoveFocus(request);
                        }
                    }
                    return true;
                    
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
                    break;
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
                    return HandleUpKey(currencyTextBox);
                case Key.Right:
                    break;
                case Key.Down:
                    return HandleDownKey(currencyTextBox);
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
                    return HandleDeleteKey(currencyTextBox);
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

        public bool HandleBackSpaceKey(CurrencyTextBox currencyTextBox)
        {
            if (currencyTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = currencyTextBox.GetCulture().NumberFormat;

            string maskedText = currencyTextBox.MaskedText;
            int currencyflag = 0;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int separatorStart = maskedText.IndexOf(numberFormat.CurrencyDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.CurrencyDecimalSeparator.Length;

            int caretPosition = currencyTextBox.SelectionStart;
            string unmaskedText = "";

            if (currencyTextBox.SelectionLength == 1)
            {
                if (!char.IsDigit(maskedText[currencyTextBox.SelectionStart]))
                {
                    currencyTextBox.SelectionLength = 0;
                    return true;
                }
            }
            else if (currencyTextBox.SelectionLength == 0 && currencyTextBox.SelectionStart != 0)
            {
                if (!char.IsDigit(maskedText[currencyTextBox.SelectionStart - 1]))
                {
                    if (maskedText[currencyTextBox.SelectionStart -1] == '(' || maskedText[currencyTextBox.SelectionStart -1] == ')')
                    {
                        unmaskedText = currencyTextBox.MaskedText;
                        unmaskedText = maskedText.Remove(selectionStart, 1);
                        int len = unmaskedText.Length;
                        unmaskedText = unmaskedText.Remove(len - 1, 1);
                        currencyTextBox.MaskedText = unmaskedText;
                        unmaskedText = unmaskedText.Remove(selectionStart, 1);
                        currencyTextBox.Value = decimal.Parse(unmaskedText);
                        //currencyTextBox.SelectionStart--;
                        currencyTextBox.SelectionLength = 0;
                        return true;
                    }
                    if (maskedText[currencyTextBox.SelectionStart - 1].ToString() == currencyTextBox.CurrencyGroupSeparator || maskedText[currencyTextBox.SelectionStart - 1].ToString() == numberFormat.CurrencySymbol)
                    {
                        unmaskedText = "";
                        currencyTextBox.SelectionStart--;
                        currencyTextBox.SelectionLength = 0;
                        return true;
                    }
                }
                else
                {
                    if (numberFormat.CurrencySymbol != null)
                    {
                        if (currencyTextBox.SelectionStart == 2 && maskedText[currencyTextBox.SelectionStart - 1].ToString()=="0")
                        {
                            unmaskedText = "";
                            currencyTextBox.SelectionStart--;
                            currencyTextBox.SelectionLength = 0;
                            return true;
                        }
                    }

                }
            }

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == currencyTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (currencyTextBox.SelectionStart + currencyTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.ToString();
                }

                if (i < maskedText.Length)
                {
                    if (char.IsDigit(maskedText[i]))
                        unmaskedText += maskedText[i];
                }
            }
            selectionLength = selectionEnd - selectionStart;

            //if (separatorStart < 0)
            //{
            //    separatorStart = unmaskedText.Length;
            //    separatorEnd = unmaskedText.Length;
            //}

            if (unmaskedText.Length == selectionLength)
            {
                currencyTextBox.SetValue(true, null);
                return true;
            }

            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd)
            {
                for (int decpos = separatorEnd; decpos < selectionEnd; decpos++)
                {
                    if (decpos != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        unmaskedText = unmaskedText.Insert(decpos, "0");
                    }
                }
                unmaskedText = unmaskedText.Remove(selectionStart, (separatorStart - selectionStart));
                caretPosition = selectionStart;
            }
            else if (selectionStart <= separatorStart && selectionEnd < separatorEnd)
            {
                if (selectionLength == 0)
                {
                    if (selectionStart != 0)
                    {
                        selectionLength = 1;
                        unmaskedText = unmaskedText.Remove(selectionStart - 1, selectionLength);
                        caretPosition = selectionStart - 1;
                        if (caretPosition == 0)
                            currencyflag = 1;
                    }
                    else
                        return true;
                }
                else if (separatorStart < 0)
                {
                    unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                    caretPosition = selectionStart - 1;
                }
                else
                {
                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                    caretPosition = selectionStart;
                }
            }
            else
            {
                if (selectionStart == selectionEnd)
                {
                    if (selectionStart != separatorEnd)
                    {
                        unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                        //unmaskedText = unmaskedText.Insert(selectionStart - 1, "0");
                        caretPosition = selectionStart - 1;
                    }
                    else
                    {
                        if(currencyTextBox.SelectionStart > 0)
                        currencyTextBox.SelectionStart = currencyTextBox.SelectionStart - 1;
                        return true;
                    }
                }
                else
                {
                    if (selectionLength > 0)
                        unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                    //for (int decpos = selectionStart; decpos < selectionEnd; decpos++)
                    //{
                    //    unmaskedText = unmaskedText.Remove(decpos, 1);
                    //    unmaskedText = unmaskedText.Insert(decpos, "0");
                    //}
                    caretPosition = selectionStart;
                }
            }

            decimal preValue;
            bool separatorflag = false;
            if (decimal.TryParse(unmaskedText, out preValue))
            {
                if (currencyTextBox.IsNegative)
                {
                    preValue = preValue * -1;
                }

                if ((preValue > currencyTextBox.MaxValue) && (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (currencyTextBox.MaxValueOnExceedMaxDigit)
                        preValue = currencyTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < currencyTextBox.MinValue) && (currencyTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (currencyTextBox.MinValueOnExceedMinDigit)
                        preValue = currencyTextBox.MinValue;
                    else return true;
                }
                currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                maskedText = currencyTextBox.MaskedText;
                currencyTextBox.SetValue(false, preValue);
                if (currencyflag == 0)
                {
                    int j = 0;
                    for (i = 0; i < unmaskedText.Length; i++)
                    {
                        if (i == caretPosition)
                            break;

                        if (j == maskedText.Length)
                            break;

                        if (char.IsDigit(maskedText[j]))
                            j++;

                        else
                        {
                            for (int k = j; k < maskedText.Length; k++)
                            {
                                if (j == maskedText.IndexOf(numberFormat.CurrencyDecimalSeparator))
                                    separatorflag = true;

                                if (char.IsDigit(maskedText[k]))
                                    break;
                                j++;
                            }
                            if (separatorflag == false)
                                i--;
                            separatorflag = false;
                        }
                    }
                    currencyTextBox.SelectionStart = j;
                    currencyTextBox.SelectionLength = 0;
                }
                else
                {
                    currencyTextBox.SelectionStart = 1;
                    currencyTextBox.SelectionLength = 0;
                }

            }
            else
            {
                currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                maskedText = currencyTextBox.MaskedText;
                currencyTextBox.SetValue(false, preValue);
            }
            return true;
        }

        public bool HandleDeleteKey(CurrencyTextBox currencyTextBox)
        {
            if (currencyTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = currencyTextBox.GetCulture().NumberFormat;

            string maskedText = currencyTextBox.MaskedText;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int currencyflag = 0;
            int separatorStart = maskedText.IndexOf(numberFormat.CurrencyDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.CurrencyDecimalSeparator.Length;

            int caretPosition = currencyTextBox.SelectionStart;
            string unmaskedText = "";


            if (currencyTextBox.SelectionLength <= 1 && currencyTextBox.SelectionStart != maskedText.Length)
            {
                if (!char.IsDigit(maskedText[currencyTextBox.SelectionStart]))
                {
                    if (maskedText[currencyTextBox.SelectionStart] == '(' || maskedText[currencyTextBox.SelectionStart]==')')
                    {
                            unmaskedText = currencyTextBox.MaskedText;
                            unmaskedText = maskedText.Remove(selectionStart, 1);
                            int len = unmaskedText.Length;
                            unmaskedText = unmaskedText.Remove(len - 1, 1);
                            currencyTextBox.MaskedText = unmaskedText;
                            unmaskedText = unmaskedText.Remove(selectionStart, 1);
                            currencyTextBox.Value = decimal.Parse(unmaskedText);
                            //currencyTextBox.SelectionStart--;
                            currencyTextBox.SelectionLength = 0;
                            return true;
                          }                    
                      
                  }
                if (maskedText[currencyTextBox.SelectionStart].ToString() == currencyTextBox.CurrencyGroupSeparator || maskedText[currencyTextBox.SelectionStart].ToString() == numberFormat.CurrencySymbol)
                {                    
                    unmaskedText = "";
                    currencyTextBox.SelectionStart++;
                    currencyTextBox.SelectionLength = 0;
                    return true;
                }
                if (numberFormat.CurrencySymbol != null)
                {
                    if (currencyTextBox.SelectionStart != 0)
                    {
                        if (maskedText[currencyTextBox.SelectionStart - 1].ToString() == numberFormat.CurrencySymbol && maskedText[currencyTextBox.SelectionStart].ToString() == "0")
                        {
                            unmaskedText = "";
                            currencyTextBox.SelectionStart++;
                            currencyTextBox.SelectionLength = 0;
                            return true;
                        }
                    }
                }
                
            }

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == currencyTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (currencyTextBox.SelectionStart + currencyTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.ToString();
                }

                if (i < maskedText.Length)
                {
                    if (char.IsDigit(maskedText[i]))
                        unmaskedText += maskedText[i];
                }
            }
            selectionLength = selectionEnd - selectionStart;

            //if (separatorStart < 0)
            //{
            //    separatorStart = unmaskedText.Length;
            //    separatorEnd = unmaskedText.Length;
            //}

            if (unmaskedText.Length == selectionLength)
            {
                currencyTextBox.SetValue(true, null);
                return true;
            }

            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd)
            {
                for (int decpos = separatorEnd; decpos < selectionEnd; decpos++)
                {
                    if (decpos != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        unmaskedText = unmaskedText.Insert(decpos, "0");
                    }
                }
                unmaskedText = unmaskedText.Remove(selectionStart, (separatorStart - selectionStart));
                caretPosition = selectionStart;
            }
            else if (selectionStart <= separatorStart && selectionEnd < separatorEnd)
            {
                if (selectionLength == 0)
                {
                    if (selectionStart != separatorStart)
                    {
                        selectionLength = 1;
                        unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                        caretPosition = selectionStart;
                        if (caretPosition == 0)
                            currencyflag = 1;
                    }
                    else
                    {
                        currencyTextBox.SelectionStart = currencyTextBox.SelectionStart + 1;
                        return true;
                    }
                }
                else
                {
                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                    caretPosition = selectionStart;
                }
            }
            else
            {
                if (selectionStart == selectionEnd)
                {
                    if (selectionStart != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(selectionStart, 1);
                        //unmaskedText = unmaskedText.Insert(selectionStart, "0");
                    }
                    else
                        return true;
                }
                else
                {
                    if(selectionLength > 0)
                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                    //for (int decpos = selectionStart; decpos < selectionEnd; decpos++)
                    //{
                    //    unmaskedText = unmaskedText.Remove(decpos, 1);
                    //    unmaskedText = unmaskedText.Insert(decpos, "0");
                    //}
                    caretPosition = selectionStart + selectionLength;
                }
            }

            decimal preValue;
            if (decimal.TryParse(unmaskedText, out preValue))
            {
                if (currencyTextBox.IsNegative)
                {
                    preValue = preValue * -1;
                }

                if ((preValue > currencyTextBox.MaxValue) && (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (currencyTextBox.MaxValueOnExceedMaxDigit)
                        preValue = currencyTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < currencyTextBox.MinValue) && (currencyTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (currencyTextBox.MinValueOnExceedMinDigit)
                        preValue = currencyTextBox.MinValue;
                    else return true;
                }

                currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                maskedText = currencyTextBox.MaskedText;
                currencyTextBox.SetValue(false, preValue);
                if (currencyflag == 0)
                {
                    int j = 0;
                    for (i = 0; i < unmaskedText.Length; i++)
                    {
                        if (i == caretPosition)
                        {
                            break;
                        }
                        if (j == maskedText.Length)
                            break;
                        if (char.IsDigit(maskedText[j]))
                            j++;
                        else
                        {
                            for (int k = j; k < maskedText.Length; k++)
                            {
                                if (char.IsDigit(maskedText[k]))
                                    break;
                                j++;
                            }
                            i--;
                        }
                    }
                    currencyTextBox.SelectionStart = j;
                    selectionStart = j;
                    currencyTextBox.SelectionLength = 0;
                }
                else
                {
                    currencyTextBox.SelectionStart = 1;
                    selectionStart = 1;
                    currencyTextBox.SelectionLength = 0;
                }
            }
            else
            {
                currencyTextBox.MaskedText = preValue.ToString("C", numberFormat);
                maskedText = currencyTextBox.MaskedText;
                currencyTextBox.SetValue(false, preValue);
            }
            return true;
        }

        public bool HandleDownKey(CurrencyTextBox currencyTextBox)
        {
            if (currencyTextBox.IsReadOnly || !currencyTextBox.IsScrollingOnCircle)
                return true;

            if (currencyTextBox.mValue != null)
            {

                if (((currencyTextBox.mValue - 1) < currencyTextBox.MinValue) && (currencyTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    //if (integerTextBox.MinValueOnExceedMinDigit)
                    //    integerTextBox.mValue = integerTextBox.MinValue;
                    //else 
                    return true;
                }
                else
                {
                    currencyTextBox.SetValue(true, currencyTextBox.mValue - (decimal)currencyTextBox.ScrollInterval);
                }
            }
            return true;
        }

        public bool HandleUpKey(CurrencyTextBox currencyTextBox)
        {
            if (currencyTextBox.IsReadOnly || !currencyTextBox.IsScrollingOnCircle)
                return true;

            if (currencyTextBox.mValue != null)
            {
                if (((currencyTextBox.mValue + 1) > currencyTextBox.MaxValue) && (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    //if (integerTextBox.MaxValueOnExceedMaxDigit)
                    //    integerTextBox.mValue = integerTextBox.MaxValue;
                    //else 
                    return true;
                }
                else
                {
                    currencyTextBox.SetValue(true, currencyTextBox.mValue + (decimal)currencyTextBox.ScrollInterval);
                }
            }
            return true;
        }
    }
}
