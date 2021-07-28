﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public class HotKeyData
    {
        public int index;
        public KeyInputLabel.KeyType keyType;
        public List<Keys> keyList = new List<Keys>();
        public string extraData = "";
        public string keyResult = "";

        public HotKeyData()
        {

        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, List<Keys> keyList, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();

            foreach(var obj in keyList)
            {
                this.keyList.Add(obj);
            }

            this.extraData = extraData;
            this.keyResult = keyResult;
        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();
            this.keyResult = keyResult;

            char[] token = new char[] { ' ' };
            string[] keys = keyResult.Split(token, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Keys test = (Keys)System.Enum.Parse(typeof(Keys), keys[i]);
                    keyList.Add(test);
                }

            }
            catch
            {
                keyList.Clear();
                this.keyResult = "";
            }
          

            this.extraData = extraData;
           
        }

    }

    public class AdvencedOptionManager
    {
        public const string TEMP_USER_TRANSLATION_DB_FILE = @"TEMP_USER_TRANSLATION.txt";

        public const int OPEN_SETTING_MAX = 4;
        public const string KEY_HOTKEY_OPEN_SETTING = "@HOTKEY_OPEN_SETTING_{0} ";
        public const string KEY_HOTKEY_FILE_PATH = "@HOTKEY_OPEN_FILE_PATH_{0} ";

        public const string KEY_FONT_AUTO_SIZE = "@OVERLAY_FONT_AUTO_SIZE ";
        public const string KEY_FONT_AUTO_MIN_SIZE = "@OVERLAY_FONT_AUTO_MIN_SIZE ";
        public const string KEY_FONT_AUTO_MAX_SIZE = "@OVERLAY_FONT_AUTO_MAX_SIZE ";

        public const string KEY_TRANSLATION_LIST = "@TRANSLATION_FILES ";   // <>로 해야한다
        public const string KEY_TRANSLATION_SEARCH_TYPE = "@TRANSLATION_SEARCH_TYPE ";   //번역집 검색 방식
        public class Data
        {
            //고급 단축키
            public List<HotKeyData> hotKeyList = new List<HotKeyData>();

            //번역창
            public bool isUseAutoSizeFont = false;
            public int minAutoSizeFont = 10;
            public int maxAutoSizeFont = 50;

            //번역집
            public List<string> translationFileList = new List<string>();
            public bool isTranslationDbStyle = false;
        }

        public static Data data = new Data();


        public static List<string> TranslationFileList
        {
            set { data.translationFileList = value; }
            get { return data.translationFileList; }
        }

        public static bool IsTranslationDbStyle
        {
            set { data.isTranslationDbStyle = value; }
            get { return data.isTranslationDbStyle; }
        }

        public static bool IsAutoFontSize
        {
            get { return data.isUseAutoSizeFont; }
        }

        public static int MinAutoFontSize
        {
            get { return data.minAutoSizeFont; }
        }

        public static int MaxAutoFontSize
        {
            get { return data.maxAutoSizeFont; }
        }

        public static float GetResultAutoFontSize(float fontSize)
        {
            float result = fontSize;

            if(fontSize > data.maxAutoSizeFont)
            {
                result = data.maxAutoSizeFont;
            }
            else if(fontSize < data.minAutoSizeFont)
            {
                result = data.minAutoSizeFont;
            }


            return result;
        }

        public static void SetOverLay(bool isAutoSize, int minSize, int maxSize)
        {
            data.isUseAutoSizeFont = isAutoSize;
            data.minAutoSizeFont = minSize;
            data.maxAutoSizeFont = maxSize;

        }


        public static void SetHotKey(List<HotKeyData> list)
        {
            data.hotKeyList.Clear();

            foreach(var obj in list)
            {
                data.hotKeyList.Add(obj);
            }
        }

        public static List<HotKeyData> GetHotKeyList()
        {
            return data.hotKeyList;
        }

        public static HotKeyData GetHotKeyResult(List<Keys> inputList)
        {
            HotKeyData result = null;

            for(int i = 0; i < data.hotKeyList.Count; i++)
            {
                if(KeyInputLabel.GetResult(inputList, data.hotKeyList[i].keyList))
                {
                    result = data.hotKeyList[i];
                    break;
                }
            }


            return result;
        }


        //파일에서 초기화 -> 한 번만 한다.
        public static void Init()
        {
            using (StreamReader r = Util.OpenFile(GlobalDefine.ADVENCED_SETTING_FILE))
            {
                if (r != null)
                {
                    string fileData = r.ReadToEnd();
                    if(fileData != "")
                    {
                        //설정파일 단축키 가져오기
                        for (int i = 0; i < OPEN_SETTING_MAX; i++)
                        {
                            string key = string.Format(KEY_HOTKEY_OPEN_SETTING, i.ToString());
                            string result = Util.GetNextLine(fileData, key);


                            string fileKey = string.Format(KEY_HOTKEY_FILE_PATH, i.ToString());
                            string fileResult = Util.GetNextLine(fileData, fileKey);


                            HotKeyData hotKeyData = new HotKeyData(i, KeyInputLabel.KeyType.OpenSetting, result, fileResult);
                            data.hotKeyList.Add(hotKeyData);

                        }

                        data.isUseAutoSizeFont = Util.ParseBool(fileData, KEY_FONT_AUTO_SIZE);
                        data.minAutoSizeFont = Util.ParseInt(fileData, KEY_FONT_AUTO_MIN_SIZE);
                        data.maxAutoSizeFont = Util.ParseInt(fileData, KEY_FONT_AUTO_MAX_SIZE);

                        //번역집
                        LoadTranslationFileList(fileData);


                    }
                    
                }

                r.Close();
            }

            if(!Directory.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH))
            {
                Directory.CreateDirectory(GlobalDefine.ADVENCED_TRANSRATION_PATH);
            }
        }

        private static void LoadTranslationFileList(string fileData)
        {
            string sep = "\t";
            string content = Util.ParseString(fileData, KEY_TRANSLATION_LIST, '<', '>');
            string[] keys = content.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            data.translationFileList.Clear();
            foreach (var obj in keys)
            {
                //파일이 존재하는 것 만 넣는다.
                if(File.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt"))
                {
                    data.translationFileList.Add(obj);
                }
            }

            data.isTranslationDbStyle = Util.ParseBool(fileData, KEY_TRANSLATION_SEARCH_TYPE);

        }

        public static void Reset()
        {
            data.hotKeyList.Clear();
            data.isUseAutoSizeFont =false;
            data.minAutoSizeFont = 10;
            data.maxAutoSizeFont = 50;

            data.translationFileList.Clear();
            data.isTranslationDbStyle = false;

        }

        public static void Save()
        {
            string result = "";


            for (int i = 0; i < data.hotKeyList.Count; i++)
            {
                if (data.hotKeyList[i].keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    string key = string.Format(KEY_HOTKEY_OPEN_SETTING, i.ToString());
                    string keyResult = data.hotKeyList[i].keyResult;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                    key = string.Format(KEY_HOTKEY_FILE_PATH, i.ToString());
                    keyResult = data.hotKeyList[i].extraData;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                }

            }


            result += KEY_FONT_AUTO_SIZE + '[' + data.isUseAutoSizeFont.ToString() + "]" + System.Environment.NewLine;
            result += KEY_FONT_AUTO_MIN_SIZE + '[' + data.minAutoSizeFont.ToString() + "]" + System.Environment.NewLine;
            result += KEY_FONT_AUTO_MAX_SIZE + '[' + data.maxAutoSizeFont.ToString() + "]" + System.Environment.NewLine;


            if(data.translationFileList.Count > 0)
            {
                string files = KEY_TRANSLATION_LIST + "<";

                for (int i = 0; i < data.translationFileList.Count; i++)
                {
                    files += data.translationFileList[i];

                    if(i+1 != data.translationFileList.Count)
                    {
                        files += "\t";
                    }
                }

                files += ">";

                result += files;

            }

            result += KEY_TRANSLATION_SEARCH_TYPE + '[' + data.isTranslationDbStyle.ToString() + ']' + System.Environment.NewLine;

            Util.SaveFile(GlobalDefine.ADVENCED_SETTING_FILE, result);
        }




    }

}
