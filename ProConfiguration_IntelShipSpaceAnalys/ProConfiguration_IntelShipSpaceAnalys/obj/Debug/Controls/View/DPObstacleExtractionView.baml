   M S B A M L   `   `   `  ����`  \ProConfiguration_IntelShipSpaceAnalys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:ProConfiguration_IntelShipSpaceAnalys%ProConfiguration_IntelShipSpaceAnalys  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      D    >ProConfiguration_IntelShipSpaceAnalys.DPObstacleExtractionView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Elocal3clr-namespace:ProConfiguration_IntelShipSpaceAnalys     5      ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ..\Theme\CustomStyle.xaml?�6%   %    5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��0.3* �6   6.   �� 5      $��0.55* �6   6/   �� 5      $��0.15* �6   6/   .+�5   
   �� $��0��6   $��0��6!   $��航线选取：��61   $��Center=�6>   $��Right=�6Z   6x   �� 5    
   $��0��6   $��1��6!   $
��0,10q�61   ��6?   �� *CourseOIDList+ �� SelectedItem 5!      �� *CourseListSelectedItem+ �� Mode$ TwoWay=� �� SelectedIndex 6P   �� *CourseListSelectedIndex+$ TwoWay=�.  (System.Windows.Interactivity.Interaction   Triggers 5"      /  )System.Windows.Interactivity.EventTrigger  5#      	   	EventName$	 SelectionChanged��6!   .  (System.Windows.Interactivity.TriggerBase
   Actions.
 5$      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6-   �� *SetCourseSelectedItemCommand+   CommandParameter 6^   �� *SelectedItem+ �� ElementName$ TemplateList��5%      5'      �� 5(   
   $��0��6   $��2��6   $��5q�6/   #��[" �5)       � ToolTip$ 刷新��6M   ��5(   :   �� *RefleshCourseCommand+.��5*      �� $
��Fill=�6   $I��C/ProConfiguration_IntelShipSpaceAnalys;component/Images/reflesh.png��6#   $��20��6p   $��20��6{   6�   5+      �� 5-   
   $��1��6   $��0��6!   $��碍航物要素：��61   $��Center=�6?   $��Right=�6[   6y   �� 5.   
   $��1��6   $��1��6    $
��0,10q�60   ��6>   �� *ObstaclePath+6_   �� 5/   
   $��1��6   $��2��6   $��5q�6/   #��[" �50      $ 打开��6M   ��5/   :   �� *OpenObstacleCommand+.��51      �� $
��Fill=�6   $J��D/ProConfiguration_IntelShipSpaceAnalys;component/Images/menucopy.png��6#   $��20��6q   $��20��6|   6�   52      �� 54   
   $��2��6   $��0��6!   $��提取距离：��61   $��Center=�6>   $��Right=�6Z   6x   �� 55   
   $��2��6   $��1��6    $
��0,10q�61   ��6?   �� *ExtraDistance+6`   �� 56   
   $��2��6   $��2��6!   $��海里��61   $��Center=�6;   $��Center=�6W   6v   �� 58   
   $��4��6   $��0��6   $��3��6.      DockpaneBottomBtnStyle#��[  6B   $��执行��6r   $��Right=�6   ��59      �� *ExcuteCommand+65   5:      5;      