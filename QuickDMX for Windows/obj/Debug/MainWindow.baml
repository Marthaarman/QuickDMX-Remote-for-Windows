   M S B A M L   `   `   `  ����O  KQuickDMX for Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:QuickDMX_for_WindowsQuickDMX_for_Windows  5   	   %    QuickDMX_for_Windows.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:QuickDMX_for_Windows     5   	     � Title$!  QuickDMX remote for Windows��5   	   .��5	      �� -   5         ��Name$
 tabs��5	      $��2,15,2,2q�6   .@�5
   
   �� -   5         ��Name$ tabConnection��5
      $��	Connectie��6)   
���   ���6<   Z� ��6S   Z� .��5      �  � RowDefinitions 5      �� 5      $	��76* �6$   �� 5      $	��77* �6$   �� 5      $
��76.8 �6$   �� 5      $	��76* �6$   �� 5      $	��77* �6$   �� 5      $	��76* �6$    � ColumnDefinitions 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5      �� -   5         ��Name$ ipBox��5      $��2��6)   $
��Left=�6=   $��38��6X   $��	125,0,0,0q�6d   $��1��6w    �� TextWrapping$
 Wrap=�6�   $��	localhost��6�   $	��Top=�6�   $	��126��6�   �� 5      -   5        $ portBox��5      $��2��6+   $
��Left=�6?   $��38��6Z   $��125,0.2,0,0q�6f   $��2��6{   $
 Wrap=�6�   $
��7348��6�   $	��Top=�6�   $	��126��6�   �� 5      -   5         ��Name$ 
connectBtn��5      $��	Verbinden��6-   $
��Left=�6A   $��125,0.4,0,0q�6\   $��3��6q   $	��Top=�6~   $	��126��6�   $��2��6�   $��35��6�   6�   5      5       �� 5!   
   -   5        $ 
tabButtons��5!      $��Knoppen��6&   $��23��6d   $��-1.8,-2,-1.6,-0.2q�6p   $	��Top=�6�   {�.  ��67   Z� ��6M   Z� .��5"      � -   5         �Name$ dataGridViewTabButtons��5"       5#      �� 5$      �� 5%      �� 5&      �� 5'      �� 5(      �� 5)      �� 5*       5,      �� 5-      �� 5.      �� 5/      �� 50      �� 51      �� 52      54      55      56      57      