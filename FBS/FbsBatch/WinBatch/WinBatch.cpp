// WinBatch.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "WinBatch.h"

#define MAX_LOADSTRING 100
#define _BUILD_SHORT 1

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	SettingsDlgProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	CertificateImportDlgProc(HWND, UINT, WPARAM, LPARAM);
#ifndef _BUILD_SHORT
INT_PTR CALLBACK	EntrantImportDlgProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	InputEntrantDlgProc(HWND, UINT, WPARAM, LPARAM);
#endif

INT_PTR CALLBACK	InputCertificateDlgProc(HWND, UINT, WPARAM, LPARAM);

char* subjectName[] = {"Русский язык", "Математика", "Физика", "Химия", "Биология", "История  России", 
					   "Обществознание", "Литература", "География", "Английский язык", "Французский язык",
					   "Немецкий язык", "Испанский язык", "Информатика"};

bool				MarkValidate(HWND, int, WPARAM, LPARAM);	// проверка EditBox-ов оценок

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_WINBATCH, szWindowClass, MAX_LOADSTRING);

#ifdef _BUILD_SHORT
	memset(szTitle, 0, sizeof(szTitle));
	memcpy(szTitle, "Генератор файлов для пакетной обработки", 
		sizeof("Генератор файлов для пакетной обработки"));
#endif

	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WINBATCH));

	if(!CSettings::LoadSettings()) return FALSE;

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WINBATCH));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW-1);
#ifndef _BUILD_SHORT
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_WINBATCH);
#else
	wcex.lpszMenuName = MAKEINTRESOURCE(IDC_WINBATCH_SHORT);
#endif
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, SW_SHOWMAXIMIZED);
   UpdateWindow(hWnd);

   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:

		switch (wmId)
		{
#ifndef _BUILD_SHORT
		case ID_FILE_INPUTENTRANT:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG5), hWnd, InputEntrantDlgProc);
			} break;
#endif
		case ID_FILE_INPUTCERTIFICATE:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG4), hWnd, InputCertificateDlgProc);
			} break;
		case ID_FILE_IMPORTCERTIFICATE:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG2), hWnd, CertificateImportDlgProc);
			} break;
#ifndef _BUILD_SHORT
		case ID_FILE_ENTRANTIMPORT:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG3), hWnd, EntrantImportDlgProc);
			} break;
#endif
		case ID_SETTINGS_WORKDIR:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG1), hWnd, SettingsDlgProc);
			} break;
		case ID_ABOUT_PROGRAMM:
			{
#ifndef _BUILD_SHORT
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG6), hWnd, About);
#else
				DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG7), hWnd, About);
#endif
			} break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

//
INT_PTR CALLBACK SettingsDlgProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	HWND hwnd;
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		SetDlgItemText(hDlg, IDC_EDIT1, CSettings::GetPath());
		hwnd = GetDlgItem(hDlg, IDC_STATIC2);
		SetWindowText(hwnd, CSettings::GetPath());
		return (INT_PTR)TRUE;
	case WM_COMMAND:
		if (LOWORD(wParam) == IDBROWSE)
		{
			BROWSEINFO bi = {
				hDlg,
				NULL,
				CSettings::GetPath(),
				"Выбор рабочей дирректории:",
				BIF_DONTGOBELOWDOMAIN|BIF_RETURNONLYFSDIRS|BIF_EDITBOX|BIF_BROWSEFORCOMPUTER,
				NULL,
				NULL,
				0
			};

			char szDir[256] = { 0 };
			LPCITEMIDLIST lpItemDList = NULL;

			lpItemDList = SHBrowseForFolder(&bi);
			if(NULL != lpItemDList)
			{
				SHGetPathFromIDList(lpItemDList, szDir);
				SetDlgItemText(hDlg, IDC_EDIT1, szDir);
			}
		}
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if(LOWORD(wParam) == IDOK)
			{
				char szDir[256] = { 0 };
				GetDlgItemText(hDlg, IDC_EDIT1, szDir, sizeof(szDir));
				CSettings::SetPath(szDir);
			}
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

