﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MORT
{
    public partial class KeyInputLabel : UserControl
    {
        public bool isFocus;
        public List<Keys> keyList = new List<Keys>();
        public List<Keys> backupList = new List<Keys>();
        public KeyInputLabel()
        {
            InitializeComponent();
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                code = Keys.ShiftKey;
            }
            else if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                code = Keys.ControlKey;
            }
            else if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            {
                code = Keys.Menu;
            }

            if(e.KeyCode == Keys.Escape)
            {
                SetEmpty();
            }
            else if(e.KeyCode == Keys.Back)
            {
                SetEmpty();
            }
            else
            {
                if(backupList.Count >=3)
                {
                    return;
                }
                for(int i = 0 ; i < backupList.Count; i++)
                {
                    if (backupList[i] == code)
                    {
                        return;
                    }
                }
               
                if (backupList.Count != 0)
                {
                    textBox1.Text += '+';
                }
                backupList.Add(code);
                textBox1.Text += code;

            }
            
        }

        //적용과 동시에 스트잉으로 변환함.
        public string GetKeyListToString()
        {
            string result = "";

            keyList.Clear();
            for (int i = 0; i < backupList.Count; i++)
            {
                keyList.Add(backupList[i]);
                result += keyList[i];
                result += " ";
            }

                return result;
        }

        //파일을 불러와서 적용함.
        public void SetKeyList(string data)
        {
            char[] token = new char[] { ' ' };
            string[] keys = data.Split(token, StringSplitOptions.RemoveEmptyEntries);
            textBox1.Text = "";
            keyList.Clear();
            backupList.Clear();
            for (int i = 0; i < keys.Length; i++ )
            {
                Keys test = (Keys)System.Enum.Parse(typeof(Keys), keys[i]);
                keyList.Add(test);
                backupList.Add(test);
                if (keyList.Count != 1)
                {
                    textBox1.Text += "+";
                }
                textBox1.Text += test;                
            }
        }

        public bool GetIsCorrect(List<Keys> list)
        {
            bool isCorrect = false;            

            if(list.Count == keyList.Count && keyList.Count > 0)
            {
                isCorrect = true;
                for (int i = 0; i < list.Count; i++ )
                {
                    bool isHave = false;
                    for(int j = 0; j < keyList.Count; j++)
                    {
                        if(list[i] == keyList[j])
                        {
                            isHave = true;
                            break;
                        }
                    }

                    if(!isHave)
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }

            return isCorrect;
        }

        public void ResetInput(List<Keys> list)
        {
            isFocus = false;
            this.keyList.Clear();
            this.backupList.Clear();
            textBox1.Text = "";
            for (int i = 0; i < list.Count; i++ )
            {
                if (keyList.Count != 0)
                {
                    textBox1.Text += '+';
                }
                keyList.Add(list[i]);
                backupList.Add(list[i]);
                textBox1.Text += list[i];
            }

        }

        public void SetEmpty()
        {
            textBox1.Text = "";
            backupList.Clear();
        }

        public void SetText(string text)
        {
            textBox1.Text = text;
        }

        private void KeyInputLabel_Enter(object sender, EventArgs e)
        {
            isFocus = true;
        }

        private void KeyInputLabel_Leave(object sender, EventArgs e)
        {
            isFocus = false;
        }
    }
}
