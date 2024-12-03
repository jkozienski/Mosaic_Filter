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
            panel1 = new Panel();
            bottomPanel = new Panel();
            label5 = new Label();
            mosaicPower = new TrackBar();
            label4 = new Label();
            threadNumber = new TrackBar();
            label3 = new Label();
            clearImage = new Button();
            locationTextBox = new TextBox();
            label2 = new Label();
            label1 = new Label();
            radioASM = new RadioButton();
            radioCSharp = new RadioButton();
            filterButton = new Button();
            imageUpload = new Button();
            rightPanel = new Panel();
            rightPanelLabel = new Label();
            imageAfterFilter = new PictureBox();
            leftPanel = new Panel();
            leftPanelLabel = new Label();
            imageBeforeFilter = new PictureBox();
            panel1.SuspendLayout();
            bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mosaicPower).BeginInit();
            ((System.ComponentModel.ISupportInitialize)threadNumber).BeginInit();
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
            bottomPanel.Controls.Add(label5);
            bottomPanel.Controls.Add(mosaicPower);
            bottomPanel.Controls.Add(label4);
            bottomPanel.Controls.Add(threadNumber);
            bottomPanel.Controls.Add(label3);
            bottomPanel.Controls.Add(clearImage);
            bottomPanel.Controls.Add(locationTextBox);
            bottomPanel.Controls.Add(label2);
            bottomPanel.Controls.Add(label1);
            bottomPanel.Controls.Add(radioASM);
            bottomPanel.Controls.Add(radioCSharp);
            bottomPanel.Controls.Add(filterButton);
            bottomPanel.Controls.Add(imageUpload);
            bottomPanel.Location = new Point(3, 284);
            bottomPanel.Margin = new Padding(2);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(1160, 240);
            bottomPanel.TabIndex = 0;
            bottomPanel.Paint += bottomPanel_Paint;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(734, 204);
            label5.Name = "label5";
            label5.Size = new Size(97, 15);
            label5.TabIndex = 65;
            label5.Text = "Rozmiar mozaiki:";
            // 
            // mosaicPower
            // 
            mosaicPower.Location = new Point(837, 195);
            mosaicPower.Maximum = 100;
            mosaicPower.Minimum = 1;
            mosaicPower.Name = "mosaicPower";
            mosaicPower.Size = new Size(120, 45);
            mosaicPower.TabIndex = 66;
            mosaicPower.Value = 4;
            mosaicPower.Scroll += mosaicPower_Scroll;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(600, 145);
            label4.Name = "label4";
            label4.Size = new Size(87, 15);
            label4.TabIndex = 10;
            label4.Text = "Liczba wątków:";
            // 
            // threadNumber
            // 
            threadNumber.Location = new Point(693, 145);
            threadNumber.Maximum = 64;
            threadNumber.Minimum = 1;
            threadNumber.Name = "threadNumber";
            threadNumber.Size = new Size(400, 45);
            threadNumber.TabIndex = 64;
            threadNumber.Value = 1;
            threadNumber.Scroll += trackBar1_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(600, 121);
            label3.Name = "label3";
            label3.Size = new Size(90, 15);
            label3.TabIndex = 8;
            label3.Text = "Parametryzacja:";
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
            clearImage.Click += clearImage_Click;
            // 
            // locationTextBox
            // 
            locationTextBox.Location = new Point(92, 17);
            locationTextBox.Name = "locationTextBox";
            locationTextBox.Size = new Size(305, 23);
            locationTextBox.TabIndex = 6;
            locationTextBox.TextChanged += textBox1_TextChanged;
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
            label1.Location = new Point(19, 121);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 4;
            label1.Text = "Wybierz bibliotekę:";
            // 
            // radioASM
            // 
            radioASM.AutoSize = true;
            radioASM.Location = new Point(30, 159);
            radioASM.Margin = new Padding(2);
            radioASM.Name = "radioASM";
            radioASM.Size = new Size(49, 19);
            radioASM.TabIndex = 3;
            radioASM.Text = "Asm";
            radioASM.UseVisualStyleBackColor = true;
            // 
            // radioCSharp
            // 
            radioCSharp.AutoSize = true;
            radioCSharp.Checked = true;
            radioCSharp.Location = new Point(30, 138);
            radioCSharp.Margin = new Padding(2);
            radioCSharp.Name = "radioCSharp";
            radioCSharp.Size = new Size(40, 19);
            radioCSharp.TabIndex = 2;
            radioCSharp.TabStop = true;
            radioCSharp.Text = "C#";
            radioCSharp.UseVisualStyleBackColor = true;
            radioCSharp.CheckedChanged += radioButton1_CheckedChanged;
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
            imageAfterFilter.Location = new Point(83, 48);
            imageAfterFilter.Margin = new Padding(2);
            imageAfterFilter.Name = "imageAfterFilter";
            imageAfterFilter.Size = new Size(400, 200);
            imageAfterFilter.SizeMode = PictureBoxSizeMode.Zoom;
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
            imageBeforeFilter.Location = new Point(65, 48);
            imageBeforeFilter.Margin = new Padding(2);
            imageBeforeFilter.Name = "imageBeforeFilter";
            imageBeforeFilter.Size = new Size(400, 200);
            imageBeforeFilter.SizeMode = PictureBoxSizeMode.Zoom;
            imageBeforeFilter.TabIndex = 0;
            imageBeforeFilter.TabStop = false;
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
            ((System.ComponentModel.ISupportInitialize)mosaicPower).EndInit();
            ((System.ComponentModel.ISupportInitialize)threadNumber).EndInit();
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
        private Button filterButton;
        private RadioButton radioASM;
        private RadioButton radioCSharp;
        private Label label1;
        private Label label2;
        private TextBox locationTextBox;
        private Button clearImage;
        private Label label3;
        private TrackBar threadNumber;
        private Label label5;
        private TrackBar mosaicPower;
        private Label label4;
    }
}
