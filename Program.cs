using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using CefSharp;
using CefSharp.WinForms;
using FISCA.Permission;
using FISCA.Authentication;

namespace _1campus.notice.v17
{
    public static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [FISCA.MainMethod]
        public static void Main()
        {


            //重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要
            //重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要
            //重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要重要

            //2017/8/16 穎驊註解，以下方法尋找cefSharp 路徑都失敗， 後來曜明 包裝了一個特別的版本 FISCA with Chrome embed 就可以了 ，
            //因此注意以後執行本程式碼 必須為FISCA with Chrome embed 的 ischool 主程式 ， 
            // 下載 位置:https://storage.googleapis.com/ischool-dra/1admin/FISCAWithChrome.7z

            #region 失敗的方法

            ////取得目前的專案dll檔的 絕對路徑(若在使用者端 就會抓絕對的使用者電腦路徑)
            //FileInfo projectDllURL = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""));
            ////取得html 相對路徑
            //string cefLibRelativeURL = Path.Combine(projectDllURL.DirectoryName, "CefLibrary"); 
            //string envirRefPath = Environment.GetEnvironmentVariable("PATH")  +";" +cefLibRelativeURL ;
            //Environment.SetEnvironmentVariable("PATH", envirRefPath, EnvironmentVariableTarget.Process);

            //var pluginsPath = Path.Combine(Environment.CurrentDirectory, "CefLibrary");
            //var path = Environment.GetEnvironmentVariable("PATH") + ";" + pluginsPath;
            //Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process); 
            #endregion
            {
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Image = Properties.Resources.speech_balloon_64;
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Medium;
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Enable = false;
                K12.Presentation.NLDPanels.Student.SelectedSourceChanged += delegate
                {
                    K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Enable = K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0 && Permissions.發送學生智慧簡訊權限;
                };
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Click += delegate
                {
                    //必須要使用greening帳號登入才能用
                    if (DSAServices.AccountType == AccountType.Greening)
                    {
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            new PostNotice_SP(PostNotice_SP.Type.Student).ShowDialog();
                        }
                        else
                        {
                            new PostNotice(PostNotice.Type.Student).ShowDialog();
                        }
                    }
                    else
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("必須是Greening帳號方可使用本功能");
                    }
                };
            }
            {
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Image = Properties.Resources.speech_balloon_64;
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Medium;
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Enable = false;
                K12.Presentation.NLDPanels.Teacher.SelectedSourceChanged += delegate
                {
                    K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Enable = K12.Presentation.NLDPanels.Teacher.SelectedSource.Count > 0 && Permissions.發送老師智慧簡訊權限;
                };
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["發送智慧簡訊"].Click += delegate
                {
                    //必須要使用greening帳號登入才能用
                    if (FISCA.Authentication.DSAServices.AccountType == FISCA.Authentication.AccountType.Greening)
                    {
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            new PostNotice_SP(PostNotice_SP.Type.Teacher).ShowDialog();
                        }
                        else
                        {
                            new PostNotice(PostNotice.Type.Teacher).ShowDialog();
                        }
                    }
                    else
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("必須是Greening帳號方可使用本功能");
                    }
                };

                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Image = Properties.Resources.book_zoom_64;
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Medium;
                //K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Enable = Permissions.學生公告歷程權限;
                K12.Presentation.NLDPanels.Student.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Click += delegate
                {
                    new StudentLogView().ShowDialog();


                };

                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Image = Properties.Resources.book_zoom_64;
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Medium;
                //K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Enable = Permissions.教師公告歷程權限;
                K12.Presentation.NLDPanels.Teacher.RibbonBarItems["智慧簡訊"]["智慧簡訊歷程"].Click += delegate
                {
                    new TeacherLogView().ShowDialog();
                };
            }

            //權限管理
            RoleAclSource.Instance["學生"]["智慧簡訊"].Add(new RibbonFeature(Permissions.發送學生智慧簡訊, "發送學生智慧簡訊"));
            RoleAclSource.Instance["教師"]["智慧簡訊"].Add(new RibbonFeature(Permissions.發送老師智慧簡訊, "發送老師智慧簡訊"));
            RoleAclSource.Instance["學生"]["智慧簡訊"].Add(new RibbonFeature(Permissions.學生公告歷程, "學生智慧簡訊歷程"));
            RoleAclSource.Instance["教師"]["智慧簡訊"].Add(new RibbonFeature(Permissions.教師公告歷程, "教師智慧簡訊歷程"));


        }

    }
}
