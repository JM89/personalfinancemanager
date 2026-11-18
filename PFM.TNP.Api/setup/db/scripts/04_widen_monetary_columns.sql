USE PFM_TNP_DB;

CREATE TEMPORARY TABLE pfm_money_migration_check (
    decimal_columns INT NOT NULL,
    CONSTRAINT expected_pension_and_tax_decimal_columns CHECK (decimal_columns = 5)
);

INSERT INTO pfm_money_migration_check (decimal_columns)
SELECT COUNT(*)
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND DATA_TYPE = 'decimal'
  AND ((TABLE_NAME = 'Pensions' AND COLUMN_NAME IN ('CurrentPot', 'CurrentContribution'))
    OR (TABLE_NAME = 'IncomeTaxReports' AND COLUMN_NAME IN ('TaxableIncome', 'IncomeTaxPaid', 'NationalInsurancePaid')));

DROP TEMPORARY TABLE pfm_money_migration_check;

-- Preserve integer capacity and never narrow a column that already has at least two decimal places.
SET @pension_columns = (
    SELECT GROUP_CONCAT(
        CONCAT('MODIFY COLUMN `', COLUMN_NAME, '` DECIMAL(',
               GREATEST(19, NUMERIC_PRECISION - NUMERIC_SCALE + 2), ',2)',
               IF(IS_NULLABLE = 'NO', ' NOT NULL', ' NULL'),
               IF(COLUMN_DEFAULT IS NULL, '', CONCAT(' DEFAULT ', QUOTE(COLUMN_DEFAULT))))
        ORDER BY ORDINAL_POSITION SEPARATOR ', ')
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'Pensions'
      AND COLUMN_NAME IN ('CurrentPot', 'CurrentContribution')
      AND NUMERIC_SCALE < 2
);
SET @migration = IF(@pension_columns IS NULL, 'DO 0', CONCAT('ALTER TABLE `Pensions` ', @pension_columns));
PREPARE money_migration FROM @migration;
EXECUTE money_migration;
DEALLOCATE PREPARE money_migration;

SET @tax_columns = (
    SELECT GROUP_CONCAT(
        CONCAT('MODIFY COLUMN `', COLUMN_NAME, '` DECIMAL(',
               GREATEST(19, NUMERIC_PRECISION - NUMERIC_SCALE + 2), ',2)',
               IF(IS_NULLABLE = 'NO', ' NOT NULL', ' NULL'),
               IF(COLUMN_DEFAULT IS NULL, '', CONCAT(' DEFAULT ', QUOTE(COLUMN_DEFAULT))))
        ORDER BY ORDINAL_POSITION SEPARATOR ', ')
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'IncomeTaxReports'
      AND COLUMN_NAME IN ('TaxableIncome', 'IncomeTaxPaid', 'NationalInsurancePaid')
      AND NUMERIC_SCALE < 2
);
SET @migration = IF(@tax_columns IS NULL, 'DO 0', CONCAT('ALTER TABLE `IncomeTaxReports` ', @tax_columns));
PREPARE money_migration FROM @migration;
EXECUTE money_migration;
DEALLOCATE PREPARE money_migration;
