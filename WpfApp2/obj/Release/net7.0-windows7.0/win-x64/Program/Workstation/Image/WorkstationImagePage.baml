   M S B A M L   `   `   `  ���� A  =Retros, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Retros.Program.DisplaySystemRetros.Program.DisplaySystem  7    1Retros.Program.DisplaySystem.WorkstationImagePage   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     <local*clr-namespace:Retros.Program.DisplaySystem     -     N�Name$
  Page�� N� Title$ WorkstationImagePage��.���� -    ��Name$ 
MainCanvas������ *ActualWidth+ �� ElementName$
 Page������ *ActualHeight+$
 Page��.+�� -    �Name$ MainGrid��$��Center=����� *ActualWidth+$
 Page������ *ActualHeight+$
 Page�� � RowDefinitions �� $
��Auto ��� ���� *Height+$ Image��.+��� -   $ ImageHandleActivation��$��0��
������ $��20������ *ActualWidth+$ MainGrid��.+�� $��20������ *ActualWidth+$ ImageHandleActivation��.+��� -    ��Name$ ImageHandle��$��5q�$y�	Collapsed=��� -    ��Name$ Image��$��Center=�$��Center=�$��50q�$��1��$��Uniform=����� *ActualHeight+$
 Page��