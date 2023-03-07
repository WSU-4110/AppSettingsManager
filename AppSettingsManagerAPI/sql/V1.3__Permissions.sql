START TRANSACTION;

ALTER TABLE `Settings` DROP FOREIGN KEY `FK_Settings_BaseUsers_BaseUserUserId`;

DROP TABLE `BaseUsers`;

ALTER TABLE `Settings` DROP INDEX `IX_Settings_BaseUserUserId`;

ALTER TABLE `Settings` DROP COLUMN `BaseUserUserId`;

ALTER TABLE `Settings` RENAME COLUMN `Id` TO `SettingGroupId`;

ALTER TABLE `Settings` RENAME INDEX `IX_Settings_Id` TO `IX_Settings_SettingGroupId`;

CREATE TABLE `SettingGroups`
(
    `SettingGroupId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedBy`      longtext CHARACTER SET utf8mb4 NOT NULL,
    `LastUpdatedAt`  timestamp(6)                       NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP (6),
    CONSTRAINT `PK_SettingGroups` PRIMARY KEY (`SettingGroupId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users`
(
    `UserId`   varchar(36) CHARACTER SET utf8mb4 NOT NULL,
    `Password` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
    `Email`    varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Permissions`
(
    `UserId`                   varchar(36) CHARACTER SET utf8mb4  NOT NULL,
    `SettingGroupId`           varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CurrentPermissionLevel`   int                                NOT NULL,
    `ApprovedBy`               longtext CHARACTER SET utf8mb4 NOT NULL,
    `WaitingForApproval`       tinyint(1) NOT NULL,
    `RequestedPermissionLevel` int                                NOT NULL,
    CONSTRAINT `PK_Permissions` PRIMARY KEY (`UserId`, `SettingGroupId`),
    CONSTRAINT `FK_Permissions_SettingGroups_SettingGroupId` FOREIGN KEY (`SettingGroupId`) REFERENCES `SettingGroups` (`SettingGroupId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Permissions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Permissions_SettingGroupId` ON `Permissions` (`SettingGroupId`);

ALTER TABLE `Settings`
    ADD CONSTRAINT `FK_Settings_SettingGroups_SettingGroupId` FOREIGN KEY (`SettingGroupId`) REFERENCES `SettingGroups` (`SettingGroupId`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES (''20230227202735_Permissions'', ''6.0.10 '');

COMMIT;