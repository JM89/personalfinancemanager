USE PFM_TNP_DB
GO

set @PensionId := (SELECT UUID());
insert into Pensions (Id, SchemeName, LoginUrl, UserId, CurrencyId, CountryId)
values (@PensionId, CONCAT('Provider - Scheme ', @PensionId), 'https://provider', 'jess', 1, 1);

set @IncomeTaxId := (SELECT UUID());
insert into IncomeTaxReports (Id, PayDay, TaxableIncome, IncomeTaxPaid, NationalInsurancePaid, UserId, CurrencyId, CountryId)
values (@IncomeTaxId, now(), 1000, 20, 30, 'jess', 1, 1)