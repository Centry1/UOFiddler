/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System.Drawing;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;

namespace UoFiddler.Controls.Forms
{
    public partial class MapDetails : Form
    {
        public MapDetails(Map currmap, Point point)
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();
            TopMost = true;
            Tile currtile = currmap.Tiles.GetLandTile(point.X, point.Y);
            richTextBox.AppendText($"X: {point.X} Y: {point.Y}\n\n");
            richTextBox.AppendText("LandTile:\n");
            richTextBox.AppendText(
                $"{TileData.LandTable[currtile.ID].Name}: 0x{currtile.ID:X} Altitute: {currtile.Z}\n\n");
            HuedTile[] currStatics = currmap.Tiles.GetStaticTiles(point.X, point.Y);
            richTextBox.AppendText("Statics:\n");

            foreach (HuedTile currstatic in currStatics)
            {
                ushort id = currstatic.ID;
                richTextBox.AppendText(
                    $"{TileData.ItemTable[id].Name}: 0x{id:X} Hue: {currstatic.Hue} Altitute: {currstatic.Z}\n");
            }
        }
    }
}