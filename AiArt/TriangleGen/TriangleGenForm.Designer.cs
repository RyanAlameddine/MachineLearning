namespace TriangleGen
{
    partial class TriangleGenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ImagePreview = new System.Windows.Forms.PictureBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.OpenImageButton = new System.Windows.Forms.Button();
            this.GeneratedImagePreview = new System.Windows.Forms.PictureBox();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SplitButton = new System.Windows.Forms.Button();
            this.mutationRateTracker = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.colorMutationIntensityTracker = new System.Windows.Forms.TrackBar();
            this.positionMutationTracker = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GeneratedImagePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutationRateTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMutationIntensityTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionMutationTracker)).BeginInit();
            this.SuspendLayout();
            // 
            // ImagePreview
            // 
            this.ImagePreview.Location = new System.Drawing.Point(175, 12);
            this.ImagePreview.Name = "ImagePreview";
            this.ImagePreview.Size = new System.Drawing.Size(446, 486);
            this.ImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImagePreview.TabIndex = 0;
            this.ImagePreview.TabStop = false;
            // 
            // RunButton
            // 
            this.RunButton.Enabled = false;
            this.RunButton.Location = new System.Drawing.Point(94, 12);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // OpenImageButton
            // 
            this.OpenImageButton.Location = new System.Drawing.Point(12, 12);
            this.OpenImageButton.Name = "OpenImageButton";
            this.OpenImageButton.Size = new System.Drawing.Size(75, 23);
            this.OpenImageButton.TabIndex = 2;
            this.OpenImageButton.Text = "Open Image";
            this.OpenImageButton.UseVisualStyleBackColor = true;
            this.OpenImageButton.Click += new System.EventHandler(this.OpenImageButton_Click);
            // 
            // GeneratedImagePreview
            // 
            this.GeneratedImagePreview.Location = new System.Drawing.Point(631, 10);
            this.GeneratedImagePreview.Name = "GeneratedImagePreview";
            this.GeneratedImagePreview.Size = new System.Drawing.Size(446, 486);
            this.GeneratedImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.GeneratedImagePreview.TabIndex = 3;
            this.GeneratedImagePreview.TabStop = false;
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "OpenFileDialog";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // SplitButton
            // 
            this.SplitButton.Location = new System.Drawing.Point(94, 41);
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.Size = new System.Drawing.Size(75, 23);
            this.SplitButton.TabIndex = 5;
            this.SplitButton.Text = "split";
            this.SplitButton.UseVisualStyleBackColor = true;
            this.SplitButton.Click += new System.EventHandler(this.SplitButton_Click);
            // 
            // mutationRateTracker
            // 
            this.mutationRateTracker.LargeChange = 10;
            this.mutationRateTracker.Location = new System.Drawing.Point(13, 97);
            this.mutationRateTracker.Maximum = 100;
            this.mutationRateTracker.Name = "mutationRateTracker";
            this.mutationRateTracker.Size = new System.Drawing.Size(156, 69);
            this.mutationRateTracker.TabIndex = 6;
            this.mutationRateTracker.Scroll += new System.EventHandler(this.mutationRateTracker_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 234);
            this.label1.TabIndex = 7;
            this.label1.Text = "mutation rate\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\ncolor mutation intensity\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nposition m" +
    "utation intensity";
            // 
            // colorMutationIntensityTracker
            // 
            this.colorMutationIntensityTracker.LargeChange = 10;
            this.colorMutationIntensityTracker.Location = new System.Drawing.Point(12, 218);
            this.colorMutationIntensityTracker.Maximum = 255;
            this.colorMutationIntensityTracker.Minimum = 1;
            this.colorMutationIntensityTracker.Name = "colorMutationIntensityTracker";
            this.colorMutationIntensityTracker.Size = new System.Drawing.Size(156, 69);
            this.colorMutationIntensityTracker.TabIndex = 8;
            this.colorMutationIntensityTracker.Value = 1;
            this.colorMutationIntensityTracker.Scroll += new System.EventHandler(this.colorMutationIntensityTracker_Scroll);
            // 
            // positionMutationTracker
            // 
            this.positionMutationTracker.LargeChange = 10;
            this.positionMutationTracker.Location = new System.Drawing.Point(15, 318);
            this.positionMutationTracker.Maximum = 100;
            this.positionMutationTracker.Minimum = 1;
            this.positionMutationTracker.Name = "positionMutationTracker";
            this.positionMutationTracker.Size = new System.Drawing.Size(156, 69);
            this.positionMutationTracker.TabIndex = 9;
            this.positionMutationTracker.Value = 1;
            this.positionMutationTracker.Scroll += new System.EventHandler(this.positionMutationTracker_Scroll);
            // 
            // TriangleGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 508);
            this.Controls.Add(this.positionMutationTracker);
            this.Controls.Add(this.colorMutationIntensityTracker);
            this.Controls.Add(this.mutationRateTracker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SplitButton);
            this.Controls.Add(this.GeneratedImagePreview);
            this.Controls.Add(this.OpenImageButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.ImagePreview);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TriangleGenForm";
            this.Text = "Genetic Tri Gen";
            ((System.ComponentModel.ISupportInitialize)(this.ImagePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GeneratedImagePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutationRateTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMutationIntensityTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionMutationTracker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ImagePreview;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button OpenImageButton;
        private System.Windows.Forms.PictureBox GeneratedImagePreview;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button SplitButton;
        private System.Windows.Forms.TrackBar mutationRateTracker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar colorMutationIntensityTracker;
        private System.Windows.Forms.TrackBar positionMutationTracker;
    }
}

