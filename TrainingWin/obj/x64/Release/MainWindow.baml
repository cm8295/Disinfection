   M S B A M L   `   `   `  ����   TrainingWin    TrainingWin.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     b ^Telerik.Windows.Data, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7x tTelerik.Windows.Controls.DataVisualization, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ctelerik1http://schemas.telerik.com/2008/xaml/presentation    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    >d2http://schemas.microsoft.com/expression/blend/2008    -     � Title$  TrainingWindow�� � 
ResizeMode$ NoResize=�$
��1280��$
��1024�� � WindowStyle$
 None=� � WindowState$ 	Maximized=�.��� -   $��gd_Main�� � RowDefinitions �� -    �Name$
 row1��$	��150 ��� -   $
 row2�� � ColumnDefinitions �� -   $
 col1��$
��1062 ��� -   $
 col2��.+�� $��0��$��0��$��2������  �� ImageSource$ /Pic/top.jpg��.+��� $
��Left=�$��/Pic/logo.png��$	��100��$��	70,22,0,0q�$	��Top=�$	��100���� $��模拟消毒培训系统��
�����_�$��35(�$
��Left=�$��
186,42,0,0q�$	��Top=�$��61��$	��338��$
��Bold"�� $��0��$��1��.+��� -   $��lbName��$��Label��$��15(�$��Right=�$	��Top=�$	��102�����	#FF237217$��0,9,16,0q��� $��登  录 ID:��$��15(�$��Right=�$	��Top=�$��75�����	#FF237217$��	0,9,118,0q��� -   $��完  成��$��Right=�
�������$��Bottom=�$��91��$��40��$��	0,0,10,10q��� -	   $��双击退出��$��Right=�
�������$��Bottom=�$��91��$��40��$��	0,0,10,61q�� $��1��$��0��.+�� -
    �Name$ backg2��$
��Left=�$	��Top=�	 K� ZIndex$	 4��$
��1062��$	��850��$��0,0,0,0q�.+��� -   
 ��Name$
 c4������ -   s2�� �� Color$ White�� �� Opacity$ 0Q�� -   $ backg��$
��Left=�$	��Top=�$
��1062��$	��850��$��0,0,0,0q�� -   $	 c1g�� J� Opacity$	 0.7Q�$
��Left=�$	��Top=�$	��600��$	��850��$��	240,0,0,0q�.+��� -   $
 c1��$	��600��$	��570��$��
0,200,0,80q����� -   s1��$ White��$
 0.01Q�� -   $	 c2g��$	 0.7Q�$
��Left=�$	��Top=�$	��330��$	��850��$��	720,0,0,0q�.+��� -   $
 c2��$	��330��$	��570��$��
0,200,0,80q����� $ White��$
 0.01Q�� -   $	 c3g��$	 0.7Q�$
��Left=�$	��Top=�$	��330��$	��850��$��0,0,0,0q�.+��� -   $
 c3��$	��330��$	��570��$��
0,200,0,80q����� $ White��$
 0.01Q�� -   $
��gd11��$��1��$	 9��$��1������ # � ��.+��� -    ��Name$ 	txt_timer��$��25��$��1176,10,10,733q�$	��Top=��� -   $	��bd1��
�������$��1q�$
��Left=�$	��717��$	��Top=�$	��202��$��
10,153,0,0q�._��� -   $��sp��$
��Left=�$��
0,0,-2,-20q� �� Orientation$ Vertical=�$	��703��$	��202��$	��Top=�.+��� -   $ Errtb1��$��18(�$
��Left=�$
��Wrap=�$�� ��$	��Top=�$��25��$��84���� -   $ txt_err_explain1��$
��Left=�$	��Top=�$�� ��$
��Wrap=�$��25(�
���  ��$	��191��$	��160��$��1,0,0,0q��� -   $ Errtb2��$��18(�$
��Left=�$
��Wrap=�$�� ��$	��Top=�$��25��$��84���� -   $ txt_err_explain2��$
��Left=�$	��Top=�$�� ��$
��Wrap=�$��25(�
���  ��$	��155��$	��160��$��1,0,0,0q��� -   $ Errtb3��$��18(�$
��Left=�$
��Wrap=�$�� ��$	��Top=�$��25��$��84���� -   $ txt_err_explain3��$
��Left=�$	��Top=�$�� ��$
��Wrap=�$��25(�
���  ��$	��155��$	��160��$��1,0,0,0q��� -   $ 
Positionxy��$��Right=�$��0,-113,86,0q�$
��Wrap=�$	��x:y��$	��Top=�$��21��$	��100���� -     ��Name$	 tt1��$�� ��$��Right=�$��1149,555,0,0q�$	��Top=��� $
��Left=�$��/Pic/111_04.png��$��30��$��	10,15,0,0q�$	��Top=�$��30���� $��下按获取压力��
������$��25(�$
��Left=�$��50,7,0,0q�$	��Top=�$	��158���� -!   $	 tb1��$��50(�
���2͚�$��
32,74,68,0q�$
��Wrap=�$	��Top=���  � Language$ zh-cn�$�0��