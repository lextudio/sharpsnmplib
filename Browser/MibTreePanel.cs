/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using Lextm.SharpSnmpLib.Mib;

namespace Browser
{
	/// <summary>
	/// Description of MibTreePanel.
	/// </summary>
	public partial class MibTreePanel : DockContent
	{
		public MibTreePanel()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
            ObjectRegistry repository = ObjectRegistry.Instance;
            TreeNode root = Wrap(repository.Tree.Root);
            foreach (TreeNode node in root.Nodes)
            {
            	treeView1.Nodes.Add(node);
            }
		}

        private static TreeNode Wrap(Definition definition)
        {
            TreeNode node = new TreeNode(definition.Name) { Tag = definition, ToolTipText = definition.TextualForm };
            foreach (Definition def in definition.Children)
            {
            	node.Nodes.Add(Wrap(def));
            }
            return node;
        }
	}
}
