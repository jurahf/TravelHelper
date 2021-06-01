
--- Пользователь
insert into "UserSet"
("Id", "Login", "UserSettingsId")
values
(1, 'TestUser', null);



--- Путешествие
insert into "TravelSet"
("Id", "Name", "StartDate", "EndDate", "CurrentDate", "UserId", "CityId")
values
(1, 'В Казань на выходные', '2021-06-01', '2021-06-02', '2021-06-01', 1, (select "Id" from "CitySet" where "Name" = 'Казань'))
;



--- Настройки пользователя
insert into "UserSettingsSet"
("Id", "SelectedTravelId")
values
(1, 1)
;

update "UserSet"
set "UserSettingsId" = 1
where "Id" = 1;



