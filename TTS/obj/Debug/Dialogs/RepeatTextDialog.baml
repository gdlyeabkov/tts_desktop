   M S B A M L   `   `   `  ����>  :TTS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TTS.DialogsTTS.Dialogs  5   	   "    TTS.Dialogs.RepeatTextDialog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TTS.Dialogs     5   	   X TXceed.Wpf.Toolkit, Version=4.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f47toolkit)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$#  Повторить текст��5	   	   $	��450��5
   	   $	��380��5   	   .��5      �� .+�5   
   �� $��1q�5      
�������$��15q�5      _�5      �� 5      $��15q�5      .+�5      �� $
��15 5q�5      $����Части текста будут повторены несколько раз. Это
может быть полезно для тренировки памяти или
запоминания иностранных слов и выражений.��5      �� 5      $
��15 5q�5      $'��!Единицы измерения��5      �� 5      $��1q�5      
�������$��15q�5      _�5      �� 5      $��15q�5    !   .+�5!   "   	� $
��15 5q�5"   %   $��
Слова��5#   %   	� 5$   "    d� 	IsChecked$
 TrueY�5%   %   $
��15 5q�5&   %   $��Предложения��5'   %   	� 5(   "   $
��15 5q�5)   %   $��Абзацы��5*   %   5+      5-      �� 5.       �� Orientation$ 
Horizontal=�5/      .+�50      �� $
��15 5q�51      $!��Количество раз��52      %  Xceed.Wpf.Toolkit.IntegerUpDown  53      -   5          Name$ countRepeatsSpinner��54      $��Center=�55      $
��15 0q�56      �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 2X�57         Maximum$ 99X�58         Value$ 3X�59      5:      5;      5=      �� 5>   
   $	��0 5q�5?      $��Right=�5@      $ 
Horizontal=�5A      .+�5B      �� -   5        $
��15 0q�5C      $��65��5D      $��25��5E      $
��ОК��5F      5G      �� 5H      -   5        $
��15 0q�5I      $��65��5J      $��25��5K      $��Отмена��5L      5M      5N      5O      5P      