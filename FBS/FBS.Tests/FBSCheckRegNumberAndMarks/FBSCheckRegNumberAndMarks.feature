Функция: Запрос по номеру и серии документа, и баллам по предметам
	 
Предыстория: 
	Допустим Я захожу под пользователем "super"
	И Я открываю страницу Запрос по номеру и серии документа, и баллам по предметам

Сценарий: 01. Проверка открытия страницы с Запрос по номеру и серии документа, и баллам по предметам
	То нахожусь на странице Запрос по номеру и серии документа, и баллам по предметам

Сценарий: 02. Проверка присутствия хлебных крошек на странице Запрос по номеру и серии документа, и баллам по предметам
	То на странице есть следующие хлебные крошки:
         | Text                                                       |
         | Свидетельства                                              |
         | Свидетельства ЕГЭ                                          |
         | Запрос по номеру и серии документа, и баллам по предметам |

Сценарий: 03. Проверка полей по умолчанию
То вижу в полях следующие данные:
         | Name                      | Value           |
         | cSeries                   | Серия документа |
         | cNumber                   | Номер документа |
         | rpSubjects_ctl01_txtValue |                 |
         | rpSubjects_ctl02_txtValue |                 |
         | rpSubjects_ctl03_txtValue |                 |
         | rpSubjects_ctl04_txtValue |                 |
         | rpSubjects_ctl05_txtValue |                 |
         | rpSubjects_ctl06_txtValue |                 |
         | rpSubjects_ctl07_txtValue |                 |
         | rpSubjects_ctl08_txtValue |                 |
         | rpSubjects_ctl09_txtValue |                 |
         | rpSubjects_ctl10_txtValue |                 |
         | rpSubjects_ctl11_txtValue |                 |
         | rpSubjects_ctl12_txtValue |                 |
         | rpSubjects_ctl13_txtValue |                 |
         | rpSubjects_ctl14_txtValue |                 |

Сценарий: 04. Проверка обязательный полей
	Когда вношу в поля следующие данные:
         | Name                      | Value |
         | cSeries                   |       |
         | cNumber                   |       |
         | rpSubjects_ctl01_txtValue |       |
         | rpSubjects_ctl02_txtValue |       |
         | rpSubjects_ctl03_txtValue |       |
         | rpSubjects_ctl04_txtValue |       |
         | rpSubjects_ctl05_txtValue |       |
         | rpSubjects_ctl06_txtValue |       |
         | rpSubjects_ctl07_txtValue |       |
         | rpSubjects_ctl08_txtValue |       |
         | rpSubjects_ctl09_txtValue |       |
         | rpSubjects_ctl10_txtValue |       |
         | rpSubjects_ctl11_txtValue |       |
         | rpSubjects_ctl12_txtValue |       |
         | rpSubjects_ctl13_txtValue |       |
         | rpSubjects_ctl14_txtValue |       |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                             |
         | Произошли следующие ошибки:                       |
         | Поле "Номер документа" обязательно для заполнения |

Сценарий: 05. Проверка обязательности заполнения полей с баллами
	Когда вношу в поля следующие данные:
          | Name                      | Value  |
          | cSeries                   | 9205   |
          | cNumber                   | 527439 |
          | rpSubjects_ctl01_txtValue |        |
          | rpSubjects_ctl02_txtValue |        |
          | rpSubjects_ctl03_txtValue |        |
          | rpSubjects_ctl04_txtValue |        |
          | rpSubjects_ctl05_txtValue |        |
          | rpSubjects_ctl06_txtValue |        |
          | rpSubjects_ctl07_txtValue |        |
          | rpSubjects_ctl08_txtValue |        |
          | rpSubjects_ctl09_txtValue |        |
          | rpSubjects_ctl10_txtValue |        |
          | rpSubjects_ctl11_txtValue |        |
          | rpSubjects_ctl12_txtValue |        |
          | rpSubjects_ctl13_txtValue |        |
          | rpSubjects_ctl14_txtValue |        |	
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                   |
         | Произошли следующие ошибки:             |
         | Укажите баллы хотя бы по двум предметам |

