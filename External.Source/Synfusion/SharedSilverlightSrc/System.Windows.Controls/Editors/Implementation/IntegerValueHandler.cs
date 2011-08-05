﻿using System;
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
    internal class IntegerValueHandler
    {
        public static IntegerValueHandler integerValueHandler = new IntegerValueHandler();

        public bool MatchWithMask(IntegerTextBox integerTextBox, string text)
        {

            if (integerTextBox.IsReadOnly)
                return true;

            bool minusKeyValidationflag = false;           
                if (integerTextBox.mValue == null || integerTextBox.mValue == 0)
                {
                    if (text == "-")
                    {
                        integerTextBox.minusPressed = true;
                        if (integerTextBox.count % 2 == 0)
                            {
                                integerTextBox.Foreground = integerTextBox.PositiveForeground;
                            }
                            else
                            integerTextBox.Foreground = integerTextBox.NegativeForeground;
                            
                            integerTextBox.count++;
                            return true; 
                    } 
                    else
                    {
                        if (integerTextBox.minusPressed == true)
                        {
                            integerTextBox.minusPressed = false;
                            if(integerTextBox.count % 2 == 0)
                            minusKeyValidationflag = true;
                        }
                    }
                }

                NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
                if (text == "-" || text == "+")
                {
                    if (integerTextBox.SelectionLength == integerTextBox.Text.Length)
                    {
                        integerTextBox.count = 1;
                        HandleDeleteKey(integerTextBox);
                    }
                    else if (integerTextBox.SelectionLength > 0 && integerTextBox.SelectionLength != integerTextBox.Text.Length)
                    {

                    }
                    else
                    {
                        Int64 tempVal;
                        if (integerTextBox.mValue != null)
                        {
                            if (text == "+")
                            {
                                if (integerTextBox.Value < 1)
                                    tempVal = (Int64)integerTextBox.mValue * -1;
                                else
                                    tempVal = (Int64)integerTextBox.mValue * 1;
                            }
                            else
                                tempVal = (Int64)integerTextBox.mValue * -1;
                            if ((tempVal > integerTextBox.MaxValue) && (integerTextBox.MaxValidation == MaxValidation.OnKeyPress))
                            {
                                if (integerTextBox.MaxValueOnExceedMaxDigit)
                                    tempVal = integerTextBox.MaxValue;
                                else return true;
                            }

                            if ((tempVal < integerTextBox.MinValue) && (integerTextBox.MinValidation == MinValidation.OnKeyPress))
                            {
                                if (integerTextBox.MinValueOnExceedMinDigit)
                                    tempVal = integerTextBox.MinValue;
                                else return true;
                            }
                            if (integerTextBox.MaskedText.Length == integerTextBox.MaxLength)
                            {
                                integerTextBox.MaskedText = integerTextBox.MaskedText;
                            }
                            integerTextBox.MaskedText = tempVal.ToString("N", numberFormat);
                            integerTextBox.SetValue(false, tempVal);
                        }
                    }
                    return true;
                }

                int selectionStart = 0;
                int selectionEnd = 0;
                int selectionLength = 0;
                int caretPosition = 0;
                string unmaskedText = integerTextBox.Text;
                string maskedText = integerTextBox.MaskedText;
                if (unmaskedText != "")
                {
                    if (unmaskedText.Length == 1)
                    {
                        if (unmaskedText == "0")
                        {
                            unmaskedText = text;
                            goto here;
                        }
                    }
                    else
                    {
                        if (unmaskedText[0] == '0' && unmaskedText.Length == 1)
                        {
                            unmaskedText = text + unmaskedText;
                            goto here;
                        }
                        else
                        {
                            goto go;
                        }

                    }

                }

            go:
                unmaskedText = "";
                for (int i = 0; i <= maskedText.Length; i++)
                {
                    if (i == integerTextBox.SelectionStart)
                    {
                        selectionStart = unmaskedText.Length;
                        caretPosition = unmaskedText.Length;
                    }

                    if (i == (integerTextBox.SelectionStart + integerTextBox.SelectionLength))
                        selectionEnd = unmaskedText.Length;


                    if (i < maskedText.Length)
                    {
                        if (integerTextBox.Value == null || integerTextBox.Value == 0)
                        {
                            if (char.IsDigit(maskedText[i]) || maskedText[i] == '-')
                                unmaskedText += maskedText[i];
                        }
                        else
                        {
                            if (char.IsDigit(maskedText[i]))
                                unmaskedText += maskedText[i];
                        }
                    }

                }
                selectionLength = selectionEnd - selectionStart;
                if (unmaskedText.Length > 0 && unmaskedText[0] == '0' && selectionStart == 0 && selectionLength == 0)
                {
                    // unmaskedText = "";
                }
                else
                {
                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                }
                if (char.IsDigit(text[0]))
                {
                    if (selectionStart == 0 && text == "0")
                    {
                    }
                    else
                        unmaskedText = unmaskedText.Insert(selectionStart, text);
                }
            here:
                Int64 preValue;
                if (char.IsDigit(text[0]))
                {
                    if (Int64.TryParse(unmaskedText, NumberStyles.Number, numberFormat, out preValue))
                    {
                        if (integerTextBox.IsNegative || minusKeyValidationflag)
                            preValue = preValue * -1;

                        if (preValue > integerTextBox.MaxValue)
                        {
                            if (integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
                            {
                                if (integerTextBox.MaxValueOnExceedMaxDigit)
                                    preValue = integerTextBox.MaxValue;
                                else return true;
                            }
                        }

                        if (preValue < integerTextBox.MinValue)
                        {
                            if (integerTextBox.MinValidation == MinValidation.OnKeyPress)
                            {
                                if (integerTextBox.MinValueOnExceedMinDigit)
                                    preValue = integerTextBox.MinValue;
                                else
                                {
                                    integerTextBox.checktext = integerTextBox.checktext + text;
                                    if (int.Parse(integerTextBox.checktext) > integerTextBox.MinValue)
                                    {
                                        integerTextBox.Value = int.Parse(integerTextBox.checktext);
                                        integerTextBox.CaretIndex = integerTextBox.Value.ToString().Length;
                                    }
                                    return true;
                                }
                            }
                        }
                        if (integerTextBox.MaxLength != 0)
                        {
                            if (unmaskedText.Length > integerTextBox.MaxLength)
                            {
                                caretPosition = integerTextBox.CaretIndex;
                                preValue = long.Parse(unmaskedText.Remove(integerTextBox.MaxLength));
                                integerTextBox.SetValue(false, preValue);
                                integerTextBox.MaskedText = preValue.ToString("N", numberFormat);
                                caretPosition++;
                                integerTextBox.CaretIndex = caretPosition;
                                return true;
                            }
                        }                   
                }
                else
                    return true;

                integerTextBox.SetValue(false, preValue);
                integerTextBox.MaskedText = preValue.ToString("N", numberFormat);
                if (unmaskedText != "")
                {
                    if (unmaskedText[0] == '-' && integerTextBox.OldValue == 0)
                    {
                        integerTextBox.MaskedText = unmaskedText;
                        maskedText = integerTextBox.MaskedText;
                        integerTextBox.SelectionStart = 2;
                        return true;
                    }

                    if (unmaskedText[0] == '0')
                    {
                        if (integerTextBox.Value < 1)
                        {
                            char text1 = '-';
                            integerTextBox.MaskedText = text1 + unmaskedText;
                        }
                        else
                            integerTextBox.MaskedText = unmaskedText;
                    }
                    maskedText = integerTextBox.MaskedText;

                }
                int j = -1;
                int i1 = 0;

                int selectionstart_temp = 0;
                int selectionlength_temp = 0;

                bool textflag = false;
                int textpos = 0;
                for (i1 = 0; i1 < maskedText.Length; )
                {
                    if (char.IsDigit(maskedText[i1]))
                    {
                        j++;
                    }

                    if (j == selectionStart)
                    {
                        selectionstart_temp = i1;
                        textflag = true;
                    }

                    if (textflag == true && char.IsDigit(maskedText[i1]))
                    {
                        textpos++;
                    }

                    if (textflag == true && textpos == text.Length)
                    {
                        selectionlength_temp = i1 + 1;
                        break;
                    }

                    i1++;
                }
                integerTextBox.SelectionStart = selectionlength_temp;//selectionstart_temp + selectionlength_temp;
                integerTextBox.SelectionLength = 0;
            }
            return true;
                
        }

        public Int64? ValueFromText(IntegerTextBox integerTextBox,string maskedText)
        {
            Int64 preValue;
            if (Int64.TryParse(maskedText, NumberStyles.Number, integerTextBox.GetCulture().NumberFormat, out preValue))
                return preValue;
            else
            {
                if (integerTextBox.UseNullOption)
                    return null;
                else
                {
                    Int64 temp = 0;
                    if (temp > integerTextBox.MaxValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
                    {
                        temp = integerTextBox.MaxValue;
                    }
                    else if (temp < integerTextBox.MinValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
                    {
                        temp = integerTextBox.MinValue;
                    }
                    return temp;
                }
            }
        }

        public bool HandleKeyDown(IntegerTextBox integerTextBox, KeyEventArgs eventArgs)
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
                    integerTextBox.count = 1;
                    return HandleBackSpaceKey(integerTextBox);

                case Key.Tab:
                    break;
#if WPF
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    if (integerTextBox.EnterToMoveNext)
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
                    return HandleUpKey(integerTextBox);
                case Key.Right:
                    break;
                case Key.Down:
                    return HandleDownKey(integerTextBox);
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
                    integerTextBox.count = 1;
                    return HandleDeleteKey(integerTextBox);
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

        public bool HandleBackSpaceKey(IntegerTextBox integerTextBox)
        {
            if (integerTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int caretPosition = 0;
            string unmaskedText = "";
            string maskedText = integerTextBox.MaskedText;

            if (integerTextBox.SelectionLength == 1)
            {
                if (!char.IsDigit(maskedText[integerTextBox.SelectionStart]))
                {
                    if (maskedText[integerTextBox.SelectionStart] == '-')
                    {
                        integerTextBox.Value = (integerTextBox.Value * -1);
                    } 
                    integerTextBox.SelectionLength = 0;
                    return true;
                }
            }
            else if (integerTextBox.SelectionLength == 0 && integerTextBox.SelectionStart != 0)
            {
                if (!char.IsDigit(maskedText[integerTextBox.SelectionStart - 1]))
                {
                    if (maskedText[0] == '-')
                    {                       
                        integerTextBox.Value = (-1 * integerTextBox.mValue);
                        integerTextBox.SelectionLength = 0;
                        if (integerTextBox.Value == 0)
                            {
                                maskedText = maskedText.Remove(0, 1);
                                integerTextBox.MaskedText = maskedText;
                             }
                                 
                        return true;
                    }
                    else
                    {
                        integerTextBox.SelectionStart--;
                        integerTextBox.Value = (1 * integerTextBox.mValue);
                        integerTextBox.SelectionLength = 0;
                        return true;
                    }
                }
            }

            for (int i = 0; i <= maskedText.Length; i++)
            {
                if (i == integerTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = unmaskedText.Length;
                }

                if (i == (integerTextBox.SelectionStart + integerTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i < maskedText.Length)
                {
                    if (char.IsDigit(maskedText[i]) || maskedText[i] == '-')
                        unmaskedText += maskedText[i];
                }
            }
            selectionLength = selectionEnd - selectionStart;

            if (selectionLength != 0)
                unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
            else
            {
                if (selectionStart != 0)
                {
                    selectionLength = 1;
                    unmaskedText = unmaskedText.Remove(selectionStart - 1, selectionLength);
                }
                else
                    return true;
            }
            selectionStart = selectionStart - selectionLength;

            if (unmaskedText.Length == 0)
            {
                if (integerTextBox.UseNullOption == true)
                {
                    integerTextBox.MaskedText = "";
                    integerTextBox.SetValue(true, null);
                }
                else
                {
                    if (unmaskedText == "")
                    {
                        integerTextBox.MaskedText = "0";
                        integerTextBox.Value = 0;
                    }
                }
                return true;
            }
          
            Int64 preValue;
            if (Int64.TryParse(unmaskedText, NumberStyles.Number, numberFormat, out preValue))
            {
                if (integerTextBox.IsNegative)
                {
                    preValue = preValue * 1;
                }

                if ((preValue > integerTextBox.MaxValue) && (integerTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (integerTextBox.MaxValueOnExceedMaxDigit)
                        preValue = integerTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < integerTextBox.MinValue) && (integerTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (integerTextBox.MinValueOnExceedMinDigit)
                        preValue = integerTextBox.MinValue;
                    else return true;
                }
            }
            else
            {
                if (unmaskedText == "-")
                {
                    if (integerTextBox.UseNullOption == true)
                    {
                        integerTextBox.MaskedText = "";
                        integerTextBox.SetValue(true, null);
                    }
                    else
                    {
                        integerTextBox.MaskedText = "0";
                        integerTextBox.Value = 0;
                    }
                }
                return true;
            }
         
            integerTextBox.SetValue(false, preValue);
            integerTextBox.MaskedText = preValue.ToString("N", numberFormat);
            //integerTextBox.MaskedText = unmaskedText;           
            maskedText = integerTextBox.MaskedText;
            integerTextBox.CaretIndex = selectionStart + selectionLength;
            if (integerTextBox.MaskedText.Contains(integerTextBox.NumberGroupSeparator))
            {
                int count = 0;
                foreach (char i in integerTextBox.MaskedText)
                {
                    if (integerTextBox.NumberFormat != null)
                    {
                        if (i.ToString() == integerTextBox.NumberFormat.NumberGroupSeparator)
                            count++;
                    }
                    else
                    {
                        if (i.ToString() == integerTextBox.NumberGroupSeparator || i == ',')
                            count++;
                    }

                }
                integerTextBox.CaretIndex = integerTextBox.CaretIndex + count;
            }
      
            int j = -1;
            int i1 = 0;
            for (i1 = 0; i1 < maskedText.Length; )
            {
                if (char.IsDigit(maskedText[i1]))
                {
                    j++;
                }
                if (j == selectionStart)
                    break;
                i1++;
            }           
         
            if (unmaskedText[0] == '-')
            {   
                if(unmaskedText.Length == selectionStart)
                 integerTextBox.SelectionStart = i1;
                else
                integerTextBox.SelectionStart = i1 - 1;
            }
            //else
            //    integerTextBox.SelectionStart = i1;
            integerTextBox.SelectionLength = 0;
            return true;
        }

        public bool HandleDeleteKey(IntegerTextBox integerTextBox)
        {
            if (integerTextBox.IsReadOnly)
                return true;

            NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
            int selectionStart = 0;
            int selectionEnd = 0;
            int selectionLength = 0;
            int caretPosition = 0;
            string unmaskedText = "";
            string maskedText = integerTextBox.MaskedText;

            if (integerTextBox.SelectionLength <= 1 && integerTextBox.SelectionStart != maskedText.Length)
            {
                if (!char.IsDigit(maskedText[integerTextBox.SelectionStart]))
                {
                    if (maskedText[0] == '-')
                    {                    
                        integerTextBox.Value = (-1 * integerTextBox.mValue);
                        integerTextBox.SelectionLength = 0;
                           if (integerTextBox.Value == 0)
                            {
                                maskedText = maskedText.Remove(0, 1);
                                integerTextBox.MaskedText = maskedText;

                            }                                           
                        return true;
                    }
                    else
                    {
                        integerTextBox.SelectionStart++;
                        integerTextBox.Value = (1 * integerTextBox.mValue);
                        integerTextBox.SelectionLength = 0;
                        return true;
                    }
                }
            }

            for (int i = 0; i <= maskedText.Length; i++)
            {
                if (i == integerTextBox.SelectionStart)
                {
                    selectionStart = unmaskedText.Length;
                    caretPosition = unmaskedText.Length;
                }

                if (i == (integerTextBox.SelectionStart + integerTextBox.SelectionLength))
                    selectionEnd = unmaskedText.Length;

                if (i < maskedText.Length)
                {                   
                     if (char.IsDigit(maskedText[i]) || maskedText[i] == '-')
                         unmaskedText += maskedText[i];                  
                }
            }
            selectionLength = selectionEnd - selectionStart;

            if (selectionLength != 0)
            {
                unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
            }          
            else
            {
                if (selectionStart != unmaskedText.Length)
                {
                    selectionLength = 1;
                    unmaskedText = unmaskedText.Remove(selectionStart, selectionLength);
                }
                else
                    return true;
            }

            if (unmaskedText.Length == 0)
            {
                if (integerTextBox.UseNullOption == true)
                {
                    integerTextBox.MaskedText = "";
                    integerTextBox.SetValue(true, null);
                }
                else
                {
                    if (unmaskedText == "")
                        integerTextBox.MaskedText = "0";
                        integerTextBox.Value = 0;
                }
               return true;
            }

            Int64 preValue;
            if (Int64.TryParse(unmaskedText, NumberStyles.Number, numberFormat, out preValue))
            {
                if (integerTextBox.IsNegative)
                {
                    preValue = preValue * 1;
                }

                if ((preValue > integerTextBox.MaxValue) && (integerTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    if (integerTextBox.MaxValueOnExceedMaxDigit)
                        preValue = integerTextBox.MaxValue;
                    else return true;
                }

                if ((preValue < integerTextBox.MinValue) && (integerTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    if (integerTextBox.MinValueOnExceedMinDigit)
                        preValue = integerTextBox.MinValue;
                    else return true;
                }
              
            }
            else
            {
                if (unmaskedText == "-")
                {
                    if (integerTextBox.UseNullOption == true)
                        integerTextBox.SetValue(true, null);
                    else
                        integerTextBox.MaskedText = "0";
                }
                return true;
            }
          
            integerTextBox.SetValue(false, preValue);
            if (integerTextBox.Value == 0)
            {
                if (integerTextBox.UseNullOption == true)
                {
                    integerTextBox.Value = 0;
                    integerTextBox.MaskedText = "";
                }
                else
                    integerTextBox.MaskedText = unmaskedText;
            }
            integerTextBox.MaskedText = preValue.ToString("N", numberFormat);
            //integerTextBox.MaskedText = unmaskedText;             
            maskedText = integerTextBox.MaskedText;

            int j = -1;
            int i1 = 0;
            for (i1 = 0; i1 < maskedText.Length; )
            {
                if (char.IsDigit(maskedText[i1]))
                {
                    j++;
                }
                if (j == selectionStart)
                    break;
                i1++;
            }
            if (unmaskedText.Length == selectionStart)
                integerTextBox.SelectionStart = i1;
            else
            {
                if(unmaskedText[0] == '-')
                integerTextBox.SelectionStart = i1 - 1;
                else
                    integerTextBox.SelectionStart = i1;
            }
            integerTextBox.SelectionLength = 0;
            return true;
        }
        
        public bool HandleDownKey(IntegerTextBox integerTextBox)
        {
            if (integerTextBox.IsReadOnly && !integerTextBox.IsScrollingOnCircle)
                return true;
            if (integerTextBox.IsReadOnly)
                return false;
            if (!integerTextBox.IsScrollingOnCircle)
                return false;

            if (integerTextBox.mValue != null)
            {

                if (((integerTextBox.mValue - 1) < integerTextBox.MinValue))// && (integerTextBox.MinValidation == MinValidation.OnKeyPress))
                {
                    //if (integerTextBox.MinValueOnExceedMinDigit)
                    //    integerTextBox.mValue = integerTextBox.MinValue;
                    //else 
                    return true;
                }
                else
                {
                    integerTextBox.SetValue(true, integerTextBox.mValue - integerTextBox.ScrollInterval);
                }
            }
            return true;
        }

        public bool HandleUpKey(IntegerTextBox integerTextBox)
        {
            if (integerTextBox.IsReadOnly || !integerTextBox.IsScrollingOnCircle)
                return true;
            

            if (integerTextBox.mValue != null)
            {
                if (((integerTextBox.mValue + 1) > integerTextBox.MaxValue))// && (integerTextBox.MaxValidation == MaxValidation.OnKeyPress))
                {
                    //if (integerTextBox.MaxValueOnExceedMaxDigit)
                    //    integerTextBox.mValue = integerTextBox.MaxValue;
                    //else 
                    return true;
                }
                else
                {
                   integerTextBox.SetValue(true, integerTextBox.mValue + integerTextBox.ScrollInterval);
                }
            }
            return true;
        }
    }
}
