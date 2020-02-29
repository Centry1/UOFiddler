﻿/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.UserControls;

namespace UoFiddler.Controls.Forms
{
    public partial class AnimationListNewEntries : Form
    {
        private readonly AnimationList _form;
        private int _currentSelect;
        private int _facing = 1;
        private bool _animate;
        private Timer _timer;
        private int _frameIndex;
        private Frame[] _animation;

        public AnimationListNewEntries(AnimationList form)
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();
            _form = form;
        }

        private bool Animate
        {
            get => _animate;
            set
            {
                if (_animate == value)
                {
                    return;
                }

                _animate = value;
                StopAnimation();
                if (_animate)
                {
                    SetAnimation();
                }
            }
        }

        private int CurrentSelectAction { get; set; }

        private int CurrentSelect
        {
            get => _currentSelect;
            set
            {
                StopAnimation();
                _currentSelect = value;
                if (_animate)
                {
                    SetAnimation();
                }

                pictureBox1.Invalidate();
            }
        }

        private void StopAnimation()
        {
            if (_timer != null)
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                }

                _timer.Dispose();
                _timer = null;
            }

            _animation = null;
            _frameIndex = 0;
        }

        private void SetAnimation()
        {
            int body = _currentSelect;
            Animations.Translate(ref body);
            int hue = 0;
            _animation = Animations.GetAnimation(_currentSelect, CurrentSelectAction, _facing, ref hue, false, false);
            if (_animation == null)
            {
                return;
            }

            _frameIndex = 0;
            _timer = new Timer
            {
                Interval = 1000 / _animation.Length
            };
            _timer.Tick += AnimTick;
            _timer.Start();
        }

        private void AnimTick(object sender, EventArgs e)
        {
            ++_frameIndex;

            if (_frameIndex == _animation.Length)
            {
                _frameIndex = 0;
            }

            pictureBox1.Invalidate();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            facingbar.Value = (_facing + 3) & 7;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView1.TreeViewNodeSorter = new AnimNewListGraphicSorter();

            MobTypes();

            TreeNode node;
            foreach (DictionaryEntry key in BodyTable.m_Entries) //body.def
            {
                BodyTableEntry entry = (BodyTableEntry)key.Value;
                if (AlreadyFound(entry.NewID))
                {
                    continue;
                }

                if (_form.IsAlreadyDefined(entry.NewID))
                {
                    continue;
                }

                node = new TreeNode(entry.NewID.ToString())
                {
                    Tag = entry.NewID,
                    ToolTipText = $"Found in body.def {Animations.GetFileName(entry.NewID)}"
                };
                node.Tag = new[] { entry.NewID, 0 };
                treeView1.Nodes.Add(node);
                SetActionType(node, entry.NewID, 0);
            }

            if (BodyConverter.Table1 != null)
            {
                foreach (int entry in BodyConverter.Table1)  // bodyconv.def
                {
                    if (entry == -1)
                    {
                        continue;
                    }

                    if (AlreadyFound(entry))
                    {
                        continue;
                    }

                    if (_form.IsAlreadyDefined(entry))
                    {
                        continue;
                    }

                    node = new TreeNode(entry.ToString())
                    {
                        ToolTipText = $"Found in bodyconv.def {Animations.GetFileName(entry)}",
                        Tag = entry
                    };
                    node.Tag = new[] { entry, 0 };
                    treeView1.Nodes.Add(node);
                    SetActionType(node, entry, 0);
                }
            }

            if (BodyConverter.Table2 != null)
            {
                foreach (int entry in BodyConverter.Table2)
                {
                    if (entry == -1)
                    {
                        continue;
                    }

                    if (AlreadyFound(entry))
                    {
                        continue;
                    }

                    if (_form.IsAlreadyDefined(entry))
                    {
                        continue;
                    }

                    node = new TreeNode(entry.ToString())
                    {
                        ToolTipText = $"Found in bodyconv.def {Animations.GetFileName(entry)}",
                        Tag = entry
                    };
                    node.Tag = new[] { entry, 0 };
                    treeView1.Nodes.Add(node);
                    SetActionType(node, entry, 0);
                }
            }

            if (BodyConverter.Table3 != null)
            {
                foreach (int entry in BodyConverter.Table3)
                {
                    if (entry == -1)
                    {
                        continue;
                    }

                    if (AlreadyFound(entry))
                    {
                        continue;
                    }

                    if (_form.IsAlreadyDefined(entry))
                    {
                        continue;
                    }

                    node = new TreeNode(entry.ToString())
                    {
                        ToolTipText = $"Found in bodyconv.def {Animations.GetFileName(entry)}",
                        Tag = entry
                    };
                    node.Tag = new[] { entry, 0 };
                    treeView1.Nodes.Add(node);
                    SetActionType(node, entry, 0);
                }
            }

            if (BodyConverter.Table4 != null)
            {
                foreach (int entry in BodyConverter.Table4)
                {
                    if (entry == -1)
                    {
                        continue;
                    }

                    if (AlreadyFound(entry))
                    {
                        continue;
                    }

                    if (_form.IsAlreadyDefined(entry))
                    {
                        continue;
                    }

                    node = new TreeNode(entry.ToString())
                    {
                        ToolTipText = $"Found in bodyconv.def {Animations.GetFileName(entry)}",
                        Tag = entry
                    };
                    node.Tag = new[] { entry, 0 };
                    treeView1.Nodes.Add(node);
                    SetActionType(node, entry, 0);
                }
            }

            treeView1.EndUpdate();
        }

        private bool AlreadyFound(int graphic)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (((int[])node.Tag)[0] == graphic)
                {
                    return true;
                }
            }

            return false;
        }

        private void MobTypes()
        {
            string filePath = Files.GetFilePath("mobtypes.txt");

            if (filePath == null)
            {
                return;
            }

            using (StreamReader def = new StreamReader(filePath))
            {
                string line;

                while ((line = def.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }

                    try
                    {
                        string[] split = line.Split('\t');
                        if (int.TryParse(split[0], out int graphic))
                        {
                            if (!AlreadyFound(graphic) && !_form.IsAlreadyDefined(graphic))
                            {
                                TreeNode node = new TreeNode(graphic.ToString())
                                {
                                    ToolTipText = $"Found in mobtype.txt {Animations.GetFileName(graphic)}"
                                };
                                int type = 0;
                                switch (split[1])
                                {
                                    case "MONSTER": type = 0; break;
                                    case "SEA_MONSTER": type = 1; break;
                                    case "ANIMAL": type = 2; break;
                                    case "HUMAN": type = 3; break;
                                    case "EQUIPMENT": type = 3; break;
                                }
                                node.Tag = new[] { graphic, type };
                                treeView1.Nodes.Add(node);
                                SetActionType(node, graphic, type);
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private void SetActionType(TreeNode parent, int graphic, int type)
        {
            parent.Nodes.Clear();
            if (type == 4) // Equipment == human
            {
                type = 3;
            }

            for (int i = 0; i < _form.GetAnimNames[type].GetLength(0); ++i)
            {
                if (!Animations.IsActionDefined(graphic, i, 0))
                {
                    continue;
                }

                TreeNode node = new TreeNode($"{i} {_form.GetAnimNames[type][i]}")
                {
                    Tag = i
                };
                parent.Nodes.Add(node);
            }
        }

        private void OnAfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                CurrentSelectAction = 0;
                CurrentSelect = ((int[])e.Node.Tag)[0];
                ComboBoxActionType.SelectedIndex = ((int[])e.Node.Tag)[1];
            }
            else
            {
                CurrentSelectAction = (int)e.Node.Tag;
                CurrentSelect = ((int[])e.Node.Parent.Tag)[0];
                ComboBoxActionType.SelectedIndex = ((int[])e.Node.Parent.Tag)[1];
            }
        }

        private void OnPaint_PictureBox(object sender, PaintEventArgs e)
        {
            if (CurrentSelect == 0)
            {
                return;
            }

            if (_animate)
            {
                if (_animation?[_frameIndex].Bitmap == null)
                {
                    return;
                }

                Point loc = new Point
                {
                    X = (pictureBox1.Width - _animation[_frameIndex].Bitmap.Width) / 2,
                    Y = (pictureBox1.Height - _animation[_frameIndex].Bitmap.Height) / 2
                };
                e.Graphics.DrawImage(_animation[_frameIndex].Bitmap, loc);
            }
            else
            {
                int body = _currentSelect;
                Animations.Translate(ref body);
                int hue = 0;
                Frame[] frames = Animations.GetAnimation(_currentSelect, CurrentSelectAction, _facing, ref hue, false, false);
                if (frames?[0].Bitmap == null)
                {
                    return;
                }

                Point loc = new Point
                {
                    X = (pictureBox1.Width - frames[0].Bitmap.Width) / 2,
                    Y = (pictureBox1.Height - frames[0].Bitmap.Height) / 2
                };
                e.Graphics.DrawImage(frames[0].Bitmap, loc);
            }
        }

        private void OnClickAnimate(object sender, EventArgs e)
        {
            Animate = !Animate;
        }

        private void OnChangeType(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                return;
            }

            TreeNode node = treeView1.SelectedNode;
            if (node.Parent != null)
            {
                node = node.Parent;
            } ((int[])node.Tag)[1] = ComboBoxActionType.SelectedIndex;
            SetActionType(node, ((int[])node.Tag)[0], ComboBoxActionType.SelectedIndex);
        }

        private void OnScrollFacing(object sender, EventArgs e)
        {
            _facing = (facingbar.Value - 3) & 7;
            CurrentSelect = CurrentSelect;
        }

        private void OnClickAdd(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                return;
            }

            TreeNode node = treeView1.SelectedNode;
            if (node.Parent != null)
            {
                node = node.Parent;
            }

            _form.AddGraphic(((int[])node.Tag)[0], ((int[])node.Tag)[1], node.Text);
            treeView1.SelectedNode.Remove();
        }
    }

    public class AnimNewListGraphicSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            if (tx.Parent != null)
            {
                return 0;
            }

            if (ty.Parent != null)
            {
                return 0;
            }

            int[] ix = (int[])tx.Tag;
            int[] iy = (int[])ty.Tag;
            if (ix[0] == iy[0])
            {
                return 0;
            }
            else if (ix[0] < iy[0])
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}