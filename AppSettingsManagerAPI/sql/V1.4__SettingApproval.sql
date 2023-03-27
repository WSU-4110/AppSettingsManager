START TRANSACTION;

ALTER TABLE `Settings` ADD `ApprovedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `Settings` ADD `ApprovedBy` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `Settings` ADD `IsApproved` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230307233630_SettingApproval', '6.0.10');

COMMIT;
