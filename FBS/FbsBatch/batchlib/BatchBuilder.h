#pragma once

#include "Settings.h"
#include "stdio.h"
#include "StrObj.h"

#ifndef NULL
#define NULL 0
#endif

#define sizeLimit 1048576

class CBatchBuilder
{
public:
	static bool ImportCertificates(char*);
	static bool ImportEntrants(char*);
	static bool AddCertificateRecord(char*);
	static bool AddEntrantRecord(char*);
	static bool IsNumberCorrect(char*);
private:
	static bool IsCertificateCSVRecordCorrect(char*);
	static bool IsEntrantCSVRecordCorrect(char*);

	CBatchBuilder(void);
	~CBatchBuilder(void);
};
