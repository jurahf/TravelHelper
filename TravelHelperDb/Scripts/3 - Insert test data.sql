--- Пользователь
SET IDENTITY_INSERT UserSet ON;
insert into UserSet
(Id, Login, UserSettings_Id)
values
(1, 'TestUser', null)
SET IDENTITY_INSERT UserSet OFF;



--- Путешествие
SET IDENTITY_INSERT TravelSet ON;
insert into TravelSet
(id, Name, StartDate, EndDate, User_id, City_id)
values
(1, 'В Казань на выходные', '2018-09-22', '2018-09-23', 1, (select id from CitySet where Name = 'Казань'))
SET IDENTITY_INSERT TravelSet OFF;



--- Настройки пользователя
SET IDENTITY_INSERT UserSettingsSet ON;
insert into UserSettingsSet
(id, SelectedTravelId)
values
(1, 1)
SET IDENTITY_INSERT UserSettingsSet OFF;

update UserSet
set UserSettings_id = 1
where ID = 1



