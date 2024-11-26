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
            clearImage = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            asmLibrary = new RadioButton();
            cLibrary = new RadioButton();
            filterButton = new Button();
            imageUpload = new Button();
            rightPanel = new Panel();
            rightPanelLabel = new Label();
            imageAfterFilter = new PictureBox();
            leftPanel = new Panel();
            leftPanelLabel = new Label();
            imageBeforeFilter = new PictureBox();
            imageList1 = new ImageList(components);
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
            panel1.Location = new Point(8, 7);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1165, 543);
            panel1.TabIndex = 0;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(clearImage);
            bottomPanel.Controls.Add(textBox1);
            bottomPanel.Controls.Add(label2);
            bottomPanel.Controls.Add(label1);
            bottomPanel.Controls.Add(asmLibrary);
            bottomPanel.Controls.Add(cLibrary);
            bottomPanel.Controls.Add(filterButton);
            bottomPanel.Controls.Add(imageUpload);
            bottomPanel.Location = new Point(3, 284);
            bottomPanel.Margin = new Padding(2);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(1160, 240);
            bottomPanel.TabIndex = 0;
            bottomPanel.Paint += bottomPanel_Paint;
            // 
            // clearImage
            // 
            clearImage.Location = new Point(402, 56);
            clearImage.Margin = new Padding(2);
            clearImage.Name = "clearImage";
            clearImage.Size = new Size(120, 50);
            clearImage.TabIndex = 7;
            clearImage.Text = "Wyczyść zdjecie";
            clearImage.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(92, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(305, 23);
            textBox1.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 20);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 5;
            label2.Text = "Lokalizacja:";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 107);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 4;
            label1.Text = "Wybierz bibliotekę:";
            // 
            // asmLibrary
            // 
            asmLibrary.AutoSize = true;
            asmLibrary.Location = new Point(30, 145);
            asmLibrary.Margin = new Padding(2);
            asmLibrary.Name = "asmLibrary";
            asmLibrary.Size = new Size(49, 19);
            asmLibrary.TabIndex = 3;
            asmLibrary.TabStop = true;
            asmLibrary.Text = "Asm";
            asmLibrary.UseVisualStyleBackColor = true;
            // 
            // cLibrary
            // 
            cLibrary.AutoSize = true;
            cLibrary.Location = new Point(30, 124);
            cLibrary.Margin = new Padding(2);
            cLibrary.Name = "cLibrary";
            cLibrary.Size = new Size(40, 19);
            cLibrary.TabIndex = 2;
            cLibrary.TabStop = true;
            cLibrary.Text = "C#";
            cLibrary.UseVisualStyleBackColor = true;
            cLibrary.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // filterButton
            // 
            filterButton.Location = new Point(780, 2);
            filterButton.Margin = new Padding(2);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(240, 104);
            filterButton.TabIndex = 1;
            filterButton.Text = "Filtruj zdjecie";
            filterButton.UseVisualStyleBackColor = true;
            filterButton.Click += filterButton_Click;
            // 
            // imageUpload
            // 
            imageUpload.Location = new Point(402, 2);
            imageUpload.Margin = new Padding(2);
            imageUpload.Name = "imageUpload";
            imageUpload.Size = new Size(120, 50);
            imageUpload.TabIndex = 0;
            imageUpload.Text = "Wybierz zdjecie";
            imageUpload.UseVisualStyleBackColor = true;
            imageUpload.Click += button1_Click;
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(rightPanelLabel);
            rightPanel.Controls.Add(imageAfterFilter);
            rightPanel.Location = new Point(613, 4);
            rightPanel.Margin = new Padding(2);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(550, 260);
            rightPanel.TabIndex = 1;
            // 
            // rightPanelLabel
            // 
            rightPanelLabel.AutoSize = true;
            rightPanelLabel.Location = new Point(21, 19);
            rightPanelLabel.Margin = new Padding(2, 0, 2, 0);
            rightPanelLabel.Name = "rightPanelLabel";
            rightPanelLabel.Size = new Size(122, 15);
            rightPanelLabel.TabIndex = 2;
            rightPanelLabel.Text = "Zdjęcie po filtrowaniu";
            // 
            // imageAfterFilter
            // 
            imageAfterFilter.Location = new Point(83, 36);
            imageAfterFilter.Margin = new Padding(2);
            imageAfterFilter.Name = "imageAfterFilter";
            imageAfterFilter.Size = new Size(400, 200);
            imageAfterFilter.SizeMode = PictureBoxSizeMode.CenterImage;
            imageAfterFilter.TabIndex = 1;
            imageAfterFilter.TabStop = false;
            // 
            // leftPanel
            // 
            leftPanel.Controls.Add(leftPanelLabel);
            leftPanel.Controls.Add(imageBeforeFilter);
            leftPanel.Location = new Point(3, 4);
            leftPanel.Margin = new Padding(2);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(550, 260);
            leftPanel.TabIndex = 0;
            // 
            // leftPanelLabel
            // 
            leftPanelLabel.AutoSize = true;
            leftPanelLabel.Location = new Point(19, 19);
            leftPanelLabel.Margin = new Padding(2, 0, 2, 0);
            leftPanelLabel.Name = "leftPanelLabel";
            leftPanelLabel.Size = new Size(147, 15);
            leftPanelLabel.TabIndex = 1;
            leftPanelLabel.Text = "Zdjęcie przed filtrowaniem";
            leftPanelLabel.Click += label1_Click;
            // 
            // imageBeforeFilter
            // 
            imageBeforeFilter.Location = new Point(65, 36);
            imageBeforeFilter.Margin = new Padding(2);
            imageBeforeFilter.Name = "imageBeforeFilter";
            imageBeforeFilter.Size = new Size(400, 200);
            imageBeforeFilter.SizeMode = PictureBoxSizeMode.CenterImage;
            imageBeforeFilter.TabIndex = 0;
            imageBeforeFilter.TabStop = false;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 561);
            Controls.Add(panel1);
            Margin = new Padding(2);
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
        private Label label2;
        private TextBox textBox1;
        private Button clearImage;
    }
}
