#include "StdAfx.h"
#include "Settings.h"


char* CSettings:: m_lpszPath = NULL;
char* CSettings:: m_lpszCertificateFile = NULL;
char* CSettings:: m_lpszEntrantFile = NULL;

CSettings::CSettings(void)
{
}

CSettings::~CSettings(void)
{
	if(m_lpszPath) ::free(m_lpszPath);
	if(m_lpszCertificateFile) ::free(m_lpszCertificateFile);
	if(m_lpszEntrantFile) ::free(m_lpszEntrantFile);
}


bool CSettings::LoadSettings()
{
	if(m_lpszPath) ::free(m_lpszPath);
	if(m_lpszCertificateFile) ::free(m_lpszCertificateFile);
	if(m_lpszEntrantFile) ::free(m_lpszEntrantFile);

	m_lpszPath = _strdup("1");
	m_lpszCertificateFile = _strdup("1");
	m_lpszEntrantFile = _strdup("1");

	HKEY	hKey;
	DWORD	dwDisposition;
	DWORD	dwType;
	DWORD	dwSize;
	
	char szBuff[1024];

	if((RegCreateKeyEx(HKEY_CURRENT_USER,  "software\\batchBuilder", 
				   0, 0, 
				   REG_OPTION_NON_VOLATILE,  
				   KEY_ALL_ACCESS,
				   NULL,
				   &hKey,
				   &dwDisposition)) != ERROR_SUCCESS)
	{
		LPVOID lpMsgBuf = NULL;

		FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL, GetLastError(), 
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);
		MessageBox(0, (LPCSTR)lpMsgBuf, "Ошибка: ", MB_OK|MB_ICONERROR);
		return false;
	}
	
	if(REG_CREATED_NEW_KEY == dwDisposition)
	{
		// если это первый запуск:
		memset(szBuff, 0, sizeof(szBuff));
		GetCurrentDirectory(sizeof(szBuff), szBuff);
		
		m_lpszPath = _strdup(szBuff);
		m_lpszCertificateFile = _strdup(GenerateName(NAME_FOR_CERTIFICATE));
		m_lpszEntrantFile = _strdup(GenerateName(NAME_FOR_ENTRANT));

		if(((RegSetValueEx(hKey, "path", 0, REG_SZ, (BYTE*)m_lpszPath, (strlen(m_lpszPath) + 1))) != ERROR_SUCCESS)||
			((RegSetValueEx(hKey, "CertificateFile", 0, REG_SZ, (BYTE*)m_lpszCertificateFile, (strlen(m_lpszCertificateFile) + 1))) != ERROR_SUCCESS)||
			((RegSetValueEx(hKey, "EntrantFile", 0, REG_SZ, (BYTE*)m_lpszEntrantFile, (strlen(m_lpszEntrantFile) + 1))) != ERROR_SUCCESS))
		{
			LPVOID lpMsgBuf = NULL;

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, GetLastError(), 
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);
			MessageBox(0, (LPCSTR)lpMsgBuf, "Ошибка: ", MB_OK|MB_ICONERROR);
			RegCloseKey(hKey);
			return false;
		}
	}
	else
	{
		// если не первый запуск:
		memset(szBuff, 0, sizeof(szBuff));
		//if((
			RegQueryValueEx(hKey, "path", 0, &dwType, (BYTE*)szBuff, &dwSize);//) != ERROR_SUCCESS)
		/*{
			LPVOID lpMsgBuf = NULL;

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, GetLastError(), 
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);
			MessageBox(0, (LPCSTR)lpMsgBuf, "Ошибка: ", MB_OK|MB_ICONERROR);
			RegCloseKey(hKey);
			return false;
		}*/
		m_lpszPath = _strdup(szBuff);

		memset(szBuff, 0, sizeof(szBuff));
		//if((
			RegQueryValueEx(hKey, "CertificateFile", 0, &dwType, (BYTE*)szBuff, &dwSize);//) != ERROR_SUCCESS)
		/*{
			LPVOID lpMsgBuf = NULL;

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, GetLastError(), 
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);
			MessageBox(0, (LPCSTR)lpMsgBuf, "Ошибка: ", MB_OK|MB_ICONERROR);
			RegCloseKey(hKey);
			return false;
		}*/
		m_lpszCertificateFile = _strdup(szBuff);

		memset(szBuff, 0, sizeof(szBuff));
		//if((
			RegQueryValueEx(hKey, "EntrantFile", 0, &dwType, (BYTE*)szBuff, &dwSize); //) != ERROR_SUCCESS)
		/*{
			LPVOID lpMsgBuf = NULL;

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, GetLastError(), 
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpMsgBuf, 0, NULL);
			MessageBox(0, (LPCSTR)lpMsgBuf, "Ошибка: ", MB_OK|MB_ICONERROR);
			RegCloseKey(hKey);
			return false;
		}*/
		m_lpszEntrantFile = _strdup(szBuff);
	}
	RegCloseKey(hKey);
	return true;
}

char* CSettings::GenerateName(int nameFor)
{
	int prnd = 43512;
	static char buff[1024];

	memset(buff, 0, sizeof(buff));
	prnd = (5*prnd + 1) & 0xFFFF;
	
	switch(nameFor)
	{
	case NAME_FOR_CERTIFICATE:
		{
			sprintf(buff,"fbs_certificate_%lx_%x.dat",(long)time(0),prnd);
		} break;
	case NAME_FOR_ENTRANT:
		{
			sprintf(buff,"fbs_entrant_%lx_%x.dat",(long)time(0),prnd);
		} break;
	}
	
	return buff;
}

char* CSettings:: GetPath()
{
	return m_lpszPath;
}
char* CSettings:: GetCertificateFileName()
{
	return m_lpszCertificateFile;
}

char* CSettings:: GetEntrantFileName()
{
	return m_lpszEntrantFile;
}

void CSettings::SetPath(char* path)
{
	HKEY hKey;
	if((RegOpenKey(HKEY_CURRENT_USER, "software\\batchBuilder", &hKey)) == ERROR_SUCCESS)
	{
		if(m_lpszPath) ::free(m_lpszPath);
		m_lpszPath = _strdup(path);
		RegSetValueEx(hKey, "path", 0, REG_SZ, (BYTE*)m_lpszPath, (strlen(m_lpszPath) + 1));
		RegCloseKey(hKey);
	}
}

void CSettings::NewCertificateFile()
{
	HKEY hKey;
	if((RegOpenKey(HKEY_CURRENT_USER, "software\\batchBuilder", &hKey)) == ERROR_SUCCESS)
	{
		if(m_lpszCertificateFile) ::free(m_lpszCertificateFile);
		m_lpszCertificateFile = _strdup(GenerateName(NAME_FOR_CERTIFICATE));
		RegSetValueEx(hKey, "CertificateFile", 0, REG_SZ, (BYTE*)m_lpszCertificateFile, (strlen(m_lpszCertificateFile) + 1));
		RegCloseKey(hKey);
	}
}

void CSettings::NewEntrantFile()
{
	HKEY hKey;
	if((RegOpenKey(HKEY_CURRENT_USER, "software\\batchBuilder", &hKey)) == ERROR_SUCCESS)
	{
		if(m_lpszEntrantFile) ::free(m_lpszEntrantFile);
		m_lpszEntrantFile= _strdup(GenerateName(NAME_FOR_ENTRANT));
		RegSetValueEx(hKey, "EntrantFile", 0, REG_SZ, (BYTE*)m_lpszEntrantFile, (strlen(m_lpszEntrantFile) + 1));
		RegCloseKey(hKey);
	}
}
