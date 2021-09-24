-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (43, '043_2012_06_07_FBDOSH_groups.sql')
-- =========================================================================


SET IDENTITY_INSERT dbo.[System] ON
INSERT INTO dbo.System
        ( SystemID,
          Code ,
          Name ,
          FullName ,
          AvailableRegistration
        )
VALUES  ( 6,
		  'olymp' , -- Code - nvarchar(510)
          'ФБД ОШ' , -- Name - nvarchar(510)
          'ФБД ОШ' , -- FullName - nvarchar(1000)
          1  -- AvailableRegistration - bit
        )
SET IDENTITY_INSERT dbo.[System] OFF

SET IDENTITY_INSERT dbo.[Group] ON 

INSERT INTO dbo.[Group]
         (Id,
         Code ,
          Name ,
          SystemID ,
          [Default] ,
          IsUserIS
        )
VALUES  ( 
	      21,
		  'olymp_administrator' , -- Code - nvarchar(255)
          'Администратор ФБД ОШ' , -- Name - nvarchar(255)
          6 , -- SystemID - int
          0 , -- Default - bit
          0  -- IsUserIS - bit
        )
        
INSERT INTO dbo.[Group]
        ( Id,
          Code ,
          Name ,
          SystemID ,
          [Default] ,
          IsUserIS
        )
VALUES  ( 
	      22,
		  'olymp_user' , -- Code - nvarchar(255)
          'Пользователь ФБД ОШ' , -- Name - nvarchar(255)
          6 , -- SystemID - int
          1 , -- Default - bit
          0  -- IsUserIS - bit
        )        

INSERT INTO dbo.[Group]
        ( Id,
          Code ,
          Name ,
          SystemID ,
          [Default] ,
          IsUserIS
        )
VALUES  ( 
	      23,
		  'olymp_authorizedstaff' , -- Code - nvarchar(255)
          'Уполномоченный сотрудник ОУ' , -- Name - nvarchar(255)
          6 , -- SystemID - int
          0 , -- Default - bit
          0  -- IsUserIS - bit
        )   
 
 
SET IDENTITY_INSERT dbo.[Group] OFF