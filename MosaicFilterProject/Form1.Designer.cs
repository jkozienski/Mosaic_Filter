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
            saveImage = new Button();
            mosaicPowerThread = new Label();
            threadNumberText = new Label();
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
            testBtn = new Button();
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
            panel1.Location = new Point(11, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1664, 905);
            panel1.TabIndex = 0;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(testBtn);
            bottomPanel.Controls.Add(saveImage);
            bottomPanel.Controls.Add(mosaicPowerThread);
            bottomPanel.Controls.Add(threadNumberText);
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
            bottomPanel.Location = new Point(4, 473);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(1657, 400);
            bottomPanel.TabIndex = 0;
            bottomPanel.Paint += bottomPanel_Paint;
            // 
            // saveImage
            // 
            saveImage.Location = new Point(1291, 28);
            saveImage.Name = "saveImage";
            saveImage.Size = new Size(241, 118);
            saveImage.TabIndex = 71;
            saveImage.Text = "Zapisz obraz";
            saveImage.UseVisualStyleBackColor = true;
            saveImage.Click += saveImage_Click;
            // 
            // mosaicPowerThread
            // 
            mosaicPowerThread.AutoSize = true;
            mosaicPowerThread.Location = new Point(1570, 322);
            mosaicPowerThread.Margin = new Padding(4, 0, 4, 0);
            mosaicPowerThread.Name = "mosaicPowerThread";
            mosaicPowerThread.Size = new Size(0, 25);
            mosaicPowerThread.TabIndex = 70;
            // 
            // threadNumberText
            // 
            threadNumberText.AutoSize = true;
            threadNumberText.Location = new Point(1570, 242);
            threadNumberText.Margin = new Padding(4, 0, 4, 0);
            threadNumberText.Name = "threadNumberText";
            threadNumberText.Size = new Size(0, 25);
            threadNumberText.TabIndex = 69;
            threadNumberText.Click += threadNumberText_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(857, 322);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(146, 25);
            label5.TabIndex = 65;
            label5.Text = "Rozmiar mozaiki:";
            // 
            // mosaicPower
            // 
            mosaicPower.Location = new Point(990, 320);
            mosaicPower.Margin = new Padding(4, 5, 4, 5);
            mosaicPower.Maximum = 160;
            mosaicPower.Minimum = 4;
            mosaicPower.Name = "mosaicPower";
            mosaicPower.Size = new Size(571, 69);
            mosaicPower.SmallChange = 4;
            mosaicPower.TabIndex = 66;
            mosaicPower.TickFrequency = 4;
            mosaicPower.Value = 4;
            mosaicPower.Scroll += mosaicPower_Scroll;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(857, 242);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(130, 25);
            label4.TabIndex = 10;
            label4.Text = "Liczba wątków:";
            // 
            // threadNumber
            // 
            threadNumber.Location = new Point(990, 242);
            threadNumber.Margin = new Padding(4, 5, 4, 5);
            threadNumber.Maximum = 64;
            threadNumber.Minimum = 1;
            threadNumber.Name = "threadNumber";
            threadNumber.Size = new Size(571, 69);
            threadNumber.TabIndex = 64;
            threadNumber.Value = 1;
            threadNumber.Scroll += trackBar1_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(857, 202);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(133, 25);
            label3.TabIndex = 8;
            label3.Text = "Parametryzacja:";
            // 
            // clearImage
            // 
            clearImage.Location = new Point(574, 93);
            clearImage.Name = "clearImage";
            clearImage.Size = new Size(171, 83);
            clearImage.TabIndex = 7;
            clearImage.Text = "Wyczyść obraz";
            clearImage.UseVisualStyleBackColor = true;
            clearImage.Click += clearImage_Click;
            // 
            // locationTextBox
            // 
            locationTextBox.Location = new Point(131, 28);
            locationTextBox.Margin = new Padding(4, 5, 4, 5);
            locationTextBox.Name = "locationTextBox";
            locationTextBox.Size = new Size(434, 31);
            locationTextBox.TabIndex = 6;
            locationTextBox.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 33);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(99, 25);
            label2.TabIndex = 5;
            label2.Text = "Lokalizacja:";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 202);
            label1.Name = "label1";
            label1.Size = new Size(163, 25);
            label1.TabIndex = 4;
            label1.Text = "Wybierz bibliotekę:";
            // 
            // radioASM
            // 
            radioASM.AutoSize = true;
            radioASM.Checked = true;
            radioASM.Location = new Point(43, 265);
            radioASM.Name = "radioASM";
            radioASM.Size = new Size(73, 29);
            radioASM.TabIndex = 3;
            radioASM.TabStop = true;
            radioASM.Text = "Asm";
            radioASM.UseVisualStyleBackColor = true;
            // 
            // radioCSharp
            // 
            radioCSharp.AutoSize = true;
            radioCSharp.Location = new Point(43, 230);
            radioCSharp.Name = "radioCSharp";
            radioCSharp.Size = new Size(59, 29);
            radioCSharp.TabIndex = 2;
            radioCSharp.Text = "C#";
            radioCSharp.UseVisualStyleBackColor = true;
            radioCSharp.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // filterButton
            // 
            filterButton.Location = new Point(955, 28);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(241, 118);
            filterButton.TabIndex = 1;
            filterButton.Text = "Filtruj obraz";
            filterButton.UseVisualStyleBackColor = true;
            filterButton.Click += filterButton_Click;
            // 
            // imageUpload
            // 
            imageUpload.Location = new Point(574, 3);
            imageUpload.Name = "imageUpload";
            imageUpload.Size = new Size(171, 83);
            imageUpload.TabIndex = 0;
            imageUpload.Text = "Wybierz obraz";
            imageUpload.UseVisualStyleBackColor = true;
            imageUpload.Click += imageUpload_Click;
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(rightPanelLabel);
            rightPanel.Controls.Add(imageAfterFilter);
            rightPanel.Location = new Point(876, 7);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(786, 433);
            rightPanel.TabIndex = 1;
            // 
            // rightPanelLabel
            // 
            rightPanelLabel.AutoSize = true;
            rightPanelLabel.Location = new Point(30, 32);
            rightPanelLabel.Name = "rightPanelLabel";
            rightPanelLabel.Size = new Size(182, 25);
            rightPanelLabel.TabIndex = 2;
            rightPanelLabel.Text = "Zdjęcie po filtrowaniu";
            // 
            // imageAfterFilter
            // 
            imageAfterFilter.Location = new Point(119, 80);
            imageAfterFilter.Name = "imageAfterFilter";
            imageAfterFilter.Size = new Size(571, 333);
            imageAfterFilter.SizeMode = PictureBoxSizeMode.Zoom;
            imageAfterFilter.TabIndex = 1;
            imageAfterFilter.TabStop = false;
            // 
            // leftPanel
            // 
            leftPanel.Controls.Add(leftPanelLabel);
            leftPanel.Controls.Add(imageBeforeFilter);
            leftPanel.Location = new Point(4, 7);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(786, 433);
            leftPanel.TabIndex = 0;
            // 
            // leftPanelLabel
            // 
            leftPanelLabel.AutoSize = true;
            leftPanelLabel.Location = new Point(27, 32);
            leftPanelLabel.Name = "leftPanelLabel";
            leftPanelLabel.Size = new Size(220, 25);
            leftPanelLabel.TabIndex = 1;
            leftPanelLabel.Text = "Zdjęcie przed filtrowaniem";
            leftPanelLabel.Click += label1_Click;
            // 
            // imageBeforeFilter
            // 
            imageBeforeFilter.Location = new Point(93, 80);
            imageBeforeFilter.Name = "imageBeforeFilter";
            imageBeforeFilter.Size = new Size(571, 333);
            imageBeforeFilter.SizeMode = PictureBoxSizeMode.Zoom;
            imageBeforeFilter.TabIndex = 0;
            imageBeforeFilter.TabStop = false;
            // 
            // testBtn
            // 
            testBtn.Location = new Point(574, 184);
            testBtn.Name = "testBtn";
            testBtn.Size = new Size(171, 83);
            testBtn.TabIndex = 72;
            testBtn.Text = "Test";
            testBtn.UseVisualStyleBackColor = true;
            testBtn.Click += testBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1691, 935);
            Controls.Add(panel1);
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
        private Label mosaicPowerThread;
        private Label threadNumberText;
        private Button saveImage;
        private Button testBtn;
    }
}
