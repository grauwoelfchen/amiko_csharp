/*
Copyright (c) ywesee GmbH

This file is part of AmiKo for Windows.

AmiKo for Windows is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AmiKoWindows
{
    namespace ControlExtensions
    {
        /// <summary>
        /// The extension methods for user feedback on custom user control.
        /// </summary>
        public static class FeedbackExtension
        {
            public static void FeedbackField<T>(this UserControl _control, T element, bool hasError)
            {
                // TODO:
                // Refactor
                // TexBox and Border are FrameworkElement, but TextBox is Control, Border is not Control...
                if (element == null || !(element is T))
                    return;

                var converter = new BrushConverter();
                if (element is TextBox)
                {
                    var box = element as TextBox;
                    if (hasError)
                    {
                        box.BorderBrush = converter.ConvertFrom(Colors.ErrorBrushColor) as Brush;
                        box.Background = converter.ConvertFrom(Colors.ErrorFieldColor) as Brush;
                    }
                    else
                    {
                        box.BorderBrush = Brushes.DarkGray;
                        box.Background = Brushes.White;
                    }
                }
                else if (element is Image)
                {
                    // NOTE: Assumes Image's parent element is Border
                    var img = element as Image;
                    var border = img.Parent as Border;
                    if (border == null)
                        return;

                    if (hasError)
                    {
                        border.BorderBrush = converter.ConvertFrom(Colors.ErrorBrushColor) as Brush;
                        border.Background = converter.ConvertFrom(Colors.ErrorFieldColor) as Brush;
                    }
                    else
                    {
                        border.BorderBrush = Brushes.LightGray;
                        border.Background = converter.ConvertFrom(Colors.PaleGray) as Brush;
                    }
                }
            }

            public static void FeedbackMessage(this UserControl _control, TextBlock block, bool needsDisplay)
            {
                if (block == null)
                    return;

                if (!needsDisplay)
                    block.Visibility = Visibility.Hidden;
                else
                    block.Visibility = Visibility.Visible;
            }
        }
    }
}