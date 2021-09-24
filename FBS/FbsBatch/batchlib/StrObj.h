#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#ifndef NULL
#define NULL 0 
#endif

class CStrObj
{
public:
	CStrObj(void);
public:
	~CStrObj(void);

	CStrObj(char*);
	CStrObj(CStrObj &);

	CStrObj operator =(const char*);
	CStrObj operator =(char*);
	CStrObj operator =(CStrObj&);

	operator const char*();
	operator char*();

	CStrObj operator +=(char*);
	CStrObj operator +=(const char*);
	CStrObj operator +=(CStrObj&);

	char operator[] (int);

	int operator!= (const char*);
	int operator== (const char*);

	int alloc(unsigned);

	CStrObj& arg(const char*);
	CStrObj& arg(char*);
	CStrObj& arg(int);
	CStrObj& arg(char);
	CStrObj& arg(CStrObj&);
private:
	unsigned int m_length;
	char *m_lpszContent;
};
