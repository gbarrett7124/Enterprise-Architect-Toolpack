﻿/*
 * Created by SharpDevelop.
 * User: LaptopGeert
 * Date: 2/07/2016
 * Time: 7:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace EAImvertor
{
	[Guid("16F72602-8F86-4E26-AFBD-4A0FB873CA09")]
	[ComVisible(true)]
	/// <summary>
	/// Description of ImvertorControl.
	/// </summary>
	public partial class ImvertorControl : UserControl
	{
		//private attributes
		private List<EAImvertorJob> jobs = new List<EAImvertorJob>();
		public EAImvertorJob selectedJob
		{
			get
			{
				try
				{
					var selectedItems = imvertorJobGrid.SelectedItems;
					if (selectedItems.Count > 0)
					{
						return selectedItems[0].Tag as EAImvertorJob;
					}
					else
					{
						return null;
					}
				}
				catch(Exception)
				{
					return null;
				}
				
			}
		}
		public ImvertorControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.refreshToolTip.SetToolTip(this.refreshButton, "Refresh Status");
			this.resizeGridColumns();
		}
		public void addJob(EAImvertorJob job)
		{
			this.jobs.Insert(0,job);
			var row = new ListViewItem(job.sourcePackage.name);
			row.SubItems.Add(job.status);
			row.Tag = job;
			this.imvertorJobGrid.Items.Insert(0,row);
			row.Selected = true;
		}

		public void clear()
		{
			this.jobs.Clear();
			refreshJobInfo(null);
		}

		public void refreshJobInfo(EAImvertorJob imvertorJob)
		{
			try
			{
				foreach (ListViewItem row in imvertorJobGrid.Items) 
				{
					var currentJob = (EAImvertorJob) row.Tag;
					if (imvertorJob == null || currentJob.Equals(imvertorJob) )
					{
						string tries = string.Empty;
						string timedOut = string.Empty;
						if (!(currentJob.status.StartsWith("Finished") || currentJob.status.StartsWith("Error")))
						{
						   	if (currentJob.timedOut)
						   	{
						   		timedOut = " (Timed Out)";
						   	}
						    else if (currentJob.tries > 0)
							{
								tries = new string('.',currentJob.tries);
							}
						 }
						row.SubItems[1].Text =currentJob.status + tries + timedOut;
					}

				}
				this.enableDisable();
			}
			catch(Exception)
			{
				//do nothing. TODO: figure out a thread safe way to refresh control
			}
		}
		private void resizeGridColumns()
		{
			//set the last column to fill
			this.imvertorJobGrid.Columns[imvertorJobGrid.Columns.Count - 1].Width = -2;
		}
		private void enableDisable()
		{
			if (this.selectedJob != null
			    && this.selectedJob.status == "Finished")
			{
				this.resultsButton.Enabled = true;
				this.viewWarningsButton.Enabled = true;
				this.refreshButton.Enabled = false;
			}
			else
			{
				this.resultsButton.Enabled = false;
				this.viewWarningsButton.Enabled = false;
				this.refreshButton.Enabled = (this.selectedJob != null && this.selectedJob.timedOut);
			}
		}
		public event EventHandler retryButtonClick;
		void RetryButtonClick(object sender, EventArgs e)
		{
			if (this.retryButtonClick != null)
			{
				retryButtonClick(sender, e);
			}
		}
		public event EventHandler resultsButtonClick;
		void ResultsButtonClick(object sender, EventArgs e)
		{
			if (this.resultsButtonClick != null)
			{
				resultsButtonClick(sender, e);
			}

		}
		public event EventHandler viewWarningsButtonClick;
		void ViewWarningsButtonClick(object sender, EventArgs e)
		{
			if (this.viewWarningsButtonClick != null)
			{
				viewWarningsButtonClick(sender, e);
			}
		}
		void ImvertorJobGridResize(object sender, EventArgs e)
		{
			this.resizeGridColumns();
		}
		void ImvertorJobGridSelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.selectedJob != null)
			{
				this.jobIDTextBox.Text = selectedJob.jobID;
				this.propertiesTextBox.Text = selectedJob.settings.defaultProperties;
				this.processTextBox.Text = selectedJob.settings.defaultProcessName;
				this.historyFileTextBox.Text = selectedJob.settings.defaultHistoryFilePath;
				this.propertiesFileTextBox.Text = selectedJob.settings.defaultPropertiesFilePath;
			}
			else
			{
				//clear fields
				this.jobIDTextBox.Text = string.Empty;
				this.propertiesTextBox.Text = string.Empty;
				this.processTextBox.Text = string.Empty;
				this.historyFileTextBox.Text = string.Empty;
				this.propertiesFileTextBox.Text = string.Empty;
			}
			this.enableDisable();
		}
		
		void RefreshButtonClick(object sender, EventArgs e)
		{
			if (this.selectedJob != null)
			{
				this.selectedJob.refreshStatus();
				this.refreshJobInfo(this.selectedJob);
			}
		}


		
	}
}
