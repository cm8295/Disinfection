   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem   Disinfection_Fin(   "Disinfection_Fin.Pages.Login_admin   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     N JFirstFloor.ModernUI, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null0mui&http://firstfloorsoftware.com/ModernUI  3System&clr-namespace:System;assembly=mscorlib  l hTelerik.Windows.Controls.Input, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.Diagrams, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b	 ^Telerik.Windows.Data, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7x
 tTelerik.Windows.Controls.DataVisualization, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Chart, Version=2013.3.1016.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Mtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
     .���    ContentRoot#��[  .+��� -     ��Name$  svlogin��.���� $	��200��.+�� $	��180��$	��280��.+��� -    ��Name$ uidbox��$
��Left=�$��24�� �� TextWrapping$
 Wrap=�$	��Top=�$	��170��$��	84,23,0,0q����� *
LastName+ �� Mode$ TwoWay=� �� ValidatesOnDataErrors . �� -    ��Name$	 tb1��$
��Left=�$
��Wrap=�$��管理员编号:��$	��Top=�$��15(�$��86��$��0,24,0,0q��� -   $	 tb2��$
��Left=�$
��Wrap=�$��管理员密码:��$	��Top=�$��15(�$��86��$��0,60,0,0q�G� -    G�Name$ pwbox��$
��Left=�$	��Top=�$	��170��$��24��$��	84,59,0,0q��� -    ��Name$ loginbtm��$��登录��$
��Left=�$	��Top=�$��73��$��25��$��101,97,0,-10q��� -   $ endbtm��$��退出��$
��Left=�$	��Top=�$��73��$��25��$��179,97,0,-10q��� -   $  sv��$	��Top=�.���� .+�� $	��509��$	��743�����	#FFE5E5E5.+��� -	   $��
TabControl��$
��Left=�$	��509��$	��Top=�$	��743��.@��� $��学员信息��.��� ���	#FFE5E5E5$��0,0,0,-2q�.+�� -
    �Name$ dataNew��.+�&   System.Windows.Controls.DataGrid  -   $��StudentData��$
��Left=�$��0,36,0,0q�$	��Top=�$	��448��$	��462��	   GridLinesVisibility$		 All=����� �� -   $��导出��$��12(�$
��Left=�$��633,447,0,0q�$	��Top=�$��74��$��25���� -   $��Exitbt��$��退出登录��$
��Left=�$��542,447,0,0q�$	��Top=�$��86���� $
��Left=�$��	10,11,0,0q�$
��Wrap=�$��管理员编号：��$	��Top=��� -   $ 
admin_user��$
��Left=�$��	87,11,0,0q�$
��Wrap=�$	��Top=�$��91��� $��475,37,10,404q�
�������.+��� $
��Left=�$��3,11,0,0q�$
��Wrap=�$��快速检索：��$	��Top=��� -   $ 
txt_search��
 � ToolTip$
 学员编号��$
��Left=�$��23��$��65,8,0,0q�$
 Wrap=�$�� ��$	��Top=�$��87���� -   $	��btb��$
 查找��$��Search��$
��Left=�$��	152,8,0,0q�$	��Top=�$��90��$��23�����	#FF78C1F3�� $
��Left=�$��
569,11,0,0q�$
��Wrap=�$��当前时间：��$	��Top=�$��74���� -   $ timer0��$
��Left=�$��
643,11,0,0q�$
��Wrap=�$�� ��$	��Top=�$��94���� -   $ btn_AllShow��$��全部显示��$
��Left=�$��
626,80,0,0q�$	��Top=�$��92��$��25�����	#FF6CA9F3�� -    ��Name$ admintb��$��管理员信息��$y�Visible=�.��� ���	#FFE5E5E5.+�� -   $ 	admingrid�����	#FFE5E5E5.+�� -   $ dataNewAdmin��.+�  -   $��	AdminData��$
��Left=�$��0,36,0,0q�$	��Top=�$	��446��$	��522���� $
��Left=�$��	10,11,0,0q�$
��Wrap=�$��管理员编号：��$	��Top=��� -   $ admin_user_Copy��$
��Left=�$��	87,11,0,0q�$
��Wrap=�$	��Top=�$��91���� $
��Left=�$��
574,11,0,0q�$
��Wrap=�$��当前时间：��$	��Top=� J� RenderTransformOrigin$ 	1.2,0.459&��� -   $ timer1��$
��Left=�$��
639,11,0,0q�$
��Wrap=�$�� ��$	��Top=�$��93��� -   $ 	Add_Admin�����	#FFE5E5E5$��
0,10,0,-10q�$y�Hidden=�.+��� -   $ btn_register��$��注册��$
��Left=�$��152,216,0,0q�$	��Top=�$��75���� $
��Left=�$��	62,48,0,0q�$
��Wrap=�$��管理员账号：��$	��Top=��� $
��Left=�$��	62,99,0,0q�$
��Wrap=�$��管理员密码：��$	��Top=��� -   $ txt_num��$
��Left=�$��23��$��
205,40,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120���� $
��Left=�$��
50,153,0,0q�$
��Wrap=�$��再次输入密码：��$	��Top=�G� -   $ txt_pass��$
��Left=�$��
205,93,0,0q�$	��Top=�$	��120��G� -   $ txt_pass_Copy��$
��Left=�$��205,147,0,0q�$	��Top=�$	��120��� -   $ iderr��$
��Left=�$��23��$��
347,40,0,0q�$	��Top=�$��38��� -   $ pwerr��$
��Left=�$��23��$��
347,93,0,0q�$	��Top=�$��38��� -    $ repwerr��$
��Left=�$��23��$��347,145,0,0q�$	��Top=�$��38���� -!   $ return_admin��$��返回��$
��Left=�$��250,216,0,0q�$	��Top=�$��75���� -"   $ teststb��$��考试管理��$y�Visible=�.��� 
�������.+�� $��10,45,256,10q�$y�Visible=�.+�� -#   $ dataNewExam��.+�  -$   $��ExamData��$
��Left=�$	��Top=�$	��427��$	��471���� -%   $ 	txt_idnum��$
��Left=�$��23��$��
607,45,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120���� -&   $��删除��$
��Left=�$��
652,84,0,0q�$	��Top=�$��75���� -'   $ 	btn_train��$��点击关闭训练模式��$
��Left=�$��557,372,0,0q�$	��Top=�$	��150���� -(   $ btn_test��$��点击打开考核模式��$
��Left=�$��557,402,0,0q�$	��Top=�$	��150��