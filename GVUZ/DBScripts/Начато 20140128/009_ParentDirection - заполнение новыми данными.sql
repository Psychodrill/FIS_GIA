IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Direction_ParentDirection]') AND parent_object_id = OBJECT_ID(N'[dbo].[Direction]'))
ALTER TABLE [dbo].[Direction] DROP CONSTRAINT [FK_Direction_ParentDirection]
GO

set identity_insert ParentDirection on

delete from ParentDirection

insert into ParentDirection
(ParentDirectionID, Code, Name)
select 56,	'00.00.00', 	'яРЮПШЕ сця'
union
select 1,	'01.00.00',  	'люрелюрхйю х леуюмхйю'
union
select 2,	'02.00.00',  	'йнлоэчрепмше х хмтнплюжхнммше мюсйх'
union
select 3,	'03.00.00',  	'тхгхйю х юярпнмнлхъ'
union
select 4,	'04.00.00',  	'ухлхъ'
union
select 5,	'05.00.00',  	'мюсйх н гелке'
union
select 6,	'06.00.00',  	'ахнкнцхвеяйхе мюсйх'
union
select 7,	'07.00.00',  	'юпухрейрспю'
union
select 8,	'08.00.00',  	'реумхйю х реумнкнцхх ярпнхрекэярбю'
union
select 9,	'09.00.00',  	'хмтнплюрхйю х бшвхякхрекэмюъ реумхйю'
union
select 10,	'10.00.00',  	'хмтнплюжхнммюъ аегноюямнярэ'
union
select 11,	'11.00.00',  	'щкейрпнмхйю, пюдхнреумхйю х яхярелш ябъгх'
union
select 12,	'12.00.00',  	'тнрнмхйю, опханпнярпнемхе, норхвеяйхе х ахнреумхвеяйхе яхярелш х реумнкнцхх'
union
select 13,	'13.00.00',  	'щкейрпн- х реокнщмепцерхйю'
union
select 14,	'14.00.00',  	'ъдепмюъ щмепцерхйю х реумнкнцхх'
union
select 15,	'15.00.00',  	'люьхмнярпнемхе'
union
select 16,	'16.00.00',  	'тхгхйн-реумхвеяйхе мюсйх х реумнкнцхх'
union
select 17,	'17.00.00',  	'нпсфхе х яхярелш бннпсфемхъ'
union
select 18,	'18.00.00',  	'ухлхвеяйхе реумнкнцхх'
union
select 19,	'19.00.00',  	'опнлшькеммюъ щйнкнцхъ х ахнреумнкнцхх'
union
select 20,	'20.00.00',  	'реумнятепмюъ аегноюямнярэ х опхпндннасярпниярбн'
union
select 21,	'21.00.00',  	'опхйкюдмюъ ценкнцхъ, цнпмне декн, метрецюгнбне декн х цендегхъ'
union
select 22,	'22.00.00',  	'реумнкнцхх люрепхюкнб'
union
select 23,	'23.00.00',  	'реумхйю х реумнкнцхх мюгелмнцн рпюмяонпрю'
union
select 24,	'24.00.00',  	'юбхюжхнммюъ х пюйермн-йнялхвеяйюъ реумхйю'
union
select 25,	'25.00.00',  	'ющпнмюбхцюжхъ х щйяоксюрюжхъ юбхюжхнммни х пюйермн-йнялхвеяйни реумхйх'
union
select 26,	'26.00.00',  	'реумхйю х реумнкнцхх йнпюакеярпнемхъ х бндмнцн рпюмяонпрю'
union
select 27,	'27.00.00',  	'сопюбкемхе б реумхвеяйху яхярелюу'
union
select 28,	'28.00.00',  	'мюмнреумнкнцхх х мюмнлюрепхюкш'
union
select 29,	'29.00.00',  	'реумнкнцхх кецйни опнлшькеммнярх'
union
select 30,	'30.00.00',  	'тсмдюлемрюкэмюъ ледхжхмю'
union
select 31,	'31.00.00',  	'йкхмхвеяйюъ ледхжхмю'
union
select 32,	'32.00.00',  	'мюсйх н гднпнбэе х опнтхкюйрхвеяйюъ ледхжхмю'
union
select 33,	'33.00.00',  	'тюплюжхъ'
union
select 34,	'34.00.00',  	'яеярпхмяйне декн'
union
select 35,	'35.00.00',  	'яекэяйне, кеямне х пшамне унгъиярбн'
union
select 36,	'36.00.00',  	'берепхмюпхъ х гннреумхъ'
union
select 37,	'37.00.00',  	'ояхункнцхвеяйхе мюсйх'
union
select 38,	'38.00.00',  	'щйнмнлхйю х сопюбкемхе'
union
select 39,	'39.00.00',  	'янжхнкнцхъ х янжхюкэмюъ пюанрю'
union
select 40,	'40.00.00',  	'чпхяопсдемжхъ'
union
select 41,	'41.00.00',  	'онкхрхвеяйхе мюсйх х пецхнмнбедемхе'
union
select 42,	'42.00.00',  	'япедярбю люяянбни хмтнплюжхх х хмтнплюжхнммн-ахакхнревмне декн'
union
select 43,	'43.00.00',  	'яепбхя х рспхгл'
union
select 44,	'44.00.00',  	'напюгнбюмхе х оедюцнцхвеяйхе мюсйх'
union
select 45,	'45.00.00',  	'ъгшйнгмюмхе х кхрепюрспнбедемхе'
union
select 46,	'46.00.00',  	'хярнпхъ х юпуенкнцхъ'
union
select 47,	'47.00.00',  	'тхкнянтхъ, щрхйю х пекхцхнбедемхе'
union
select 48,	'48.00.00',  	'ренкнцхъ'
union
select 49,	'49.00.00',  	'тхгхвеяйюъ йскэрспю х яонпр'
union
select 50,	'50.00.00',  	'хяйсяярбнгмюмхе'
union
select 51,	'51.00.00',  	'йскэрспнбедемхе х янжхнйскэрспмше опнейрш'
union
select 52,	'52.00.00',  	'яжемхвеяйхе хяйсяярбю х кхрепюрспмне рбнпвеярбн'
union
select 53,	'53.00.00',  	'лсгшйюкэмне хяйсяярбн'
union
select 54,	'54.00.00',  	'хгнапюгхрекэмне х опхйкюдмше бхдш хяйсяярб'
union
select 55,	'55.00.00',  	'щйпюммше хяйсяярбю'

set Identity_insert ParentDirection on

DBCC CheckIdent(ParentDirection, reseed, 57) 

ALTER TABLE [dbo].[Direction]  WITH CHECK ADD  CONSTRAINT [FK_Direction_ParentDirection] FOREIGN KEY([ParentID])
REFERENCES [dbo].[ParentDirection] ([ParentDirectionID])
GO

ALTER TABLE [dbo].[Direction] CHECK CONSTRAINT [FK_Direction_ParentDirection]
GO

