using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradeCalculator1
{
	public partial class trdcalc1 : Form
	{
		private double dbl_enter, dbl_sl, dbl_exit1, dbl_exit2;
		private double dbl_height, dbl_high, dbl_low, dbl_val1, dbl_val2;
		private double orderSize = 1.0;
		const double perc10 = 0.1;
		const double perc25 = 0.25;
		const double pip = 10000.0;

 
		public trdcalc1()
		{
			InitializeComponent();
			this.direction.SelectedIndex = 1;
			Initialize();
		}

		private void Initialize()
		{
			this.highData.Focus();
		}

		private void Btn_Calc_Click(object sender, EventArgs e)
		{
			try
			{
				dbl_high = double.Parse(highData.Text);
				dbl_low = double.Parse(lowData.Text);
				orderSize = double.Parse(boxOrderSize.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message,"Input Error",MessageBoxButtons.OK);
				return;
			}

			CalculateData();
		}

		private void CalculateData()
		{
			int i_10perc, i_25perc;
			double pipExit1, pipExit2, pipTot, amExit1, amExit2, amTot;
			dbl_height = Math.Abs(dbl_high - dbl_low);
			dbl_val1 = perc10 * dbl_height*pip;
			dbl_val2 = perc25 * dbl_height*pip;
			i_10perc = (int)Math.Round(dbl_val1);
			i_25perc = (int)Math.Round(dbl_val2);
			pipExit1 = double.Parse(i_25perc.ToString());
			pipExit2 = dbl_height * pip - double.Parse(i_10perc.ToString());
			pipTot = pipExit1 + pipExit2;
			amExit1 = pipExit1 * 10 * orderSize;
			amExit2 = pipExit2 * 10 * orderSize;
			amTot = pipTot * 10 * orderSize;

			if (direction.SelectedIndex == 0) // Up direction
			{
				dbl_enter = dbl_high + double.Parse(i_10perc.ToString())/pip;
				dbl_sl = dbl_enter - double.Parse(i_25perc.ToString())/pip;
				dbl_exit1 = dbl_enter + double.Parse(i_25perc.ToString())/pip;
				dbl_exit2 = dbl_high + dbl_height;
			}
			else if (direction.SelectedIndex == 1)// Down direction
			{
				dbl_enter = dbl_low - double.Parse(i_10perc.ToString()) / pip;
				dbl_sl = dbl_enter + double.Parse(i_25perc.ToString()) / pip;
				dbl_exit1 = dbl_enter - double.Parse(i_25perc.ToString()) / pip;
				dbl_exit2 = dbl_low - dbl_height;
			}

			this.entry.Text = this.dbl_enter.ToString("0.0000");
			this.exit1.Text = this.dbl_exit1.ToString("0.0000");
			this.exit2.Text = this.dbl_exit2.ToString("0.0000");
			this.stoploss.Text = this.dbl_sl.ToString("0.0000");
			this.poleHeight.Text = (this.dbl_height * pip).ToString("#,##0");
			this.box10Perc.Text = i_10perc.ToString("#,##0");
			this.box25Perc.Text = i_25perc.ToString("#,##0");
			this.pips_loss.Text = (2 * i_25perc).ToString("#,##0");
			this.pips_exit1.Text = pipExit1.ToString("#,##0");
			this.pips_exit2.Text = pipExit2.ToString("#,##0");
			this.pips_tot.Text = pipTot.ToString("#,##0");
			this.amount_loss.Text = (double.Parse((2 * i_25perc).ToString()) * 10.0 * orderSize).ToString("$#,##0.00");
			this.amount_exit1.Text = amExit1.ToString("$#,##0.00");
			this.amount_exit2.Text = amExit2.ToString("$#,##0.00");
			this.amount_tot.Text = amTot.ToString("$#,##0.00");
			
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox1 xd = new AboutBox1();
			xd.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void flagpoleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HideAll();
			this.flagpoleTabCntrl.Visible = true;
			this.flagpoleTabCntrl.SelectTab("tabPage1");
		}

		private void HideAll()
		{
			// Hide the flagpoleTabCntrl
			this.flagpoleTabCntrl.Visible = false;
			ClearFlagpole();
			
		}

		private void ClearFlagpole()
		{
			foreach (Control x in tabPage1.Controls)
			{
				if (x.GetType() == typeof(TextBox))
				{
					if (x.Name != "boxOrderSize") x.ResetText();
					else x.Text = "1.0";
				}
				if(x.GetType() == typeof(GroupBox))
				{
					foreach (Control xx in x.Controls)
					{
						if (xx.GetType() == typeof(TextBox)) xx.ResetText();
					}
				}
			}

			foreach (Control x in tabPage2.Controls)
			{
				if (x.GetType() == typeof(TextBox))
				{
					x.ResetText();
				}
				if (x.GetType() == typeof(GroupBox))
				{
					foreach (Control xx in x.Controls)
					{
						if (xx.GetType() == typeof(TextBox)) xx.ResetText();
					}
				}
			}
		}

		private void trdcalc1_SizeChanged(object sender, EventArgs e)
		{
			this.Size = new Size(250, 291);
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			//MessageBox.Show("Key Data: " + keyData.ToString());
			if (keyData.CompareTo(Keys.Return) == 0)
			{
				this.Btn_Calc.PerformClick();
				return true;
			}
			else if (keyData.CompareTo(Keys.Tab) == 0)
			{
				base.ProcessDialogKey(keyData);
				return true;
			}
			else
			{
				base.ProcessDialogKey(keyData);
				return false;
			}
		}

		private void tabPage1_Enter(object sender, EventArgs e)
		{
			this.highData.Focus();
		}
	}
}
