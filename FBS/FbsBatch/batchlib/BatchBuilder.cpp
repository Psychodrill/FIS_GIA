#include "StdAfx.h"
#include "BatchBuilder.h"

CBatchBuilder::CBatchBuilder(void)
{
}

CBatchBuilder::~CBatchBuilder(void)
{
}

bool CBatchBuilder::IsCertificateCSVRecordCorrect(char *lpszRecord)
{
	unsigned int i = 0, j = 0;
	for(i = 0; i < strlen(lpszRecord); i++)
	{
		if(lpszRecord[i] == '%') return false;
		if(lpszRecord[i] == ';')
		{
			lpszRecord[i] = '%';
			j++;
		}
	}
	return  (j == 18) ? true : false;
}

bool CBatchBuilder::IsEntrantCSVRecordCorrect(char *lpszRecord)
{
	unsigned int i = 0, j = 0;
	for(i = 0; i < strlen(lpszRecord); i++)
	{
		if(lpszRecord[i] == '%') return false;
		if(lpszRecord[i] == ';')
		{
			lpszRecord[i] = '%';
			j++;
		}
	}
	return  (j == 7) ? true : false;
}

bool CBatchBuilder:: ImportCertificates(char *lpszImportFullPath)
{
	char ch = '\n';
	char szFullPath[1024] = { 0 };

	sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());

	char szBuff[4096];
	memset(szBuff, 0, sizeof(szBuff));
	
	FILE *fImport = NULL, *fBatch = NULL;

	if((fImport = fopen(lpszImportFullPath, "rt")) == NULL) return false;
	if((fBatch = fopen(szFullPath, "r+t")) == NULL)
	{
		if((fBatch = fopen(szFullPath, "w+t")) == NULL)
		{
			fclose(fImport);
			return false;
		}
	}

	fseek(fImport, 0, SEEK_SET);
	fseek(fBatch, 0, SEEK_END);

	for(;fgets(szBuff, sizeof(szBuff), fImport);)
	{
		if(!IsCertificateCSVRecordCorrect(szBuff))
		{
			fclose(fImport);
			fclose(fBatch);
			return false;
		}
		unsigned int size = ftell(fBatch);

		if(size + 2 + 100 + strlen(szBuff) > sizeLimit)
		{
			// םמגûי פאיכ:
			fclose(fBatch);
			CSettings::NewCertificateFile();
			
			memset(szFullPath, 0, sizeof(szFullPath));
			sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());
			
			if((fBatch = fopen(szFullPath, "w+t")) == NULL)
			{
				fclose(fImport);
				return false;
			}
		}
		
		fwrite(szBuff, 1, strlen(szBuff), fBatch);
		//fwrite(&ch, 1, 1, fBatch);

		memset(szBuff, 0, sizeof(szBuff));
	}
	fclose(fImport);
	fclose(fBatch);

	return true;
}

bool CBatchBuilder::ImportEntrants(char *lpszImportFullPath)
{
	char ch = '\n';
	char szFullPath[1024] = { 0 };

	sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());

	char szBuff[4096];
	memset(szBuff, 0, sizeof(szBuff));
	
	FILE *fImport = NULL, *fBatch = NULL;

	if((fImport = fopen(lpszImportFullPath, "rt")) == NULL) return false;
	if((fBatch = fopen(szFullPath, "r+t")) == NULL)
	{
		if((fBatch = fopen(szFullPath, "w+t")) == NULL)
		{
			fclose(fImport);
			return false;
		}
	}

	fseek(fImport, 0, SEEK_SET);
	fseek(fBatch, 0, SEEK_END);

	for(;fgets(szBuff, sizeof(szBuff), fImport);)
	{
		if(!IsEntrantCSVRecordCorrect(szBuff))
		{
			fclose(fImport);
			fclose(fBatch);
			return false;
		}
		unsigned int size = ftell(fBatch);

		if(size + 2 + 100 + strlen(szBuff) > sizeLimit)
		{
			// םמגûי פאיכ:
			fclose(fBatch);
			CSettings::NewEntrantFile();
			
			sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());
			
			if(!(fBatch = fopen(szFullPath, "w+t")))
			{
				fclose(fImport);
				return false;
			}
		}
		
		fwrite(szBuff, 1, strlen(szBuff), fBatch);
		//fwrite(&ch, 1, 1, fBatch);

		memset(szBuff, 0, sizeof(szBuff));
	}
	fclose(fImport);
	fclose(fBatch);

	return true;
}

bool CBatchBuilder::AddCertificateRecord(char *lpszRecord)
{
	char ch = '\n';
	char szFullPath[1024] = { 0 };

	sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());

	char szBuff[4096];
	memset(szBuff, 0, sizeof(szBuff));
	
	FILE *fBatch = NULL;

	if((fBatch = fopen(szFullPath, "r+t")) == NULL)
	{
		if((fBatch = fopen(szFullPath, "w+t")) == NULL)
		{
			return false;
		}
	}
	fseek(fBatch, 0, SEEK_END);

	unsigned int size = ftell(fBatch);

	if(size + 2 + 100 + strlen(lpszRecord) > sizeLimit)
	{
		// םמגûי פאיכ:
		fclose(fBatch);
		CSettings::NewCertificateFile();

		memset(szFullPath, 0, sizeof(szFullPath));
		sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetCertificateFileName());
			
		if(!(fBatch = fopen(szFullPath, "w+t")))
		{
			return false;
		}
	}
	fwrite(lpszRecord, 1, strlen(lpszRecord), fBatch);
	fwrite(&ch, 1, 1, fBatch);
	fclose(fBatch);

	return true;
}

bool CBatchBuilder::AddEntrantRecord(char *lpszRecord)
{
	char ch = '\n';
	char szFullPath[1024] = { 0 };

	sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());

	char szBuff[4096];
	memset(szBuff, 0, sizeof(szBuff));
	
	FILE *fBatch = NULL;

	if((fBatch = fopen(szFullPath, "r+t")) == NULL)
	{
		if((fBatch = fopen(szFullPath, "w+t")) == NULL)
		{
			return false;
		}
	}
	fseek(fBatch, 0, SEEK_END);

	unsigned int size = ftell(fBatch);

	if(size + 2 + 100 + strlen(lpszRecord) > sizeLimit)
	{
		// םמגûי פאיכ:
		fclose(fBatch);
		CSettings::NewEntrantFile();
		
		memset(szFullPath, 0, sizeof(szFullPath));
		sprintf(szFullPath, "%s\\%s", CSettings::GetPath(), CSettings::GetEntrantFileName());
			
		if(!(fBatch = fopen(szFullPath, "w+t")))
		{
			return false;
		}
	}
	fwrite(lpszRecord, 1, strlen(lpszRecord), fBatch);
	fwrite(&ch, 1, 1, fBatch);
	fclose(fBatch);

	return true;
}

bool CBatchBuilder::IsNumberCorrect(char *lpszNumber)
{
	if(strlen(lpszNumber) != 15) return false;
	if(lpszNumber[2] != '-') return false;
	if(lpszNumber[12] != '-') return false;
	for(int i = 0; i < strlen(lpszNumber); i++)
	{
		if((i == 2)||(i == 12)) continue;
		if(!isdigit(lpszNumber[i])) return false;
	}
	return true;
}
