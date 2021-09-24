#include "StdAfx.h"
#include "StrObj.h"

CStrObj::CStrObj(void)
{
	m_lpszContent = (char*)malloc(1);
	m_length = 0;
	m_lpszContent[0] = '\0';
}

CStrObj::~CStrObj(void)
{
	if(m_lpszContent) free(m_lpszContent);
	m_lpszContent = NULL;
	m_length = 0;
}

CStrObj::CStrObj(char *str)
{
	if(str)
	{
		m_length = (unsigned int)strlen(str);
		m_lpszContent = (char*)malloc(m_length + 1);
		strcpy(m_lpszContent, str);
		m_lpszContent[m_length] = '\0';
	}
}
CStrObj::CStrObj(CStrObj &str)
{
	*this = str;
}

CStrObj CStrObj::operator =(const char *str)
{
	int newlen = (int)strlen(str);
	if (m_lpszContent)
	{
		free(m_lpszContent);
		m_lpszContent = 0;
		m_length = 0;
	}
	m_lpszContent = (char*)malloc(newlen + 1);
	m_length = newlen;
	strcpy(m_lpszContent, str);
	m_lpszContent[m_length] = '\0';
	return *this;
}

CStrObj CStrObj::operator =(char *str)
{
	int newlen = strlen(str);
	if (m_lpszContent)
	{
		free(m_lpszContent);
		m_lpszContent = 0;
		m_length = 0;
	}
	m_lpszContent = (char*)malloc(newlen + 1);
	m_length = newlen;
	strcpy(m_lpszContent, str);
	m_lpszContent[m_length] = '\0';
	return *this;
}

CStrObj CStrObj::operator =(CStrObj &rhs)
{
	if(m_lpszContent) free(m_lpszContent);
	m_length = rhs.m_length;
	m_lpszContent = _strdup(rhs.m_lpszContent);
	return *this;
}

CStrObj::operator const char*()
{
	if (m_lpszContent)
		return m_lpszContent;
	return "";
}

CStrObj::operator char*()
{
	if (m_lpszContent)
		return m_lpszContent;
	return "";
}

CStrObj CStrObj::operator +=(char *rhs)
{
	*this += (const char*)(int)rhs;
	return *this;
}
CStrObj CStrObj::operator +=(const char *rhs)
{
	m_length+=strlen(rhs);
	m_lpszContent = (char*)realloc(m_lpszContent, m_length);
	strcat(m_lpszContent, rhs);
	m_lpszContent[m_length] = '\0'; 
	return *this;
}
CStrObj CStrObj:: operator +=(CStrObj &rhs)
{
	*this += (const char*)rhs;
	return *this;
}

char CStrObj::operator[] (int index)
{
	if((index < m_length) && (NULL != m_lpszContent))
	{
		return m_lpszContent[index];
	}
	return '\0';
}

int CStrObj::alloc(unsigned int val)
{
	if (!val)
		return false;
	if (m_lpszContent)
		free(m_lpszContent);
	m_length = val+1;
	m_lpszContent = (char*)malloc(m_length);
	m_lpszContent[0] = '\0';
	return true;
}

CStrObj& CStrObj::arg(const char *val)
{
	char* tmp = _strdup(m_lpszContent);
	char* pos = tmp;
	while((pos=strchr(pos,'%')))
	{
		if (((*(pos+1)) > '1') && ((*(pos+1)) < '9'))
		{
			*pos = 0;
			*this = tmp;
			*this += val;
			*this += pos+2;
			break;
		}
		pos+=2;
	}
	free(tmp);
	return *this;
}

CStrObj& CStrObj::arg(char *val)
{
	return this->arg((const char*)val);	
}

CStrObj& CStrObj::arg(int val)
{
	char buf[512];
	sprintf(buf, "%d", val);
	return this->arg(buf);
}
CStrObj& CStrObj::arg(char ch)
{
	char buf[2];
	buf[1]=0;
	buf[0]=ch;
	return this->arg(buf);
}
CStrObj& CStrObj::arg(CStrObj& val)
{
	return this->arg((const char*)val);
}

int CStrObj::operator!= (const char *rhs)
{
	return (strcmp(m_lpszContent, rhs)) ? false : true;
}

int CStrObj::operator== (const char *rhs)
{
	return (strcmp(m_lpszContent, rhs)) ? true : false;
}