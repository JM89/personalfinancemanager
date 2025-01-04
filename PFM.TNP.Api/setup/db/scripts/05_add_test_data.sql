USE PFM_TNP_DB
GO

set @PensionId := (SELECT UUID());
insert into Pensions (Id, SchemeName, LoginUrl, UserId, CurrencyId, CountryId)
values (@PensionId, CONCAT('Provider - Scheme ', @PensionId), 'https://provider', 'jess', 1, 1)

set @PensionId := (SELECT UUID());
insert into Pensions (Id, SchemeName, LoginUrl, UserId, CurrencyId, CountryId)
values (@PensionId, CONCAT('Provider - Scheme ', @PensionId), 'https://provider', 'jess', 1, 1)

set @PensionId := (SELECT UUID());
insert into Pensions (Id, SchemeName, LoginUrl, UserId, CurrencyId, CountryId)
values (@PensionId, CONCAT('Provider - Scheme ', @PensionId), 'https://provider', 'jess', 1, 1)

