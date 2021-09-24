#pragma once

#include "stdio.h"
#include "time.h"

#define NAME_FOR_CERTIFICATE	1
#define NAME_FOR_ENTRANT		2

class CSettings
{
private:
	CSettings(void);
	~CSettings(void);

public:
	static bool LoadSettings();
	static char* GenerateName(int nameFor);

	static char* GetPath();
	static char* GetCertificateFileName();
	static char* GetEntrantFileName();

	static void SetPath(char* path);
	static void NewCertificateFile();
	static void NewEntrantFile();

private:
	static char* m_lpszPath;
	static char* m_lpszCertificateFile;
	static char* m_lpszEntrantFile;
};
