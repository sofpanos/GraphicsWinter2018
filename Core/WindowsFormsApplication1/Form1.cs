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
using Core.Constructions;

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

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (!finished)
            {
                map = MapFactory.getNewGameMap((this.Width - 10) / 3, (this.Height - 10) / 3, 0);
                this.StartButton.Visible = false;
                this.Refresh();
            }
                
            finished = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (finished)
            {
                int width = map.getWidth();
                int height = map.getHeight();
                SolidBrush wallBrush = new SolidBrush(Color.Brown);
                SolidBrush floorBrush = new SolidBrush(Color.Green);
                Graphics formGraphics = this.CreateGraphics();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        switch (map[x, y])
                        {
                            case BlockType.Exit:
                            case BlockType.ExitSwitch:
                            case BlockType.Switch:
                            case BlockType.Wall:
                                formGraphics.FillRectangle(wallBrush, new System.Drawing.Rectangle(x * 3, y * 3, 3, 3));
                                break;
                            case BlockType.Floor:
                            case BlockType.Light:
                                formGraphics.FillRectangle(floorBrush, new System.Drawing.Rectangle(x * 3, y * 3, 3, 3));
                                break;
                            default:
                                continue;
                        }
                    }
                }
                wallBrush.Dispose();
                floorBrush.Dispose();
                formGraphics.Dispose();
            }
        }

    }
}


