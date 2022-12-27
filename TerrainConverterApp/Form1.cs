using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PilotGaea.Serialize;
using PilotGaea.TMPEngine;
using PilotGaea.Geometry;

namespace TerrainConverterApp
{
    public partial class Form1 : Form
    {
        CTerrainMaker m_Maker = null;
        public Form1()
        {
            InitializeComponent();

            //加入功能列表
            List<string> featureNames = new List<string>();
            featureNames.Add("基本");
            comboBox_Features.Items.AddRange(featureNames.ToArray());
            comboBox_Features.SelectedIndex = 0;
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            EnableUI(false);

            //     生成Terrain圖層。
            System.Environment.CurrentDirectory = @"C:\Program Files\PilotGaea\TileMap";//為了順利存取安裝目錄下的相關DLL
            m_Maker = new CTerrainMaker();
            //設定必要參數
            //     圖層名稱
            string layerName = "terrain";
            //      DB路徑
            string layerDBFile = string.Format(@"{0}\..\output\terrain_maker.DB", Application.StartupPath);
            //     來源dem檔案集
            string[] demFileNames = new string[] { string.Format(@"{0}\..\data\terrain_maker\台灣高程3857.dem", Application.StartupPath) };
            //     來源dem檔案EPSG集，長度需>=dem檔案集。
            int[] demEPSGs = new int[] { 3857 };
            //     輸出邊界。
            GeoBoundary exportBoundary = null;
            //     是否為橢球。
            bool isEllipsoid = false;


            //監聽轉檔事件
            m_Maker.CreateLayerCompleted += M_Maker_CreateCompletedEvent;

            //設定進階參數
            switch (comboBox_Features.SelectedIndex)
            {
                case 0://"基本"
                    break;
            }

            //開始非同步轉檔
            bool ret = m_Maker.Create(layerName, layerDBFile, demFileNames, demEPSGs,
                exportBoundary,
                isEllipsoid);

            string message = string.Format("參數檢查{0}", (ret ? "通過" : "失敗"));
            listBox_Main.Items.Add(message);
        }


        private void M_Maker_CreateCompletedEvent(string layerName, bool isSuccess, string errorMessage)
        {
            string message = string.Format("轉檔{0}", (isSuccess ? "成功" : "失敗"));
            listBox_Main.Items.Add(message);
        }


        private void EnableUI(bool enable)
        {
            button_Start.Enabled = enable;
            comboBox_Features.Enabled = enable;
        }
    }
}