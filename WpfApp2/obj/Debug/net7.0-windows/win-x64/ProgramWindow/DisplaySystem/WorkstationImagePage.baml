   M S B A M L   `   `   `  ����A  =Retros, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Retros.ProgramWindow.DisplaySystem"Retros.ProgramWindow.DisplaySystem  5      =    7Retros.ProgramWindow.DisplaySystem.WorkstationImagePage   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:Retros.ProgramWindow.DisplaySystem     5      -   5          N�Name$
  Page��5
       N� Title$ WorkstationImagePage��5	      .��5      �� -   5         ��Name$ 
MainCanvas��5      ��5      �� *ActualWidth+ �� ElementName$
 Page����5      �� *ActualHeight+$
 Page��.+�5      � -   5         �Name$ MainGrid��5      $��Center=�5      ��5      �� *ActualWidth+$
 Page����5      �� *ActualHeight+$
 Page�� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      ��6   �� *Height+$ Image��.+�5   
   �� -   5        $ ImageHandleActivation��5      $��0��5      
������ $��20��5      ��5       �� *ActualWidth+$ MainGrid��.+�5"      � $��20��5#      ��5"      �� *ActualWidth+$ ImageHandleActivation��.+�5%      �� -   5         ��Name$ ImageHandle��5%      $��5q�5&      $y�	Collapsed=�5(      5)      5*      �� 5-   
   -   5         ��Name$ Image��5-      $��Center=�5.      $��Center=�5/      $��50q�50      $��1��51      $��Uniform=�52      ��53      �� *ActualHeight+$
 Page��54      55      56      