namespace WeightPlatePlugin
{
    partial class WeightPlatePlugin
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
            panel1 = new Panel();
            resetButton = new Button();
            buildButton = new Button();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            label8 = new Label();
            presetComboBox = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBoxG = new TextBox();
            textBoxL = new TextBox();
            textBoxR = new TextBox();
            textBoxHoleDiameter = new TextBox();
            textBoxT = new TextBox();
            textBoxD = new TextBox();
            label7 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(resetButton);
            panel1.Controls.Add(buildButton);
            panel1.Location = new Point(14, 276);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(411, 39);
            panel1.TabIndex = 0;
            // 
            // resetButton
            // 
            resetButton.Location = new Point(4, 3);
            resetButton.Margin = new Padding(4, 3, 4, 3);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(252, 28);
            resetButton.TabIndex = 1;
            resetButton.Text = "Сбросить до значений по умолчанию";
            resetButton.UseVisualStyleBackColor = true;
            resetButton.Click += resetButton_Click;
            // 
            // buildButton
            // 
            buildButton.Location = new Point(264, 3);
            buildButton.Margin = new Padding(4, 3, 4, 3);
            buildButton.Name = "buildButton";
            buildButton.Size = new Size(138, 29);
            buildButton.TabIndex = 0;
            buildButton.Text = "Построить";
            buildButton.UseVisualStyleBackColor = true;
            buildButton.Click += buildButton_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(groupBox1);
            panel2.Location = new Point(14, 12);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(441, 258);
            panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(presetComboBox);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBoxG);
            groupBox1.Controls.Add(textBoxL);
            groupBox1.Controls.Add(textBoxR);
            groupBox1.Controls.Add(textBoxHoleDiameter);
            groupBox1.Controls.Add(textBoxT);
            groupBox1.Controls.Add(textBoxD);
            groupBox1.Location = new Point(4, 3);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(434, 252);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Параметры";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(8, 36);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(162, 15);
            label8.TabIndex = 13;
            label8.Text = "Предустановка параметров:";
            // 
            // presetComboBox
            // 
            presetComboBox.FormattingEnabled = true;
            presetComboBox.Location = new Point(177, 33);
            presetComboBox.Name = "presetComboBox";
            presetComboBox.Size = new Size(220, 23);
            presetComboBox.TabIndex = 12;
            presetComboBox.SelectedIndexChanged += comboBoxPreset_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 226);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(238, 15);
            label6.TabIndex = 11;
            label6.Text = "Глубина внутреннего углубления G  (мм):";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 196);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(225, 15);
            label5.TabIndex = 10;
            label5.Text = "Радиус внутреннего углубления L (мм):";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 166);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(193, 15);
            label4.TabIndex = 9;
            label4.Text = "Радиус скругления фаски R  (мм):";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 136);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(158, 15);
            label3.TabIndex = 8;
            label3.Text = "Диаметр отверстия d  (мм):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 106);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(99, 15);
            label2.TabIndex = 7;
            label2.Text = "Толщина T (мм):";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 76);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 6;
            label1.Text = "Диаметр D (мм):";
            // 
            // textBoxG
            // 
            textBoxG.BackColor = Color.White;
            textBoxG.Location = new Point(280, 223);
            textBoxG.Margin = new Padding(4, 3, 4, 3);
            textBoxG.Name = "textBoxG";
            textBoxG.Size = new Size(116, 23);
            textBoxG.TabIndex = 5;
            textBoxG.Text = "25";
            textBoxG.TextChanged += textBox_TextChanged;
            textBoxG.Validating += textBox_Validating;
            // 
            // textBoxL
            // 
            textBoxL.Location = new Point(280, 193);
            textBoxL.Margin = new Padding(4, 3, 4, 3);
            textBoxL.Name = "textBoxL";
            textBoxL.Size = new Size(116, 23);
            textBoxL.TabIndex = 4;
            textBoxL.Text = "60";
            textBoxL.TextChanged += textBox_TextChanged;
            textBoxL.Validating += textBox_Validating;
            // 
            // textBoxR
            // 
            textBoxR.Location = new Point(280, 163);
            textBoxR.Margin = new Padding(4, 3, 4, 3);
            textBoxR.Name = "textBoxR";
            textBoxR.Size = new Size(116, 23);
            textBoxR.TabIndex = 3;
            textBoxR.Text = "45";
            textBoxR.TextChanged += textBox_TextChanged;
            textBoxR.Validating += textBox_Validating;
            // 
            // textBoxHoleDiameter
            // 
            textBoxHoleDiameter.Location = new Point(281, 133);
            textBoxHoleDiameter.Margin = new Padding(4, 3, 4, 3);
            textBoxHoleDiameter.Name = "textBoxHoleDiameter";
            textBoxHoleDiameter.Size = new Size(116, 23);
            textBoxHoleDiameter.TabIndex = 2;
            textBoxHoleDiameter.Text = "28";
            textBoxHoleDiameter.TextChanged += textBox_TextChanged;
            textBoxHoleDiameter.Validating += textBox_Validating;
            // 
            // textBoxT
            // 
            textBoxT.BackColor = Color.White;
            textBoxT.Location = new Point(280, 103);
            textBoxT.Margin = new Padding(4, 3, 4, 3);
            textBoxT.Name = "textBoxT";
            textBoxT.Size = new Size(116, 23);
            textBoxT.TabIndex = 1;
            textBoxT.Text = "50";
            textBoxT.TextChanged += textBox_TextChanged;
            textBoxT.Validating += textBox_Validating;
            // 
            // textBoxD
            // 
            textBoxD.Location = new Point(280, 73);
            textBoxD.Margin = new Padding(4, 3, 4, 3);
            textBoxD.Name = "textBoxD";
            textBoxD.Size = new Size(116, 23);
            textBoxD.TabIndex = 0;
            textBoxD.Text = "200";
            textBoxD.TextChanged += textBox_TextChanged;
            textBoxD.Validating += textBox_Validating;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(470, 237);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 2;
            label7.Text = "label7";
            // 
            // WeightPlatePlugin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 341);
            Controls.Add(label7);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(480, 380);
            MinimumSize = new Size(480, 380);
            Name = "WeightPlatePlugin";
            Text = "WeightPlatePlugin";
            FormClosing += WeightPlatePlugin_FormClosing;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxG;
        private System.Windows.Forms.TextBox textBoxL;
        private System.Windows.Forms.TextBox textBoxR;
        private System.Windows.Forms.TextBox textBoxHoleDiameter;
        private System.Windows.Forms.TextBox textBoxT;
        private System.Windows.Forms.TextBox textBoxD;
        private Label label8;
        private ComboBox presetComboBox;
        private Label label7;
    }
}