Сценарий: 06. Проверка очистки полей с помощью кнопки "Очистить"
	Когда вношу в поля следующие данные:
         | Name                      | Value  |
         | txtSeries                 | 9205   |
         | txtNumber                 | 527439 |
         | rpSubjects_ctl01_txtValue | 44     |
         | rpSubjects_ctl02_txtValue | 44     |
         | rpSubjects_ctl03_txtValue | 44     |
         | rpSubjects_ctl04_txtValue | 55     |
         | rpSubjects_ctl05_txtValue | 66     |
         | rpSubjects_ctl06_txtValue | 77     |
         | rpSubjects_ctl07_txtValue | 88     |
         | rpSubjects_ctl08_txtValue | 99     |
         | rpSubjects_ctl09_txtValue | 99     |
         | rpSubjects_ctl10_txtValue | 10     |
         | rpSubjects_ctl11_txtValue | 11     |
         | rpSubjects_ctl12_txtValue | 10     |
         | rpSubjects_ctl13_txtValue | 11     |
         | rpSubjects_ctl14_txtValue | 10     |
	И нажимаю кнопку "Очистить"
	То вижу в полях следующие данные:
         | Name                      | Value           |
         | cSeries                   | Серия документа |
         | cNumber                   | Номер документа |
         | rpSubjects_ctl01_txtValue |                 |
         | rpSubjects_ctl02_txtValue |                 |
         | rpSubjects_ctl03_txtValue |                 |
         | rpSubjects_ctl04_txtValue |                 |
         | rpSubjects_ctl05_txtValue |                 |
         | rpSubjects_ctl06_txtValue |                 |
         | rpSubjects_ctl07_txtValue |                 |
         | rpSubjects_ctl08_txtValue |                 |
         | rpSubjects_ctl09_txtValue |                 |
         | rpSubjects_ctl10_txtValue |                 |
         | rpSubjects_ctl11_txtValue |                 |
         | rpSubjects_ctl12_txtValue |                 |
         | rpSubjects_ctl13_txtValue |                 |
         | rpSubjects_ctl14_txtValue |                 |

Сценарий: 07. Проверка результата, если внесены верные данные
	Когда вношу в поля следующие данные:
		  | Name                      | Value  |
		  | txtSeries                 | 9205   |
		  | txtNumber                 | 527439 |
		  | rpSubjects_ctl01_txtValue |        |
		  | rpSubjects_ctl02_txtValue |        |
		  | rpSubjects_ctl03_txtValue | 44     |
		  | rpSubjects_ctl04_txtValue |        |
		  | rpSubjects_ctl05_txtValue | 70     |
		  | rpSubjects_ctl06_txtValue |        |
		  | rpSubjects_ctl07_txtValue |        |
		  | rpSubjects_ctl08_txtValue |        |
		  | rpSubjects_ctl09_txtValue |        |
		  | rpSubjects_ctl10_txtValue |        |
		  | rpSubjects_ctl11_txtValue |        |
		  | rpSubjects_ctl12_txtValue |        |
		  | rpSubjects_ctl13_txtValue |        |
		  | rpSubjects_ctl14_txtValue |        |
	И нажимаю кнопку "Проверить"
	То на экране есть:
		  | Value                                                                                                                                                                                                                                                         |
		  | Результат запроса по номеру и серии документа, и баллам по предметам                                                                                                                                                                                          |
		  | Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство. |
	И на экране есть:
          | Value                            |
          | Свидетельство                    |
          | 16-000027009-10                  |
          | 16-000027009-11                  |
          | 16-000027009-12                  |
          | ТН                               |
          | 6856345                          |
          | 6856345                          |
          | 6856345                          |
          | Документ                         |
          | 9205                             |
          | 527439                           |
          | 9205                             |
          | 527439                           |
          | 9205                             |
          | 527439                           |
          | Регион                           |
          | Республика Татарстан (Татарстан) |
          | Республика Татарстан (Татарстан) |
          | Республика Татарстан (Татарстан) |
          | Год                              |
          | 2010                             |
          | 2011                             |
          | 2012                             |
          | Статус                           |
          | Истек срок                       |
          | Действительно                    |
          | Действительно                    |
          | Проверки *                       |
          | 1                                |
          | 0                                |
          | 0                                |
	И на экране есть:
		  | Value                                                                                                                                 |
		  | * Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер. |

