using NCalc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        Dictionary<string, int> simbols = new Dictionary<string, int> { { "0", 0 }, { "1", 0 }, { "2", 0 }, { "3", 0 }, { "4", 0 }, { "5", 0 },
        { "+", 1 },{ "-", 1 },{ "*", 1 },{ "/", 1 }, { "(", 2 },{ ")", 2 }};
        int countBrackets = 0;
        bool isMinus = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void buttonNumber_Click(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;
            Action add = () => { textBox1.Text += text; isMinus = false; };

            if (simbols[text] == 0)
            {
                if (ValidateNum(text)) add();
            }
            else if (simbols[text] == 1)
            {
                if (ValidateSim(text)) add();
            }
            else if (simbols[text] == 2)
            {
                if (ValidateBracket(text)) add();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                var val = (double)new Expression(textBox1.Text).Evaluate();
                textBox1.Text = Math.Round(val, 5).ToString();
            }
        }

        private bool ValidateSim(string text)
        {
            var textBoxText = textBox1.Text;
            if (textBoxText == "" || simbols[textBoxText[textBoxText.Length - 1].ToString()] == 1) return false;
            return true;
        }

        private bool ValidateNum(string text)
        {
            var textBoxText = textBox1.Text;
            var lastIndex = textBoxText.Length - 1;
            if (lastIndex > 0 && simbols[textBoxText[textBoxText.Length - 1].ToString()] == 0 && text == ")") return false;
            return true;
        }

        private bool ValidateBracket(string text)
        {
            var textBoxText = textBox1.Text;
            var lastIndex = textBoxText.Length - 1;
            countBrackets = (text == "(") ? ++simbols[text] : --simbols[text];
            if (countBrackets < 0) { countBrackets++; return false; }
            if (lastIndex > 0 && simbols[textBoxText[lastIndex].ToString()] == 0 && text == "(") return false;
            if (lastIndex > 0 && simbols[textBoxText[lastIndex].ToString()] == 1 && text == ")") return false;
            return true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var textBoxText = textBox1.Text;
            var lastIndex = textBoxText.Length - 1;
            if (lastIndex > 0 && simbols[textBoxText[textBoxText.Length - 1].ToString()] == 1) return;
            if (isMinus)
            {
                textBox1.Text = textBoxText.Remove(lastIndex);
                isMinus = false;
            }
            else
            {
                textBox1.Text += "-";
                isMinus = true;
            }
        }
    }
}
