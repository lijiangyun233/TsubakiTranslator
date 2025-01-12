﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Shapes;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// HookResultDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class HookResultDisplay : UserControl
    {
        private Dictionary<string, HookResultItem> HookItemDict;
        private TranslateWindow translateWindow;

        public HookResultDisplay(TranslateWindow translateWindow)
        {
            InitializeComponent();

            HookItemDict = new Dictionary<string, HookResultItem>();
            this.translateWindow = translateWindow;

            translateWindow.TextHookHandler.ProcessTextractor.OutputDataReceived += AlterItemInStackPanel;

        }


        public void AlterItemInStackPanel(object sendingProcess, DataReceivedEventArgs outLine)
        {
            foreach(string code in translateWindow.TextHookHandler.HookDict.Keys)
            {
                if (HookItemDict.ContainsKey(code))
                    HookItemDict[code].HookData.HookText = translateWindow.TextHookHandler.HookDict[code];
                else
                {
                    //Dispatcher是一个线程控制器，要控制线程里跑的东西，就要经过它。
                    //WPF里面，有个所谓UI线程，后台代码不能直接操作UI控件，需要控制，就要通过这个Dispatcher。
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        /// start 你的逻辑代码
                        HookResultItem item = new HookResultItem(code, translateWindow.TextHookHandler.HookDict[code], translateWindow);
                        HookItemDict.Add(code, item);
                        DisplayStackPanel.Children.Add(item);
                        ///  end
                    }));
                    
                }
                    
            }

        }
    }
}