Сценарий: 08. Проверка результата "Не найдено"
	Когда вношу в поля следующие данные:
		  | Name                      | Value  |
		  | txtSeries                 | 9205   |
		  | txtNumber                 | 527438 |
		  | rpSubjects_ctl01_txtValue |        |
		  | rpSubjects_ctl02_txtValue |        |
		  | rpSubjects_ctl03_txtValue | 44     |
		  | rpSubjects_ctl04_txtValue |        |
		  | rpSubjects_ctl05_txtValue | 70     |
		  | rpSubjects_ctl06_txtValue |        |
		  | rpSubjects_ctl07_txtValue |        |
		  | rpSubjects_ctl08_txtValue |        |
		  | rpSubjects_ctl09_txtValue |        |
		  | rpSubjects_ctl10_txtValue |        |
		  | rpSubjects_ctl11_txtValue |        |
		  | rpSubjects_ctl12_txtValue |        |
		  | rpSubjects_ctl13_txtValue |        |
		  | rpSubjects_ctl14_txtValue |        |
	И нажимаю кнопку "Проверить"
	То на экране есть:
		  | Value                                                                                                                                                                                                                                                         |
		  | Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство. |
	И на экране есть:
         | Value                               |
         | По Вашему запросу ничего не найдено |

Сценарий: 09. Проверка правильности ввода данных в поля Номер и Серия документа
	Когда вношу в поля следующие данные:
         | Name                      | Value           |
         | txtSeries                 | Серия документа |
         | txtNumber                 | Номер документа |
         | rpSubjects_ctl01_txtValue |                 |
         | rpSubjects_ctl02_txtValue |                 |
         | rpSubjects_ctl03_txtValue |                 |
         | rpSubjects_ctl04_txtValue |                 |
         | rpSubjects_ctl05_txtValue |                 |
         | rpSubjects_ctl06_txtValue |                 |
         | rpSubjects_ctl07_txtValue |                 |
         | rpSubjects_ctl08_txtValue |                 |
         | rpSubjects_ctl09_txtValue |                 |
         | rpSubjects_ctl10_txtValue |                 |
         | rpSubjects_ctl11_txtValue |                 |
         | rpSubjects_ctl12_txtValue |                 |
         | rpSubjects_ctl13_txtValue |                 |
         | rpSubjects_ctl14_txtValue |                 |
	И нажимаю кнопку "Проверить"
	То на экране есть:
         | Value                                                        |
         | Произошли следующие ошибки:                                  |
         | В поле "Серия документа" не должно быть пробелов             |
         | Поле "Номер документа" должно быть до 10 символов. Сейчас 15 |

Сценарий: 10. Проверка правильности заполнения поля Баллы
	Когда вношу в поля следующие данные:
         | Name                      | Value  |
         | txtSeries                 | 9205   |
         | txtNumber                 | 527439 |
         | rpSubjects_ctl01_txtValue |        |
         | rpSubjects_ctl02_txtValue | test   |
         | rpSubjects_ctl03_txtValue |        |
         | rpSubjects_ctl04_txtValue |        |
         | rpSubjects_ctl05_txtValue |        |
         | rpSubjects_ctl06_txtValue |        |
         | rpSubjects_ctl07_txtValue |        |
         | rpSubjects_ctl08_txtValue |        |
         | rpSubjects_ctl09_txtValue |        |
         | rpSubjects_ctl10_txtValue |        |
         | rpSubjects_ctl11_txtValue |        |
         | rpSubjects_ctl12_txtValue |        |
         | rpSubjects_ctl13_txtValue |        |
         | rpSubjects_ctl14_txtValue |        |
	И нажимаю кнопку "Проверить"
	То на экране есть:
		 | Value                                    |
		 | Произошли следующие ошибки:              |
		 | Проверьте правильность заполнения баллов |

