﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//UI 옵션 등을 저장처리
namespace MORT
{

    public partial class Form1
    {
        //Setting 메니져에 저장된 값을 기본 셋팅에 적용함.
        private void SetValueToUIValue()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                skinDarkRadioButton.Checked = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                skinLayerRadioButton.Checked = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                skinOverRadioButton.Checked = true;
            }
            showOcrCheckBox.Checked = MySettingManager.NowIsShowOcrResultFlag;
            saveOCRCheckBox.Checked = MySettingManager.NowIsSaveOcrReulstFlag;
            isClipBoardcheckBox1.Checked = MySettingManager.NowIsSaveInClipboardFlag;

            if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract)
            {
                OCR_Type_comboBox.SelectedIndex = 0;
            }
            else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
            {
                OCR_Type_comboBox.SelectedIndex = 1;
            }
            else
            {
                OCR_Type_comboBox.SelectedIndex = 0;
            }


            TransType_Combobox.SelectedIndex = (int)MySettingManager.NowTransType;

            checkStringUpper.Checked = MySettingManager.IsUseStringUpper;
            checkRGB.Checked = MySettingManager.NowIsUseRGBFlag;
            checkHSV.Checked = MySettingManager.NowIsUseHSVFlag;
            checkErode.Checked = MySettingManager.NowIsUseErodeFlag;

            switch (MySettingManager.NowOCRSpeed)
            {
                case 1:
                    speedRadioButton1.Checked = true;
                    break;

                case 2:
                    speedRadioButton2.Checked = true;
                    break;

                case 3:
                    speedRadioButton3.Checked = true;
                    break;

                case 4:
                    speedRadioButton4.Checked = true;
                    break;

                case 5:
                    speedRadioButton5.Checked = true;
                    break;

                default:
                    speedRadioButton3.Checked = true;
                    break;

            }

            dbFileTextBox.Text = MySettingManager.NowDBFile;
            tessDataTextBox.Text = MySettingManager.NowTessData;
            cbFastTess.Checked = MySettingManager.nowIsFastTess;
            cbDBMultiGet.Checked = MySettingManager.nowIsUsePartialDB;
            dicFileTextBox.Text = MySettingManager.NowDicFile;

            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            cbPerWordDic.Checked = MySettingManager.isUseMatchWordDic;
            setCheckSpellingToolStripMenuItem.Checked = MySettingManager.NowIsUseDicFileFlag;

            //TTS 설정
            cbUseTTS.Checked = MySettingManager.IsUseTTS;
            cbTTSWaitEnd.Checked = MySettingManager.IsWaitTTSEnd;

            //언어 설정.
            if (MySettingManager.NowIsUseEngFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 0;
            }
            else if (MySettingManager.NowIsUseJpnFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 1;
            }
            else if (MySettingManager.NowIsUseOtherLangFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 2;
            }



            SetTransLangugage(MySettingManager.WindowLanguageCode);
            //네이버.
            foreach (var obj in naverTransComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.NaverTransCode == data.naverCode)
                {
                    naverTransComboBox.SelectedItem = obj;
                    break;
                }
            }

            //네이버 번역기
            foreach (var obj in cbNaverResultCode.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if (MySettingManager.NaverResultCode == data.naverCode)
                {
                    cbNaverResultCode.SelectedItem = obj;
                    break;
                }
            }


            //구글.
            foreach (var obj in googleTransComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if (MySettingManager.GoogleTransCode == data.googleCode)
                {
                    googleTransComboBox.SelectedItem = obj;
                    break;
                }
            }

            //구글 번역기.
            foreach (var obj in googleResultCodeComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if (MySettingManager.GoogleResultCode == data.googleCode)
                {
                    googleResultCodeComboBox.SelectedItem = obj;
                    break;
                }
            }



            //윈도우 10 관련.
            if (isAvailableWinOCR)
            {
                //OCR을 찾았나 못 찾았나.
                //영어는 en 또는 en-us일 수 있다
                bool isFound = false;
                for (int i = 0; i < winLanguageCodeList.Count; i++)
                {
                    if(Util.GetIsEqualWinCode(winLanguageCodeList[i], MySettingManager.WindowLanguageCode))
                    {
                        if (WinOCR_Language_comboBox.Items.Count > i)
                        {
                            isFound = true;
                            WinOCR_Language_comboBox.SelectedIndex = i;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if(!isFound && WinOCR_Language_comboBox.Items.Count >0)
                {
                    WinOCR_Language_comboBox.SelectedIndex = 0;
                }
            }

            initColorGroup();

            colorGroup = MySettingManager.NowColorGroup;
            if (colorGroup.Count != 0)
            {
                rTextBox.Text = colorGroup[0].getValueR().ToString();
                gTextBox.Text = colorGroup[0].getValueG().ToString();
                bTextBox.Text = colorGroup[0].getValueB().ToString();

                v1TextBox.Text = colorGroup[0].getValueV1().ToString();
                v2TextBox.Text = colorGroup[0].getValueV2().ToString();
                s1TextBox.Text = colorGroup[0].getValueS1().ToString();
                s2TextBox.Text = colorGroup[0].getValueS2().ToString();

                for (int i = 2; i <= MySettingManager.NowColorGroupCount; i++)
                {
                    groupCombo.Items.Add(i);
                }
            }

            locationXList = MySettingManager.NowLocationXList;
            locationYList = MySettingManager.NowLocationYList;
            sizeXList = MySettingManager.NowSizeXList;
            sizeYList = MySettingManager.NowSizeYList;


            exceptionLocationXList = MySettingManager.nowExceptionLocationXList;
            exceptionLocationYList = MySettingManager.nowExceptionLocationYList;
            exceptionSizeXList = MySettingManager.nowExceptionSizeXList;
            exceptionSizeYList = MySettingManager.nowExceptionSizeYList;

            textFont = MySettingManager.TextFont;
            textColor = MySettingManager.TextColor;
            outlineColor1 = MySettingManager.OutLineColor1;
            outlineColor2 = MySettingManager.OutLineColor2;
            backgroundColor = MySettingManager.BackgroundColor;

            if (MySettingManager.NowSortType == SettingManager.SortType.Center)
                alignmentCenterCheckBox.Checked = true;
            else
                alignmentCenterCheckBox.Checked = false;

            useBackColorCheckBox.Checked = MySettingManager.NowIsUseBackColor;
            removeSpaceCheckBox.Checked = MySettingManager.NowIsRemoveSpace;
            cbShowOCRIndex.Checked = MySettingManager.IsShowOCRIndex;

            //엑티브 윈도우
            activeWinodeCheckBox.Checked = MySettingManager.NowIsActiveWindow;

            //업데이트 확인
            checkUpdateCheckBox.Checked = GetCheckUpdate();

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ");
            bool isDefaultPage = false;
            if(Boolean.TryParse(result, out isDefaultPage))
            {
                cbSetBasicDefaultPage.Checked = isDefaultPage;
            }
            else
            {
                cbSetBasicDefaultPage.Checked = false;
            }

         

            topMostcheckBox.Checked = isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = isTranslateFormTopMostFlag;

            MySettingManager.TextColor = Color.FromArgb(15, 15, 15);
            fontButton.Text = textFont.FontFamily.Name;
            fontSizeUpDown.Value = (int)textFont.Size;
            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            imgZoomsizeUpDown.Value = (decimal)MySettingManager.ImgZoomSize;


            FormManager.Instace.ResetCaputreAreaForm();
        }


        //설정 파일로 저장.
        private void SaveSetting(string fileName)
        {
            MySettingManager.NowTransType = transType;

            //MySettingManager.NowOCRSpeed = (ocrProcessSpeed / 500) - 1;
            MySettingManager.NowColorGroupCount = groupCombo.Items.Count - 2;
            MySettingManager.NowColorGroup = colorGroup;
            MySettingManager.NowOCRGroupcount = locationXList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;

            //제외 영역
            MySettingManager.nowExceptionGroupCount = exceptionLocationXList.Count;
            MySettingManager.nowExceptionLocationXList = exceptionLocationXList;
            MySettingManager.nowExceptionLocationYList = exceptionLocationYList;
            MySettingManager.nowExceptionSizeXList = exceptionSizeXList;
            MySettingManager.nowExceptionSizeYList = exceptionSizeYList;

            //번역창 위치 설정 - 디폴트는 모두 없애고 초기화 땐 저장을 안 한다.
            if (eCurrentState == eCurrentStateType.SetDefault)
            {
                MySettingManager.transFormLocationX = -1;
                MySettingManager.transFormLocationY = -1;

                MySettingManager.transFormSizeX = -1;
                MySettingManager.transFormSizeY = -1;
            }
            else if(eCurrentState != eCurrentStateType.Init)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;
                    MySettingManager.transFormLocationX = transForm.Location.X;
                    MySettingManager.transFormLocationY = transForm.Location.Y;

                    MySettingManager.transFormSizeX = transForm.Size.Width;
                    MySettingManager.transFormSizeY = transForm.Size.Height;

                }

            }


            MySettingManager.saveSetting(fileName);

        }



        //UI 값을 설정 매니저에 저장 -> 실제로 적용
        public void ApplyUIValueToSetting()
        {
            try
            {
                ChangeSkin();
                MySettingManager.NowIsShowOcrResultFlag = showOcrCheckBox.Checked;
                MySettingManager.NowIsSaveOcrReulstFlag = saveOCRCheckBox.Checked;
                IsUseClipBoardFlag = isClipBoardcheckBox1.Checked;

                transType = (SettingManager.TransType)TransType_Combobox.SelectedIndex;

                MySettingManager.IsUseStringUpper = checkStringUpper.Checked;
                MySettingManager.NowIsUseRGBFlag = checkRGB.Checked;
                MySettingManager.NowIsUseHSVFlag = checkHSV.Checked;
                MySettingManager.NowIsUseErodeFlag = checkErode.Checked;

                if (speedRadioButton1.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 1;
                    ocrProcessSpeed = 300;
                }
                else if (speedRadioButton2.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 2;
                    ocrProcessSpeed = 1000;
                }
                else if (speedRadioButton3.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 3;
                    ocrProcessSpeed = 1500;
                }
                else if (speedRadioButton4.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 4;
                    ocrProcessSpeed = 2000;
                }
                else if (speedRadioButton5.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 5;
                    ocrProcessSpeed = 2500;
                }

                MySettingManager.NowDBFile = dbFileTextBox.Text;
                MySettingManager.NowTessData = tessDataTextBox.Text;
                MySettingManager.nowIsUsePartialDB = cbDBMultiGet.Checked;
                MySettingManager.nowIsFastTess = cbFastTess.Checked;

                SaveHotKeyFile();

                SetCheckUpdate(checkUpdateCheckBox.Checked);

                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ", cbSetBasicDefaultPage.Checked.ToString());

                //OCR 설정.
                MySettingManager.OCRType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedIndex);

                //번역 코드 설정.
                TransManager.TransCodeData codeData = (TransManager.TransCodeData)((ComboboxItem)naverTransComboBox.SelectedItem).Value;
                MySettingManager.NaverTransCode = codeData.naverCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)cbNaverResultCode.SelectedItem).Value;
                MySettingManager.NaverResultCode = codeData.naverCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)googleTransComboBox.SelectedItem).Value;
                MySettingManager.GoogleTransCode = codeData.googleCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)googleResultCodeComboBox.SelectedItem).Value;
                MySettingManager.GoogleResultCode = codeData.googleCode;


                NaverTranslateAPI.instance.SetTransCode(MySettingManager.NaverTransCode, MySettingManager.NaverResultCode);
                GoogleBasicTranslateAPI.instance.SetTransCode(MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);

                //윈도우 10 OCR 관련.
                if (isAvailableWinOCR && winLanguageCodeList.Count > WinOCR_Language_comboBox.SelectedIndex)
                {
                    MySettingManager.WindowLanguageCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    loader.InitOCR(MySettingManager.WindowLanguageCode);
                }
                else
                {
                    MySettingManager.WindowLanguageCode = "";
                }

                //TTS 설정
                MySettingManager.IsUseTTS = cbUseTTS.Checked;
                MySettingManager.IsWaitTTSEnd = cbTTSWaitEnd.Checked;

                //언어 설정.
                MySettingManager.NowIsUseEngFlag = false;
                MySettingManager.NowIsUseJpnFlag = false;
                MySettingManager.NowIsUseOtherLangFlag = false;

                if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract || MySettingManager.OCRType == SettingManager.OcrType.NHocr)
                {
                    if (tesseractLanguageComboBox.SelectedIndex == 0)
                    {
                        //영어.
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if (tesseractLanguageComboBox.SelectedIndex == 1)
                    {
                        //일본어
                        MySettingManager.NowIsUseJpnFlag = true;

                    }
                    else if (tesseractLanguageComboBox.SelectedIndex == 2)
                    {
                        //기타
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.Window && isAvailableWinOCR)
                {
                    string selectCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    if (selectCode == "en" || selectCode == "en-US")
                    {
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if (selectCode == "ja")
                    {
                        MySettingManager.NowIsUseJpnFlag = true;
                    }
                    else
                    {
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }

                //폰트 관련
                textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
                MySettingManager.TextFont = textFont;
                MySettingManager.TextColor = textColor;
                MySettingManager.OutLineColor1 = outlineColor1;
                MySettingManager.OutLineColor2 = outlineColor2;
                MySettingManager.BackgroundColor = backgroundColor;

                MySettingManager.NowIsUseBackColor = this.useBackColorCheckBox.Checked;
                MySettingManager.IsShowOCRIndex = this.cbShowOCRIndex.Checked;

                if (this.alignmentCenterCheckBox.Checked)
                    MySettingManager.NowSortType = SettingManager.SortType.Center;
                else
                    MySettingManager.NowSortType = SettingManager.SortType.Normal;
                MySettingManager.NowIsRemoveSpace = this.removeSpaceCheckBox.Checked;

                MySettingManager.NowIsActiveWindow = activeWinodeCheckBox.Checked;


                //번역창 최상단
                isTranslateFormTopMostFlag = topMostcheckBox.Checked;
                setTranslateTopMostToolStripMenuItem.Checked = topMostcheckBox.Checked;

                //Util.ShowLog("Bing : " + transCode.ToString() + " Naver : " + MySettingManager.NaverTransCode);
                if (MySettingManager.NowSkin == SettingManager.Skin.dark)
                {
                    //더이상 이곳에서 번역키 설정 안 함
                }
                else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                {
                    FormManager.Instace.MyLayerTransForm.UpdateTransform();
                }
                else if (MySettingManager.NowSkin == SettingManager.Skin.over)
                {
                    FormManager.Instace.MyOverTransForm.UpdateTransform();
                }

                ApplyTransSetting();

                MySettingManager.ImgZoomSize = (float)imgZoomsizeUpDown.Value;
               

            }
            catch (Exception e)
            {
                Util.ShowLog(e.Message);
                MessageBox.Show(e.Message);
            }

            //색 리스트
            int[] valueRArray = new int[groupCombo.Items.Count];
            int[] valueGArray = new int[groupCombo.Items.Count];
            int[] valueBArray = new int[groupCombo.Items.Count];

            int[] valueS1Array = new int[groupCombo.Items.Count];
            int[] valueS2Array = new int[groupCombo.Items.Count];
            int[] valueV1Array = new int[groupCombo.Items.Count];
            int[] valueV2Array = new int[groupCombo.Items.Count];

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));
            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));

            //교정사전.
            MySettingManager.NowIsUseDicFileFlag = checkDic.Checked;
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            MySettingManager.isUseMatchWordDic = cbPerWordDic.Checked;
            for (int i = 0; i < colorGroup.Count; i++)
            {
                colorGroup[i].checkHSVRange();
                valueRArray[i] = colorGroup[i].getValueR();
                valueGArray[i] = colorGroup[i].getValueG();
                valueBArray[i] = colorGroup[i].getValueB();

                valueS1Array[i] = colorGroup[i].getValueS1();
                valueS2Array[i] = colorGroup[i].getValueS2();
                valueV1Array[i] = colorGroup[i].getValueV1();
                valueV2Array[i] = colorGroup[i].getValueV2();
            }


            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();  //색 그룹 개수 표시

            try
            {
                SetIsUseNHocr(false);

                if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract)
                {
                    bool isUseUnicode = false;
                    if (MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseUnicode = true;
                    }

                    if (MySettingManager.NowIsUseOtherLangFlag)
                    {
                        isUseUnicode = true;
                    }

                    string tessData = MySettingManager.NowTessData;
                    if (MySettingManager.nowIsFastTess)
                    {
                        if (tessData == "eng" || tessData == "jpn")
                        {
                            tessData = tessData + "_fast";
                        }
                    }

                    setTessdata(tessData, isUseUnicode);
                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
                {
                    bool isUseJpn = false;

                    if (MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseJpn = true;
                    }

                    SetIsUseJpn(isUseJpn);

                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.NHocr)
                {
                    SetIsUseNHocr(true);
                }


                setFiducialValue(valueRArray, valueGArray, valueBArray, valueS1Array, valueS2Array, valueV1Array, valueV2Array, groupCombo.Items.Count - 2);

                //교정사전 관련

                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile);


                bool isUseDBFlag = false;
                if (transType == SettingManager.TransType.db)
                {
                    isUseDBFlag = true;
                }
                SetRemoveSpace(MySettingManager.NowIsRemoveSpace);
                SetShowOCRIndex(MySettingManager.IsShowOCRIndex);
                SetIsStringUpper(MySettingManager.IsUseStringUpper);
            
                setUseDB(isUseDBFlag,MySettingManager.nowIsUsePartialDB,  MySettingManager.NowDBFile);
                setAdvencedImgOption(MySettingManager.NowIsUseRGBFlag, MySettingManager.NowIsUseHSVFlag, MySettingManager.NowIsUseErodeFlag, MySettingManager.ImgZoomSize);
                SetCaptureArea();
                SetIsActiveWindow(MySettingManager.NowIsActiveWindow);


                if(MySettingManager.isDebugMode)
                {
                    SetIsDebugMode(MySettingManager.isDebugMode, cbShowReplace.Checked, cbSaveCapture.Checked, cbSaveCaptureResult.Checked);
                }

            }
            catch (Exception e)
            {
                Util.ShowLog(e.Message);
                MessageBox.Show(e.Message);
            }
        }

        private void ApplyTransSetting()
        {
            yandexAccountTextBox.Text = yandexAccountTextBox.Text.Replace(" ", "");
            yandexKey = yandexAccountTextBox.Text;

            string naverApiType = MORT.NaverTranslateAPI.API_NMT;
            naverApiType = MORT.NaverTranslateAPI.API_NMT;

            MySettingManager.NaverApiType = naverApiType;

            //공백 제거
            NaverIDKeyTextBox.Text = NaverIDKeyTextBox.Text.Replace(" ", "");
            NaverSecretKeyTextBox.Text = NaverSecretKeyTextBox.Text.Replace(" ", "");
            naverIDKey = NaverIDKeyTextBox.Text;
            naverSecretKey = NaverSecretKeyTextBox.Text;

            bool isPaid = false;

            var data = TransManager.Instace.GetNaverKey();

            //번역기 초기화
            NaverTranslateAPI.instance.Init(naverIDKey, naverSecretKey, naverApiType, data.isPaid);
            YandexAPI.instance.Init(yandexKey);

            //구글 토큰 성공 여부.
            SettingManager.isErrorEmptyGoogleToken = false;
            if (transType == SettingManager.TransType.google)
            {
                //구글 시트 처리
                string sheet = googleSheet_textBox.Text.Replace(" ", "");
                if(sheet.Contains("/d/"))
                {
                    int startIndex = sheet.LastIndexOf("/d/");
                    if(startIndex >=0)
                    {
                        sheet = sheet.Remove(0, startIndex + 3);
                    }                 
                }

                if(sheet.Contains("/edit"))
                {
                    int startIndex = sheet.LastIndexOf("/edit");
                    if (startIndex >= 0)
                    {
                        sheet = sheet.Remove(startIndex);
                    }
                }


                googleSheet_textBox.Text = sheet;
                textBox_GoogleClientID.Text = textBox_GoogleClientID.Text.Replace(" ", "");
                textBox_GoogleSecretKey.Text = textBox_GoogleSecretKey.Text.Replace(" ", "");
                Logo.SetTopmost(false);
                TransManager.Instace.InitGtrans(googleSheet_textBox.Text, textBox_GoogleClientID.Text, textBox_GoogleSecretKey.Text, MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);
            }
            else if(transType == SettingManager.TransType.ezTrans)
            {
                TransManager.Instace.InitEzTrans();
            }

            SaveNaverKeyFile();
            SaveGoogleKeyFile();

        }


        public void ApplyFromQuickSetting(QuickSettingData data)
        {
            //1. 번역이 진행중이면 먼저 중단한다
            //2. SettingData를 바꾼다
            //3. ui 값을 바꾼다
            //4. 적용한다

            //바꿀사항
            //-OCR 타입
            //-번역기 타입
            //-이미지 보정 사용 여부 / HSV 값
            //활성화 된 윈도우에서 추출 -> 강제 비활성화
            //번역창 -> 강제 레이어 번역창
            //OCR 속도 -> 가장 빠름
            StopTrans();

            //OCR 타입
            MySettingManager.OCRType = data.ocrType;

            //언어 코드
            if(data.languageType == QuickSettingData.LanguageType.Japen)
            {
                MySettingManager.NowTessData = "jpn";
                MySettingManager.NowIsRemoveSpace = true;
                MySettingManager.NowIsUseJpnFlag = true;
            }
            else
            {
                MySettingManager.NowTessData = "eng";
                MySettingManager.NowIsRemoveSpace = false;
                MySettingManager.NowIsUseJpnFlag = false;
            }
            MySettingManager.NowIsUseOtherLangFlag = false;
            MySettingManager.nowIsFastTess = false;
            MySettingManager.WindowLanguageCode = data.LanguageCode;

            //번역기 타입
            MySettingManager.NowTransType = data.transType;
            if (data.languageType == QuickSettingData.LanguageType.Japen)
            {
                MySettingManager.GoogleTransCode = "ja";
                MySettingManager.NaverTransCode = "ja";
            }
            else
            {
                MySettingManager.GoogleTransCode = "en";
                MySettingManager.NaverTransCode = "en";
            }

            MySettingManager.GoogleResultCode = "ko";
            MySettingManager.NaverResultCode = "ko";

            //색 보정
            MySettingManager.NowIsUseRGBFlag = false;
            MySettingManager.NowIsUseHSVFlag = false;
            MySettingManager.NowIsUseErodeFlag = false;


            if (data.HsvList.Count > 0)
            {
                MySettingManager.NowIsUseHSVFlag = true;

                List<ColorGroup> list = new List<ColorGroup>();
                MySettingManager.NowColorGroupCount = data.HsvList.Count;

                for(int i = 0; i < data.HsvList.Count; i++)
                {
                    var color = new ColorGroup();
                    color.SetHSV(data.HsvList[i]);

                    list.Add(color);
                }

                MySettingManager.NowColorGroup = list;
            }

            //활성화 된 윈도우에서 추출
            MySettingManager.NowIsActiveWindow = false;

            //번역창
            MySettingManager.NowSkin = SettingManager.Skin.layer;

            //OCR 속도
            MySettingManager.NowOCRSpeed = 1;


            SetValueToUIValue();
            ApplyUIValueToSetting();

            SaveSetting(GlobalDefine.USER_SETTING_FILE);
        }

        public void ApplyAdvencedOption()
        {
            bool isTrans = false;
            if (thread != null && thread.IsAlive == true)
            {
                isTrans = true;
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;             
            }


            //고급 설정값을 적용한다



            if(isTrans)
            {
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
        }

    }



}
