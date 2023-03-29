USE db;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                                                       `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
                                                       `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
                                                       CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `BaseUsers` (
                             `UserId` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                             `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
                             CONSTRAINT `PK_BaseUsers` PRIMARY KEY (`UserId`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230206201303_InitialCreate', '6.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `BaseUsers` MODIFY COLUMN `Password` varchar(36) CHARACTER SET utf8mb4 NOT NULL;

CREATE TABLE `Settings` (
                            `Id` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `Version` int NOT NULL,
                            `Input` longtext CHARACTER SET utf8mb4 NOT NULL,
                            `IsCurrent` tinyint(1) NOT NULL,
                            `CreatedBy` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `LastUpdatedAt` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
                            `CreatedAt` datetime(6) NOT NULL,
                            `BaseUserUserId` varchar(36) CHARACTER SET utf8mb4 NULL,
                            CONSTRAINT `PK_Settings` PRIMARY KEY (`Id`, `Version`),
                            CONSTRAINT `FK_Settings_BaseUsers_BaseUserUserId` FOREIGN KEY (`BaseUserUserId`) REFERENCES `BaseUsers` (`UserId`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Settings_BaseUserUserId` ON `Settings` (`BaseUserUserId`);

CREATE INDEX `IX_Settings_Id` ON `Settings` (`Id`);

CREATE INDEX `IX_Settings_IsCurrent` ON `Settings` (`IsCurrent`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230217190935_Settings', '6.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `BaseUsers` ADD `Email` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230221141212_Email', '6.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `Settings` DROP FOREIGN KEY `FK_Settings_BaseUsers_BaseUserUserId`;

DROP TABLE `BaseUsers`;

ALTER TABLE `Settings` DROP INDEX `IX_Settings_BaseUserUserId`;

ALTER TABLE `Settings` DROP COLUMN `BaseUserUserId`;

ALTER TABLE `Settings` RENAME COLUMN `Id` TO `SettingGroupId`;

ALTER TABLE `Settings` RENAME INDEX `IX_Settings_Id` TO `IX_Settings_SettingGroupId`;

CREATE TABLE `SettingGroups` (
                                 `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
                                 `CreatedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
                                 `LastUpdatedAt` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
                                 CONSTRAINT `PK_SettingGroups` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
                         `Id` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                         `Password` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                         `Email` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
                         CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Permissions` (
                               `UserId` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                               `SettingGroupId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
                               `CurrentPermissionLevel` int NOT NULL,
                               `ApprovedBy` longtext CHARACTER SET utf8mb4 NOT NULL,
                               `WaitingForApproval` tinyint(1) NOT NULL,
                               `RequestedPermissionLevel` int NOT NULL,
                               CONSTRAINT `PK_Permissions` PRIMARY KEY (`UserId`, `SettingGroupId`),
                               CONSTRAINT `FK_Permissions_SettingGroups_SettingGroupId` FOREIGN KEY (`SettingGroupId`) REFERENCES `SettingGroups` (`Id`) ON DELETE CASCADE,
                               CONSTRAINT `FK_Permissions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Permissions_SettingGroupId` ON `Permissions` (`SettingGroupId`);

ALTER TABLE `Settings` ADD CONSTRAINT `FK_Settings_SettingGroups_SettingGroupId` FOREIGN KEY (`SettingGroupId`) REFERENCES `SettingGroups` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230319144328_Permissions', '6.0.10');

COMMIT;
