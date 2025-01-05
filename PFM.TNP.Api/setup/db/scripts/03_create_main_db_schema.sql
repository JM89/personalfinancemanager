USE PFM_TNP_DB;

CREATE TABLE IF NOT EXISTS Pensions (
    Id varchar(36) not null,
    SchemeName nvarchar(500) not null,
    LoginUrl nvarchar(500) not null,
    LastUpdated datetime not null default now(),
    CurrentPot decimal not null default 0,
    CurrentContribution decimal not null default 0,
    UserId nvarchar(500) not null,
    CurrencyId int not null,
    CountryId int not null,
    PRIMARY KEY(Id)
);

CREATE TABLE IF NOT EXISTS IncomeTaxReports (
    Id varchar(36) not null,
    PayDay datetime not null,
    TaxableIncome decimal not null,
    IncomeTaxPaid decimal not null,
    NationalInsurancePaid decimal not null,
    UserId nvarchar(500) not null,
    CurrencyId int not null,
    CountryId int not null,
    PRIMARY KEY(Id)
);