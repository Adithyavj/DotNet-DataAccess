﻿CREATE TABLE [dbo].[ContactPhoneNumbersMapping]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [PhoneNumberId] INT NOT NULL
)
