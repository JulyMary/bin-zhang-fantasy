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
    internal class DoubleValueHandler
    {
        public static DoubleValueHandler doubleValueHandler = new DoubleValueHandler();

        public bool MatchWithMask(DoubleTextBox doubleTextBox, string text)
        {
            if (doubleTextBox.IsReadOnly)
                return true;

            bool minusKeyValidationflag = false;
            doubleTextBox.negativeFlag = false;
            if (doubleTextBox.mValue == null || doubleTextBox.mValue == 0)
            {
                if (text == "-")
                {
                    doubleTextBox.minusPressed = true;
                    if (doubleTextBox.count % 2 == 0)
                    {
                        doubleTextBox.Foreground = doubleTextBox.PositiveForeground;
                        doubleTextBox.IsNegative = false;
                    }
                    else
                    {
                        doubleTextBox.Foreground = doubleTextBox.NegativeForeground;
                        doubleTextBox.IsNegative = true;
                    }

                    doubleTextBox.count++;      
                   
                    return true;
                }
                else
                {
                    if (doubleTextBox.minusPressed == true)
                    {
                        doubleTextBox.minusPressed = false;
                        if (doubleTextBox.count % 2 == 0)
                        minusKeyValidationflag = true;
                    }
                }
            }

            NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;

            if (text == "-" || text == "+")
            {
                if (doubleTextBox.mValue != null)
                {
                    double tempVal = (double)doubleTextBox.mValue * -1;

                    if ((tempVal > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (doubleTextBox.MaxValueOnExceedMaxDigit)
                            tempVal = doubleTextBox.MaxValue;
                        else return true;
                    }


                    if ((doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (tempVal > doubleTextBox.MinValue)
                        {
                            if (doubleTextBox.MinValueOnExceedMinDigit && (tempVal.ToString()).Length > (doubleTextBox.MinValue.ToString()).Length)
                                tempVal = doubleTextBox.MinValue;
                            else if ((tempVal.ToString()).Length <= (doubleTextBox.MinValue.ToString()).Length)
                            {
                                doubleTextBox.MaskedText = tempVal.ToString(); ;
                            }
                            else return true;
                        }
                        else
                            tempVal = doubleTextBox.MinValue;

                    }
                    doubleTextBox.MaskedText = tempVal.ToString("N", numberFormat);
                    doubleTextBox.SetValue(false, tempVal);
                }
                return true;
            }

            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            if (doubleTextBox.Value == null)
                doubleTextBox.MaskedText = "";
            string maskedText = doubleTextBox.MaskedText;
            int separatorStart = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.NumberDecimalSeparator.Length;
            
            if (text == numberFormat.NumberDecimalSeparator)
            {
                doubleTextBox.SelectionStart = separatorEnd;
                return true;
            }
            if (text == numberFormat.NumberGroupSeparator)
            {
                if ((doubleTextBox.SelectionStart < doubleTextBox.Text.Length))
                {
                    if (doubleTextBox.Text[doubleTextBox.SelectionStart].ToString() == numberFormat.NumberGroupSeparator.ToString())
                    {
                        doubleTextBox.SelectionStart += 1;
                        return true;
                    }
                }
                return true;
            }

            int caretPosition = doubleTextBox.SelectionStart;
            string unmaskedText = "";

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == doubleTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (doubleTextBox.SelectionStart + doubleTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                  //unmaskedText += ".";
                   unmaskedText += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
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
          
            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd && char.IsDigit(text[0]))
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
                        unmaskedText = unmaskedText.Insert(selectionStart, text[0].ToString());
                        if (doubleTextBox.Value > -1 && doubleTextBox.Value < 1)
                        {
                            caretPosition = selectionStart + 1;
                        }
                    }
                    else
                        return true;
                }
                else if (char.IsDigit(text[0]))
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
                else
                {
                    doubleTextBox.negativeFlag = true;
                    unmaskedText = doubleTextBox.MaskedText;
                }
            }
            #endregion

            double preValue;
            if (double.TryParse(unmaskedText, out preValue))
            {
                if (doubleTextBox.IsNegative || minusKeyValidationflag)
                {
                    if(!doubleTextBox.negativeFlag)
                    preValue = preValue * -1;
                }


                if ((preValue > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (doubleTextBox.MaxValueOnExceedMaxDigit)
                        preValue = doubleTextBox.MaxValue;
                    else return true;
                }

                if (preValue < doubleTextBox.MinValue && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    //if (doubleTextBox.MinValueOnExceedMinDigit)
                    //{
                    if (preValue <= doubleTextBox.MinValue && doubleTextBox.MinValue >= 0)
                    {
                        if (numberFormat != null)
                        {
                            if (doubleTextBox.UseNullOption)
                            {
                                unmaskedText = preValue.ToString("N", numberFormat);
                            }
                            if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) >= (doubleTextBox.MinValue.ToString()).Length)
                                preValue = doubleTextBox.MinValue;
                            else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                            {                                
                                doubleTextBox.checktext = doubleTextBox.checktext + text;
                                if (double.Parse(doubleTextBox.checktext) > doubleTextBox.MinValue)
                                {
                                    doubleTextBox.Value = double.Parse(doubleTextBox.checktext);
                                    doubleTextBox.CaretIndex = doubleTextBox.Value.ToString().Length;
                                }
                                return true;
                            }
                        }

                    }
                    else if (preValue > doubleTextBox.MinValue)
                    {
                        doubleTextBox.MaskedText = unmaskedText;
                    }
                    else if (preValue >= doubleTextBox.MinValue)
                    {
                        if (doubleTextBox.MinValueOnExceedMinDigit && unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) > (doubleTextBox.MinValue.ToString()).Length)
                            preValue = doubleTextBox.MinValue;
                        else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                        {
                            doubleTextBox.MaskedText = unmaskedText;
                        }
                        else return true;
                    }
                    else
                        preValue = doubleTextBox.MinValue;
                    //}
                    //else if (preValue < doubleTextBox.MinValue && !doubleTextBox.MinValueOnExceedMinDigit)
                    //    return true;  
                }
               
                if (doubleTextBox.MaxLength != 0)
                {
                    if (unmaskedText.Length > doubleTextBox.MaxLength && doubleTextBox.NumberDecimalDigits <= doubleTextBox.MaxLength)
                    {
                        int len = doubleTextBox.NumberDecimalDigits;
                        if (len < 0)
                        {
                            preValue = double.Parse(unmaskedText.Remove((doubleTextBox.MaxLength) - 3));
                            caretPosition++;
                            doubleTextBox.CaretIndex = caretPosition;
                        }
                        else
                            preValue = double.Parse(unmaskedText.Remove((doubleTextBox.MaxLength) - 1 - len));
                        doubleTextBox.SetValue(false, preValue);
                        doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                        if (caretPosition == doubleTextBox.CaretIndex)
                            caretPosition++;
                        doubleTextBox.CaretIndex = caretPosition;
                        return true;
                    }

                } 
               
                doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                maskedText = doubleTextBox.MaskedText;
                doubleTextBox.SetValue(false, preValue);

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
                doubleTextBox.SelectionStart = j;
                doubleTextBox.SelectionLength = 0;
            }
            return true;
            /*
            NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
            
            if (text == "-" || text == "+")
            {
                if (doubleTextBox.mValue != null)
                {
                    double tempVal = (double)doubleTextBox.mValue * -1;

                    if ((tempVal > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (doubleTextBox.MaxValueOnExceedMaxDigit)
                            tempVal = doubleTextBox.MaxValue;
                        else return true;
                    }

                    if ((tempVal < doubleTextBox.MinValue) && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                    {
                        if (doubleTextBox.MinValueOnExceedMinDigit)
                            tempVal = doubleTextBox.MinValue;
                        else return true;
                    }
                    doubleTextBox.MaskedText = tempVal.ToString("N", numberFormat);
                    doubleTextBox.SetValue(null, tempVal);
                }
                return true;
            }

            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            
            int separatorStart = doubleTextBox.MaskedText.IndexOf(numberFormat.NumberDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.NumberDecimalSeparator.Length;

            int caretPosition = doubleTextBox.SelectionStart;
            string unmaskedText = "";

            int i;
            for (i = 0; i <= doubleTextBox.MaskedText.Length; i++)
            {
                if (i == doubleTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (doubleTextBox.SelectionStart + doubleTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += ".";
                }

                if (i < doubleTextBox.MaskedText.Length)
                {
                    if (char.IsDigit(doubleTextBox.MaskedText[i]))
                        unmaskedText += doubleTextBox.MaskedText[i];
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
                        unmaskedText = unmaskedText.Insert(selectionStart, text[0].ToString());
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

            double preValue;
            if (double.TryParse(unmaskedText, out preValue))
            {
                if ((preValue > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (doubleTextBox.MaxValueOnExceedMaxDigit)
                        preValue = doubleTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < doubleTextBox.MinValue) && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (doubleTextBox.MinValueOnExceedMinDigit)
                        preValue = doubleTextBox.MinValue;
                    else return true;
                }
                
                doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                doubleTextBox.SetValue(null, preValue);

                int j = 0;
                for (i = 0; i < unmaskedText.Length; i++)
                {
                    if (i == caretPosition)
                    {
                        break;
                    }
                    if (j == doubleTextBox.MaskedText.Length)
                        break;
                    if (char.IsDigit(doubleTextBox.MaskedText[j]))
                        j++;
                    else
                    {
                        for (int k = j; k < doubleTextBox.MaskedText.Length; k++)
                        {
                            if (char.IsDigit(doubleTextBox.MaskedText[k]))
                                break;
                            j++;
                        }
                        i--;
                    }
                }
                doubleTextBox.SelectionStart = j;
                doubleTextBox.SelectionLength = 0;
            }
            return true;
             */
        }

        public double? ValueFromText(DoubleTextBox doubleTextBox,string maskedText)
        {
            double preValue;
            if (double.TryParse(maskedText, NumberStyles.Number, doubleTextBox.GetCulture().NumberFormat, out preValue))
                return preValue;
            else
            {
                if (doubleTextBox.UseNullOption)
                    return null;
                else
                {
                    double temp = 0;
                    if (temp > doubleTextBox.MaxValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
                    {
                        temp = doubleTextBox.MaxValue;
                    }
                    else if (temp < doubleTextBox.MinValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
                    {
                        temp = doubleTextBox.MinValue;
                    }
                    return temp;
                }
            }
        }

        public bool HandleKeyDown(DoubleTextBox doubleTextBox, KeyEventArgs eventArgs)
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
                    doubleTextBox.count = 1;
                    return HandleBackSpaceKey(doubleTextBox);
                case Key.Tab:
                    break;
#if WPF
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    if (doubleTextBox.EnterToMoveNext)
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
                    if (doubleTextBox.EnterToMoveNext)
                    {
                        //FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                        //TraversalRequest request = new TraversalRequest(focusDirection);
                        //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                        //if (elementWithFocus != null)
                        //{
                        //    elementWithFocus.MoveFocus(request);
                        //}
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
                    return HandleUpKey(doubleTextBox);
                case Key.Right:
                    break;
                case Key.Down:
                    return HandleDownKey(doubleTextBox);
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
                    doubleTextBox.count = 1;
                    return HandleDeleteKey(doubleTextBox);
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

        public bool HandleBackSpaceKey(DoubleTextBox doubleTextBox)
        {
            if (doubleTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;

            string maskedText = doubleTextBox.MaskedText;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int separatorStart = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.NumberDecimalSeparator.Length;
            int negflag = 0;
            int caretPosition = doubleTextBox.SelectionStart;
            string unmaskedText = "";

            if (doubleTextBox.SelectionLength == 1)
            {
                if (!char.IsDigit(maskedText[doubleTextBox.SelectionStart]))
                {
                    if (maskedText[doubleTextBox.SelectionStart] == '-')
                    {
                        doubleTextBox.Value = (doubleTextBox.Value * -1);                       
                    }                    
                    doubleTextBox.SelectionLength = 0;
                    return true;
                }
            }
            else if (doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart == 1 && doubleTextBox.SelectionStart != maskedText.Length - 2)
            {
                if (!char.IsDigit(maskedText[doubleTextBox.SelectionStart - 1]))
                {
                    if (maskedText[0] == '-')
                    {
                        doubleTextBox.Value = (doubleTextBox.Value * -1);
                        //doubleTextBox.SelectionStart--;
                        doubleTextBox.SelectionLength = 0;
                        return true;
                    }
                }               
            }
            if (doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart!=0)
            {
                if (maskedText[doubleTextBox.SelectionStart - 1] == ',')
                {
                    unmaskedText = "";
                    doubleTextBox.SelectionStart--;
                    return true;
                }
            }
           

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == doubleTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (doubleTextBox.SelectionStart + doubleTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
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


            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd && unmaskedText.Length > 0)
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
                               if (doubleTextBox.MinValue == Double.Parse(unmaskedText))
                               {
                                   caretPosition = selectionStart - 1;
                               }
                               else if(unmaskedText.Length > 0)
                               {
                                   unmaskedText = unmaskedText.Remove(selectionStart - 1, selectionLength);
                                   caretPosition = selectionStart - 1;                                    
                               }
                                                       
                        }
                        else
                            return true;
                    }
                    else if(unmaskedText.Length > 0)
                    {
                        if (maskedText[doubleTextBox.SelectionStart] == '-')
                        {
                            doubleTextBox.Value = doubleTextBox.Value * -1;
                        }                        
                        unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                        caretPosition = selectionStart;                       
                    }
                }
                else if (separatorStart < 0)
                {
                    if (selectionStart >= 0)
                    {
                        if (selectionStart == 0 && selectionLength>=1)
                        {
                            if (maskedText[doubleTextBox.SelectionStart] == '-')
                            {
                                doubleTextBox.Value = doubleTextBox.Value * -1;
                            }
                        }
                        if (selectionLength >= 1 && unmaskedText.Length > 0)
                        {
                            if (selectionLength == unmaskedText.Length && doubleTextBox.MinValue.ToString() != "" && doubleTextBox.MinValue > 0)
                            {
                                unmaskedText = doubleTextBox.MinValue.ToString();
                            }
                            else
                            {
                                unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                                caretPosition = selectionStart;
                                if (doubleTextBox.SelectionStart == 1)
                                    negflag = 1;
                            }
                        }
                        else if (selectionLength == 0 && selectionStart == unmaskedText.Length && unmaskedText.Length > 0)
                        {
                            if (Double.Parse(unmaskedText) == doubleTextBox.MinValue)
                            {
                                caretPosition = selectionStart - 1;
                            }                            
                            else
                            {
                                unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                                caretPosition = selectionStart;
                            }
                        }
                        else if (selectionLength == 0 && unmaskedText.Length > 0 && selectionStart != unmaskedText.Length && selectionStart != 0 && maskedText[doubleTextBox.SelectionStart-1]!=',')
                        {
                            if (doubleTextBox.SelectionStart==2)
                            {
                                unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                                negflag = 1;
                                
                            }
                            else
                            {
                                unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                                caretPosition = selectionStart - 1;
                            }
                        }
                        else if (selectionLength == 0 && selectionStart != unmaskedText.Length && selectionStart != 0 && maskedText[doubleTextBox.SelectionStart-1] == ',')
                        {
                            unmaskedText = "";
                            doubleTextBox.SelectionStart--;
                            return true;
                        }
                        else
                        {
                            if (selectionStart != 0 && unmaskedText.Length > 0)
                            {
                                unmaskedText = unmaskedText.Remove(selectionStart, 1);
                                caretPosition = selectionStart;
                            }
                        }                        
                       
                    }
                }
                else
                {
                    if (selectionStart == selectionEnd && unmaskedText.Length > 0)
                    {
                        if (selectionStart != separatorEnd)
                        {
                            unmaskedText = unmaskedText.Remove(selectionStart - 1, 1);
                            unmaskedText = unmaskedText.Insert(selectionStart - 1, "0");
                            caretPosition = selectionStart - 1;
                        }
                        else
                        {
                            doubleTextBox.SelectionStart = doubleTextBox.SelectionStart - 1;
                            return true;
                        }
                    }
                    else if(unmaskedText.Length > 0)
                    {
                        for (int decpos = selectionStart; decpos < selectionEnd; decpos++)
                        {
                            unmaskedText = unmaskedText.Remove(decpos, 1);
                            unmaskedText = unmaskedText.Insert(decpos, "0");
                        }
                        caretPosition = selectionStart;
                    }
                }

                double preValue;
                bool separatorflag = false;
                if (double.TryParse(unmaskedText, out preValue))
                {
                    if (preValue == 0)
                    {
                        if (doubleTextBox.UseNullOption)
                            doubleTextBox.SetValue(true, null);
                        else
                            doubleTextBox.SetValue(true, 0.0);
                        return true;
                    }
                    if (doubleTextBox.IsNegative)
                    {
                        preValue = preValue * -1;
                    }

                    if ((preValue > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                    {
                        if (doubleTextBox.MaxValueOnExceedMaxDigit)
                            preValue = doubleTextBox.MaxValue;
                        else return true;
                    }
                    if (preValue <= doubleTextBox.MinValue && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                    {
                        //if (doubleTextBox.MinValueOnExceedMinDigit)
                        //{
                        if (preValue < doubleTextBox.MinValue && doubleTextBox.MinValue >= 0)
                        {
                            if (numberFormat != null)
                            {
                                if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) >= (doubleTextBox.MinValue.ToString()).Length)
                                {
                                    if (doubleTextBox.MinValueOnExceedMinDigit)
                                        preValue = doubleTextBox.MinValue;
                                    else
                                        return true;
                                }
                                else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                                {
                                    
                                    if (doubleTextBox.MinValueOnExceedMinDigit)
                                        preValue = doubleTextBox.MinValue;
                                    else if (preValue >= doubleTextBox.MinValue)                                
                                        doubleTextBox.MaskedText = unmaskedText;
                                    else
                                        return true;
                                }
                            }
                            else return true;
                        }
                        else if (preValue > doubleTextBox.MinValue)
                        {
                            doubleTextBox.MaskedText = unmaskedText;
                        }
                        else if (preValue >= doubleTextBox.MinValue)
                        {
                            if (doubleTextBox.MinValueOnExceedMinDigit && unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) > (doubleTextBox.MinValue.ToString()).Length)
                                preValue = doubleTextBox.MinValue;
                            else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                            {
                                doubleTextBox.MaskedText = unmaskedText;
                            }
                            else return true;
                        }
                        else if (doubleTextBox.MinValueOnExceedMinDigit)
                            preValue = doubleTextBox.MinValue;
                        else
                            return true;
                        //}
                        //else if (preValue < doubleTextBox.MinValue && !doubleTextBox.MinValueOnExceedMinDigit)
                        //    return true;
                    }

                    doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                    maskedText = doubleTextBox.MaskedText;
                    doubleTextBox.SetValue(false, preValue);
                    if (negflag == 0)
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
                                    if (j == maskedText.IndexOf(numberFormat.NumberDecimalSeparator))
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

                        doubleTextBox.SelectionStart = j;
                    }
                    else
                        doubleTextBox.SelectionStart = 1;
                    doubleTextBox.SelectionLength = 0;
                    negflag = 0;
                }
                else
                {
                    if (preValue == 0)
                    {
                        if (doubleTextBox.UseNullOption)
                            doubleTextBox.SetValue(true, null);
                        else
                            doubleTextBox.SetValue(true, 0.0);
                        return true;
                    }
                    doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                    maskedText = doubleTextBox.MaskedText;
                    doubleTextBox.SetValue(false, preValue);
                }
                return true;
           
        }

        public bool HandleDeleteKey(DoubleTextBox doubleTextBox)
        {
            if (doubleTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;

            string maskedText = doubleTextBox.MaskedText;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int separatorStart = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
            int separatorEnd = separatorStart + numberFormat.NumberDecimalSeparator.Length;
            int negflag = 0;
            int caretPosition = doubleTextBox.SelectionStart;
            string unmaskedText = "";           
            if (doubleTextBox.SelectionLength <= 1 && doubleTextBox.SelectionStart != maskedText.Length)
            {
                if (!char.IsDigit(maskedText[doubleTextBox.SelectionStart]))
                {
                    if (maskedText[doubleTextBox.SelectionStart] == '-')
                    {
                        doubleTextBox.Value = (doubleTextBox.Value * -1);
                        //doubleTextBox.SelectionStart++;
                        doubleTextBox.SelectionLength = 0;
                        return true;
                    }                    
                }
                if (doubleTextBox.NumberFormat != null)
                {
                    if (doubleTextBox.SelectionStart == maskedText.Length - (doubleTextBox.NumberFormat.NumberDecimalDigits + 2))
                    {
                        if (maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
                        {
                            negflag = 1;
                        }
                    }
                }
                if (doubleTextBox.NumberFormat == null)
                {
                    if (maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
                    {
                        negflag = 1;
                    }
                }
                if (maskedText[doubleTextBox.SelectionStart] == ',')
                {
                    unmaskedText = "";
                    doubleTextBox.SelectionStart++;
                    return true;
                }
                
            }

            int i;
            for (i = 0; i <= maskedText.Length; i++)
            {
                if (i == doubleTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = selectionStart;
                }
                if (i == (doubleTextBox.SelectionStart + doubleTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i == separatorEnd)
                    separatorEnd = unmaskedText.Length;

                if (i == separatorStart)
                {
                    separatorStart = unmaskedText.Length;
                    unmaskedText += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
                }

                if (i < maskedText.Length)
                {
                    if (char.IsDigit(maskedText[i]))
                        unmaskedText += maskedText[i];
                }
            }
            selectionLength = selectionEnd - selectionStart;

            if (separatorStart < 0)
            {
                separatorStart = unmaskedText.Length;
                separatorEnd = unmaskedText.Length;
            }
            if (selectionStart <= separatorStart && selectionEnd >= separatorEnd && unmaskedText.Length > 0)
            {
                for (int decpos = separatorEnd; decpos < selectionEnd; decpos++)
                {
                    if (decpos != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        unmaskedText = unmaskedText.Insert(decpos, "0");
                    }
                }
                if (selectionLength == unmaskedText.Length && doubleTextBox.MinValue.ToString() != "" && doubleTextBox.MinValue > 0)
                {
                    unmaskedText = doubleTextBox.MinValue.ToString();
                }
                else
                {
                    unmaskedText = unmaskedText.Remove(selectionStart, (separatorStart - selectionStart));
                    caretPosition = selectionStart;
                }
            }
            else if (selectionStart <= separatorStart && selectionEnd < separatorEnd && unmaskedText.Length > 0)
            {
                if (selectionLength == 0)
                {                    
                    if (selectionStart != separatorStart)
                    {                        
                        selectionLength = 1;
                        if (doubleTextBox.MinValue == Double.Parse(unmaskedText))
                        {
                            caretPosition = selectionStart + 1;                           
                        }
                        //else if (unmaskedText.Length == doubleTextBox.MinValue.ToString().Length)
                        //{
                        //    unmaskedText = doubleTextBox.MinValue.ToString();
                        //    caretPosition = selectionStart + 1;
                        //}
                        else
                        {
                            unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                            caretPosition = selectionStart;
                        }                       
                        if (doubleTextBox.SelectionStart == 1)
                        {
                            negflag = 1;
                        }
                    }                 
                         
                    else
                    {
                        doubleTextBox.SelectionStart = doubleTextBox.SelectionStart + 1;
                        return true;
                    }
                }
                else
                {
                    if (selectionStart == 0)
                    {
                        if (maskedText[doubleTextBox.SelectionStart] == '-')
                        {
                            doubleTextBox.Value = doubleTextBox.Value * -1;
                        }
                    }

                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                    caretPosition = selectionStart;
                    if (doubleTextBox.SelectionStart == 1)
                    {
                        negflag = 1;
                    }
                }
            }
            else if(unmaskedText.Length > 0)
            {
                if (selectionStart == selectionEnd)
                {
                    if (selectionStart != unmaskedText.Length)
                    {
                        unmaskedText = unmaskedText.Remove(selectionStart, 1);
                        unmaskedText = unmaskedText.Insert(selectionStart, "0");
                    }
                    else
                        return true;
                }
                else
                {
                    for (int decpos = selectionStart; decpos < selectionEnd; decpos++)
                    {
                        unmaskedText = unmaskedText.Remove(decpos, 1);
                        unmaskedText = unmaskedText.Insert(decpos, "0");
                    }
                    caretPosition = selectionStart + selectionLength;
                }
            }

            double preValue;
            if (double.TryParse(unmaskedText, out preValue))
            {
                if (preValue == 0)
                {
                    if (doubleTextBox.UseNullOption)
                        doubleTextBox.SetValue(true, null);
                    else
                        doubleTextBox.SetValue(true, 0.0);
                    return true;
                }
                if (doubleTextBox.IsNegative)
                {
                    preValue = preValue * -1;
                }

                if ((preValue > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (doubleTextBox.MaxValueOnExceedMaxDigit)
                        preValue = doubleTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < doubleTextBox.MinValue) && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    //if (doubleTextBox.MinValueOnExceedMinDigit)
                    //{
                    if (preValue <= doubleTextBox.MinValue && doubleTextBox.MinValue >= 0)
                    {
                        if (numberFormat != null)
                            if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) >= (doubleTextBox.MinValue.ToString()).Length)
                            {
                                if (doubleTextBox.MinValueOnExceedMinDigit)
                                    preValue = doubleTextBox.MinValue;
                                else
                                    return true;
                            }
                            else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                            {                                
                                if (doubleTextBox.MinValueOnExceedMinDigit)
                                    preValue = doubleTextBox.MinValue;
                                else if (preValue >= doubleTextBox.MinValue)                                
                                    doubleTextBox.MaskedText = unmaskedText;
                                else
                                    return true;
                            }
                            else return true;
                    }
                    else if (preValue > doubleTextBox.MinValue)
                    {
                        doubleTextBox.MaskedText = unmaskedText;
                    }
                    else if (preValue >= doubleTextBox.MinValue)
                    {
                        if (doubleTextBox.MinValueOnExceedMinDigit && unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) > (doubleTextBox.MinValue.ToString()).Length)
                            preValue = doubleTextBox.MinValue;
                        else if (unmaskedText.Length - (numberFormat.NumberDecimalDigits + 1) <= (doubleTextBox.MinValue.ToString()).Length)
                        {
                            doubleTextBox.MaskedText = unmaskedText;
                        }
                        else return true;
                    }
                    else if (doubleTextBox.MinValueOnExceedMinDigit)
                        preValue = doubleTextBox.MinValue;
                    else
                        return true;
                    //}
                    //else if (preValue < doubleTextBox.MinValue && !doubleTextBox.MinValueOnExceedMinDigit)
                    //    return true;
                }
             
                doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                maskedText = doubleTextBox.MaskedText;
                doubleTextBox.SetValue(false, preValue);
                if (negflag == 0)
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
                    doubleTextBox.SelectionStart = j;
                    selectionStart = j;
                }
                else
                    doubleTextBox.SelectionStart = 1;
                doubleTextBox.SelectionLength = 0;
                negflag = 0;
            }
            else
            {
                if (preValue == 0)
                {
                    if (doubleTextBox.UseNullOption)
                        doubleTextBox.SetValue(true, null);
                    else
                        doubleTextBox.SetValue(true, 0.0);
                    return true;
                }
                doubleTextBox.MaskedText = preValue.ToString("N", numberFormat);
                maskedText = doubleTextBox.MaskedText;
                doubleTextBox.SetValue(false, preValue);
            }
            return true;
        }
        
        public bool HandleDownKey(DoubleTextBox doubleTextBox)
        {
            if (doubleTextBox.IsReadOnly || !doubleTextBox.IsScrollingOnCircle)
                return true;
           

            if (doubleTextBox.mValue != null)
            {

                if (((doubleTextBox.mValue - doubleTextBox.ScrollInterval) < doubleTextBox.MinValue) && (doubleTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    //if (integerTextBox.MinValueOnExceedMinDigit)
                    //    integerTextBox.mValue = integerTextBox.MinValue;
                    //else 
                    return true;
                }
                else
                {                    
                    doubleTextBox.SetValue(true, doubleTextBox.mValue - doubleTextBox.ScrollInterval);                                           
                }
            }
           
            return true;
        }

        public bool HandleUpKey(DoubleTextBox doubleTextBox)
        {
            if (doubleTextBox.IsReadOnly || !doubleTextBox.IsScrollingOnCircle)
                return true;


            if (doubleTextBox.mValue != null)
            {
                if (((doubleTextBox.mValue + doubleTextBox.ScrollInterval) > doubleTextBox.MaxValue) && (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    //if (integerTextBox.MaxValueOnExceedMaxDigit)
                    //    integerTextBox.mValue = integerTextBox.MaxValue;
                    //else 
                    return true;
                }
                else
                {
                    doubleTextBox.SetValue(true, doubleTextBox.mValue + doubleTextBox.ScrollInterval);
                }
            }
            return true;
        }        
    }
}
