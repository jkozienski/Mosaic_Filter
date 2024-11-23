namespace MosaicFilterProject {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            bottomPanel = new Panel();
            filterButton = new Button();
            imageUpload = new Button();
            rightPanel = new Panel();
            rightPanelLabel = new Label();
            imageAfterFilter = new PictureBox();
            leftPanel = new Panel();
            leftPanelLabel = new Label();
            imageBeforeFilter = new PictureBox();
            imageList1 = new ImageList(components);
            cLibrary = new RadioButton();
            asmLibrary = new RadioButton();
            label1 = new Label();
            panel1.SuspendLayout();
            bottomPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imageAfterFilter).BeginInit();
            leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imageBeforeFilter).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(bottomPanel);
            panel1.Controls.Add(rightPanel);
            panel1.Controls.Add(leftPanel);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1234, 640);
            panel1.TabIndex = 0;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(label1);
            bottomPanel.Controls.Add(asmLibrary);
            bottomPanel.Controls.Add(cLibrary);
            bottomPanel.Controls.Add(filterButton);
            bottomPanel.Controls.Add(imageUpload);
            bottomPanel.Location = new Point(21, 332);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(1191, 305);
            bottomPanel.TabIndex = 0;
            // 
            // filterButton
            // 
            filterButton.Location = new Point(824, 14);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(169, 33);
            filterButton.TabIndex = 1;
            filterButton.Text = "Filtruj zdjecie";
            filterButton.UseVisualStyleBackColor = true;
            filterButton.Click += filterButton_Click;
            // 
            // imageUpload
            // 
            imageUpload.Location = new Point(141, 13);
            imageUpload.Name = "imageUpload";
            imageUpload.Size = new Size(170, 34);
            imageUpload.TabIndex = 0;
            imageUpload.Text = "Wybierz zdjecie";
            imageUpload.UseVisualStyleBackColor = true;
            imageUpload.Click += button1_Click;
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(rightPanelLabel);
            rightPanel.Controls.Add(imageAfterFilter);
            rightPanel.Location = new Point(662, 3);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(550, 300);
            rightPanel.TabIndex = 1;
            // 
            // rightPanelLabel
            // 
            rightPanelLabel.AutoSize = true;
            rightPanelLabel.Location = new Point(12, 13);
            rightPanelLabel.Name = "rightPanelLabel";
            rightPanelLabel.Size = new Size(182, 25);
            rightPanelLabel.TabIndex = 2;
            rightPanelLabel.Text = "Zdjęcie po filtrowaniu";
            // 
            // imageAfterFilter
            // 
            imageAfterFilter.Location = new Point(0, 41);
            imageAfterFilter.Name = "imageAfterFilter";
            imageAfterFilter.Size = new Size(544, 256);
            imageAfterFilter.TabIndex = 1;
            imageAfterFilter.TabStop = false;
            // 
            // leftPanel
            // 
            leftPanel.Controls.Add(leftPanelLabel);
            leftPanel.Controls.Add(imageBeforeFilter);
            leftPanel.Location = new Point(21, 3);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(550, 300);
            leftPanel.TabIndex = 0;
            // 
            // leftPanelLabel
            // 
            leftPanelLabel.AutoSize = true;
            leftPanelLabel.Location = new Point(10, 13);
            leftPanelLabel.Name = "leftPanelLabel";
            leftPanelLabel.Size = new Size(220, 25);
            leftPanelLabel.TabIndex = 1;
            leftPanelLabel.Text = "Zdjęcie przed filtrowaniem";
            leftPanelLabel.Click += label1_Click;
            // 
            // imageBeforeFilter
            // 
            imageBeforeFilter.Location = new Point(3, 41);
            imageBeforeFilter.Name = "imageBeforeFilter";
            imageBeforeFilter.Size = new Size(544, 256);
            imageBeforeFilter.TabIndex = 0;
            imageBeforeFilter.TabStop = false;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // cLibrary
            // 
            cLibrary.AutoSize = true;
            cLibrary.Location = new Point(26, 206);
            cLibrary.Name = "cLibrary";
            cLibrary.Size = new Size(59, 29);
            cLibrary.TabIndex = 2;
            cLibrary.TabStop = true;
            cLibrary.Text = "C#";
            cLibrary.UseVisualStyleBackColor = true;
            cLibrary.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // asmLibrary
            // 
            asmLibrary.AutoSize = true;
            asmLibrary.Location = new Point(26, 241);
            asmLibrary.Name = "asmLibrary";
            asmLibrary.Size = new Size(73, 29);
            asmLibrary.TabIndex = 3;
            asmLibrary.TabStop = true;
            asmLibrary.Text = "Asm";
            asmLibrary.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 178);
            label1.Name = "label1";
            label1.Size = new Size(163, 25);
            label1.TabIndex = 4;
            label1.Text = "Wybierz bibliotekę:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 664);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Mosaic Filter by Jakub Kozieński";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            rightPanel.ResumeLayout(false);
            rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imageAfterFilter).EndInit();
            leftPanel.ResumeLayout(false);
            leftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imageBeforeFilter).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel leftPanel;
        private Panel bottomPanel;
        private Panel rightPanel;
        private PictureBox imageBeforeFilter;
        private PictureBox imageAfterFilter;
        private Button imageUpload;
        private Label leftPanelLabel;
        private Label rightPanelLabel;
        private ImageList imageList1;
        private Button filterButton;
        private RadioButton asmLibrary;
        private RadioButton cLibrary;
        private Label label1;
    }
}