INT_PTR CALLBACK CertificateImportDlgProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		{
			char szFilePath[256] = { 0 };
			HWND hwnd;
			sprintf(szFilePath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());
			hwnd = GetDlgItem(hDlg, IDC_STATIC3);
			SetWindowText(hwnd, szFilePath);
			return (INT_PTR)TRUE;
		}
	case WM_COMMAND:
		if (LOWORD(wParam) == IDBROWSE)
		{
			OPENFILENAME ofn;

			memset(&ofn, 0, sizeof(OPENFILENAME));
			
			char szFile[256] = { 0 };
			char szFileTitle[256] = { 0 };
			char szFilter[256] = { 0 };

			szFile[0] = '\0';

			ofn.Flags = OFN_PATHMUSTEXIST|OFN_FILEMUSTEXIST|OFN_EXPLORER;
			ofn.hInstance = hInst;

			ofn.lStructSize       = sizeof(OPENFILENAME);
			ofn.hwndOwner         = hDlg;
			ofn.lpstrFilter       = "Файлы импорта\0*.csv\0Все файлы\0*.*\0";
			ofn.nFilterIndex      = 1;
			ofn.lpstrFile         = szFile;
			ofn.nMaxFile          = sizeof(szFile);
			ofn.lpstrFileTitle    = NULL;
			ofn.nMaxFileTitle     = 0;
			ofn.lpstrInitialDir   = CSettings::GetPath();
	
			GetOpenFileName(&ofn);
			
			SetDlgItemText(hDlg, IDC_EDIT1, ofn.lpstrFile);
		}
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if(LOWORD(wParam) == IDOK)
			{
				char szFileImport[256] = { 0 };
				GetDlgItemText(hDlg, IDC_EDIT1, szFileImport, sizeof(szFileImport));
				if(!CBatchBuilder::ImportCertificates(szFileImport))
				{
					MessageBox(hDlg, "Не удалось импортировать данные - файл имеет неверный формат или некорректные записи",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
				}
			}
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

#ifndef _BUILD_SHORT 
INT_PTR CALLBACK EntrantImportDlgProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		{
			char szFilePath[256] = { 0 };
			HWND hwnd;
			sprintf(szFilePath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());
			hwnd = GetDlgItem(hDlg, IDC_STATIC3);
			SetWindowText(hwnd, szFilePath);
			return (INT_PTR)TRUE;
		}
	case WM_COMMAND:
		if (LOWORD(wParam) == IDBROWSE)
		{
			OPENFILENAME ofn;

			memset(&ofn, 0, sizeof(OPENFILENAME));
			
			char szFile[256] = { 0 };
			char szFileTitle[256] = { 0 };
			char szFilter[256] = { 0 };

			szFile[0] = '\0';

			ofn.Flags = OFN_PATHMUSTEXIST|OFN_FILEMUSTEXIST|OFN_EXPLORER;
			ofn.hInstance = hInst;

			ofn.lStructSize       = sizeof(OPENFILENAME);
			ofn.hwndOwner         = hDlg;
			ofn.lpstrFilter       = "Файлы импорта\0*.csv\0Все файлы\0*.*\0";
			ofn.nFilterIndex      = 1;
			ofn.lpstrFile         = szFile;
			ofn.nMaxFile          = sizeof(szFile);
			ofn.lpstrFileTitle    = NULL;
			ofn.nMaxFileTitle     = 0;
			ofn.lpstrInitialDir   = CSettings::GetPath();
	
			GetOpenFileName(&ofn);
			
			SetDlgItemText(hDlg, IDC_EDIT1, ofn.lpstrFile);
		}
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if(LOWORD(wParam) == IDOK)
			{
				char szFileImport[256] = { 0 };
				GetDlgItemText(hDlg, IDC_EDIT1, szFileImport, sizeof(szFileImport));
				if(!CBatchBuilder::ImportEntrants(szFileImport))
				{
					MessageBox(hDlg, "Не удалось импортировать данные - файл имеет неверный формат или некорректные записи",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
				}
			}
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
#endif

INT_PTR CALLBACK InputCertificateDlgProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		{
			char szFilePath[256] = { 0 };
			HWND hwnd;
			sprintf(szFilePath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());
			hwnd = GetDlgItem(hDlg, IDC_STATIC4);
			SetWindowText(hwnd, szFilePath);
			return (INT_PTR)TRUE;
		}
	case WM_COMMAND:
		// раскоментарить, если понадобится проверка непосредственно при заполнении: 
		//for(int i = 0; i < 14; i++) 
		//{ 
		//	if(!MarkValidate(hDlg, IDC_EDIT5 + i, wParam, lParam)) return 1;
		//}
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if(LOWORD(wParam) == IDOK)
			{
				char szBuff[1024] = { 0 };
				BOOL bTranslated = TRUE; 

				int subjects[14] = { 0 };
				int startCtl = IDC_EDIT5;
				for(int i = 0; i < 14; i++)
				{
					subjects[i] = GetDlgItemInt(hDlg, startCtl + i, &bTranslated, FALSE);
					if(!bTranslated) subjects[i] = -1;
					if(subjects[i] > 100) 
					{
						char szError[256] = { 0 };
						sprintf(szError, "Максимальный балл по предмету \"%s\" не может превышать 100", subjectName[i]);
						MessageBox(hDlg, szError,
							"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
						PostMessage(hDlg, WM_NEXTDLGCTL, (WPARAM)GetDlgItem(hDlg, startCtl + i), 1);
						return 1;
					}
				}

				char szFirstName[256] = { 0 };
				char szLastName[256] = { 0 };
				char szPatronymicName[256] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT1, szLastName, sizeof(szLastName));
				GetDlgItemText(hDlg, IDC_EDIT2, szFirstName, sizeof(szFirstName));
				GetDlgItemText(hDlg, IDC_EDIT3, szPatronymicName, sizeof(szPatronymicName));

				if(strlen(szFirstName) == 0)
				{
					MessageBox(hDlg, "Не заполнено поле имени абитуриента",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}
				if(strlen(szLastName) == 0)
				{
					MessageBox(hDlg, "Не заполнено поле фамилии абитуриента",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}

				UINT uOriginal = IsDlgButtonChecked(hDlg, IDC_CHECK1);

				char szNumber[32] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT4, szNumber, sizeof(szNumber));
				if(!CBatchBuilder::IsNumberCorrect(szNumber))
				{
					MessageBox(hDlg, "Введённый номер сертификата не соответствует формату xx-xxxxxxxxx-xx.",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}

				char szNum[14][32];

				memset(szNum[0], 0, sizeof(szNum[0])*14);
				
				sprintf(szBuff, "%s%%%u%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s",
					szNumber, uOriginal, szLastName, szFirstName, szPatronymicName,
					(subjects[0] < 0) ? "" : _itoa(subjects[0], szNum[0], 10),
					(subjects[1] < 0) ? "" : _itoa(subjects[1], szNum[1], 10),
					(subjects[2] < 0) ? "" : _itoa(subjects[2], szNum[2], 10),
					(subjects[3] < 0) ? "" : _itoa(subjects[3], szNum[3], 10),
					(subjects[4] < 0) ? "" : _itoa(subjects[4], szNum[4], 10),
					(subjects[5] < 0) ? "" : _itoa(subjects[5], szNum[5], 10),
					(subjects[6] < 0) ? "" : _itoa(subjects[6], szNum[6], 10),
					(subjects[7] < 0) ? "" : _itoa(subjects[7], szNum[7], 10),
					(subjects[8] < 0) ? "" : _itoa(subjects[8], szNum[8], 10),
					(subjects[9] < 0) ? "" : _itoa(subjects[9], szNum[9], 10),
					(subjects[10] < 0) ? "" : _itoa(subjects[10], szNum[10], 10),
					(subjects[11] < 0) ? "" : _itoa(subjects[11], szNum[11], 10),
					(subjects[12] < 0) ? "" : _itoa(subjects[12], szNum[12], 10),
					(subjects[13] < 0) ? "" : _itoa(subjects[13], szNum[13], 10));

				if(!CBatchBuilder::AddCertificateRecord(szBuff))
				{
					MessageBox(hDlg, "Не удалось добавить запись. Проверте настройки прогрммы.",
						"Ошибка: ", MB_OK|MB_ICONERROR);
				}
			}
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}


#ifndef _BUILD_SHORT
INT_PTR CALLBACK InputEntrantDlgProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		{
			char szFilePath[256] = { 0 };
			HWND hwnd;
			sprintf(szFilePath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());
			hwnd = GetDlgItem(hDlg, IDC_STATIC3);
			SetWindowText(hwnd, szFilePath);
			return (INT_PTR)TRUE;
		}
	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if(LOWORD(wParam) == IDOK)
			{
				char szFirstName[256] = { 0 };
				char szLastName[256] = { 0 };
				char szPatronymicName[256] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT1, szLastName, sizeof(szLastName));
				GetDlgItemText(hDlg, IDC_EDIT2, szFirstName, sizeof(szFirstName));
				GetDlgItemText(hDlg, IDC_EDIT3, szPatronymicName, sizeof(szPatronymicName));

				if(strlen(szFirstName) == 0)
				{
					MessageBox(hDlg, "Не заполнено поле имени абитуриента",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}
				if(strlen(szLastName) == 0)
				{
					MessageBox(hDlg, "Не заполнено поле фамилии абитуриента",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}

				char szNumber[32] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT4, szNumber, sizeof(szNumber));
				if(!CBatchBuilder::IsNumberCorrect(szNumber))
				{
					MessageBox(hDlg, "Введённый номер сертификата не соответствует формату xx-xxxxxxxxx-xx. Запись не дбавлена",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}

				char szPassportNumber[64] = { 0 };
				char szPassportSeria[64] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT5, szPassportNumber, sizeof(szPassportNumber));
				if(strlen(szPassportNumber) == 0)
				{
					MessageBox(hDlg, "Паспортные данные заполнены неверно",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}
				GetDlgItemText(hDlg, IDC_EDIT6, szPassportSeria, sizeof(szPassportSeria));
				if(strlen(szPassportSeria) == 0)
				{
					MessageBox(hDlg, "Паспортные данные заполнены неверно",
						"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
					return 1;
				}

				char szDirectionCode[64] = { 0 };
				char szSpecializationCode[64] = { 0 };

				GetDlgItemText(hDlg, IDC_EDIT7, szDirectionCode, sizeof(szDirectionCode));
				GetDlgItemText(hDlg, IDC_EDIT8, szSpecializationCode, sizeof(szSpecializationCode));

				char szBuff[1024] = { 0 };
				sprintf(szBuff, "%s%%%s%%%s%%%s%%%s%%%s%%%s%%%s",
					szLastName, 
					szFirstName,
					szPatronymicName,
					szNumber,
					szPassportNumber,
					szPassportSeria,
					szDirectionCode,
					szSpecializationCode);

				if(!CBatchBuilder::AddEntrantRecord(szBuff))
				{
					MessageBox(hDlg, "Не удалось добавить запись. Проверте настройки прогрммы.",
						"Ошибка: ", MB_OK|MB_ICONERROR);
				}

			}
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
#endif


// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

bool MarkValidate(HWND hDlg, int CtlID, WPARAM wParam, LPARAM lParam)
{
	if(LOWORD(wParam) == CtlID && HIWORD(wParam) == EN_KILLFOCUS)
	{
		BOOL bTranslated = TRUE; 
		UINT mark = GetDlgItemInt(hDlg, CtlID, &bTranslated, FALSE);
		if(mark > 100)
		{
			char szError[256] = { 0 };
			sprintf(szError, "Максимальный балл по предмету \"%s\" не может превышать 100", 
				subjectName[CtlID - IDC_EDIT5]);
			MessageBox(hDlg, szError,
				"Ошибка: ", MB_OK|MB_ICONEXCLAMATION);
				PostMessage(hDlg, WM_NEXTDLGCTL, (WPARAM)GetDlgItem(hDlg, CtlID), 1);
				return false;
		}
	}
	return true;
}
