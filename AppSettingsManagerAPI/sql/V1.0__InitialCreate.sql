CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
    ) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `BaseUsers` (
                             `UserId` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                             `UserName` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
                             `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
                             CONSTRAINT `PK_BaseUsers` PRIMARY KEY (`UserId`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_BaseUsers_UserName` ON `BaseUsers` (`UserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230206200423_InitialCreate', '6.0.10');

COMMIT;