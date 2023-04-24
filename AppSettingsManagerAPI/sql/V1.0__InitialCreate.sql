CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                                                       `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
                                                       `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
                                                       CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

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

CREATE TABLE `Settings` (
                            `SettingGroupId` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `Version` int NOT NULL,
                            `Input` longtext CHARACTER SET utf8mb4 NOT NULL,
                            `IsCurrent` tinyint(1) NOT NULL,
                            `CreatedBy` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `LastUpdatedAt` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
                            `CreatedAt` datetime(6) NOT NULL,
                            CONSTRAINT `PK_Settings` PRIMARY KEY (`SettingGroupId`, `Version`),
                            CONSTRAINT `FK_Settings_SettingGroups_SettingGroupId` FOREIGN KEY (`SettingGroupId`) REFERENCES `SettingGroups` (`Id`) ON DELETE CASCADE
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

CREATE INDEX `IX_Settings_IsCurrent` ON `Settings` (`IsCurrent`);

CREATE INDEX `IX_Settings_SettingGroupId` ON `Settings` (`SettingGroupId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230417011220_InitialCreate', '6.0.10');

COMMIT;

