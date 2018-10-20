using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using Core.Utility;
using Core.Factories;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private GameMap map;
        private bool finished = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (finished)
            {
                for (int x = 0; x < map.getWidth(); x++)
                {
                    for (int y = 0; y < map.getHeight(); x++)
                    {
                        switch (map[x, y])
                        {
                            case BlockType.Exit:
                            case BlockType.ExitSwitch:
                            case BlockType.Switch:
                            case BlockType.Wall:
                                e.Graphics.FillRectangle(new SolidBrush(Color.Brown), new System.Drawing.Rectangle(x * 2, y * 2, 2, 2));
                                break;
                            case BlockType.Floor:
                            case BlockType.Light:
                                e.Graphics.FillRectangle(new SolidBrush(Color.Green), new System.Drawing.Rectangle(x * 2, y * 2, 2, 2));
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (!finished && !backgroundWorker1.IsBusy)
                map = MapFactory.getNewGameMap((panel1.Width - 20) / 2, (panel1.Height - 20) / 2, 0);
            finished = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            map = MapFactory.getNewGameMap((panel1.Width - 20) / 2, (panel1.Height - 20) / 2, 0);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            StartButton.Visible = false;
            finished = true;
        }

    }
}


